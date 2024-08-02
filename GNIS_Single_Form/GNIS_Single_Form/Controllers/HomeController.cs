using GNIS_Single_Form.Models;
using GNIS_Single_Form.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;


namespace GNIS_Single_Form.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetCustomers(string type)
        {
            var customers = type == "corporate"
                ? await _context.CorporateCustomers
                    .Select(c => new { value = c.Id, text = c.Name })
                    .ToListAsync()
                : await _context.IndividualCustomers
                    .Select(c => new { value = c.Id, text = c.Name })
                    .ToListAsync();

            return Json(customers);
        }

        [HttpGet]
        public async Task<JsonResult> GetProducts()
        {
            var products = await _context.ProductServices
                .Select(p => new { value = p.Id, text = p.Name, unit = p.Unit })
                .ToListAsync();

            return Json(products);
        }

        [HttpPost]
        public async Task<IActionResult> Save(MeetingViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(viewModel.MeetingDetailsJson))
                {
                    List<MeetingDetailViewModel> meetingDetails;
                    try
                    {
                        meetingDetails = JsonConvert.DeserializeObject<List<MeetingDetailViewModel>>(viewModel.MeetingDetailsJson);
                    }
                    catch (JsonException ex)
                    {
                        TempData["ErrorMessage"] = "Error parsing meeting details.";
                        return RedirectToAction("Index");
                    }
                    using (var transaction = await _context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var newMasterId = new SqlParameter("@NewMasterId", SqlDbType.Int)
                            {
                                Direction = ParameterDirection.Output
                            };

                            await _context.Database.ExecuteSqlRawAsync(
                                "EXEC Meeting_Minutes_Master_Save_SP @CustomerId, @CustomerType, @MeetingDate, @MeetingTime, @MeetingPlace, @AttendsClient, @AttendsHost, @MeetingAgenda, @MeetingDiscussion, @MeetingDecision, @NewMasterId OUT",
                                new SqlParameter("@CustomerId", viewModel.SelectedCustomerId),
                                new SqlParameter("@CustomerType", viewModel.SelectedCustomerType),
                                new SqlParameter("@MeetingDate", viewModel.MeetingDate),
                                new SqlParameter("@MeetingTime", viewModel.MeetingTime.ToString(@"hh\:mm\:ss")), // Ensure the format matches
                                new SqlParameter("@MeetingPlace", viewModel.MeetingPlace),
                                new SqlParameter("@AttendsClient", viewModel.AttendsClient),
                                new SqlParameter("@AttendsHost", viewModel.AttendsHost),
                                new SqlParameter("@MeetingAgenda", viewModel.MeetingAgenda),
                                new SqlParameter("@MeetingDiscussion", viewModel.MeetingDiscussion),
                                new SqlParameter("@MeetingDecision", viewModel.MeetingDecision),
                                newMasterId
                            );

                            int masterId = (int)newMasterId.Value;

                            foreach (var detail in meetingDetails)
                            {
                                await _context.Database.ExecuteSqlRawAsync(
                                    "EXEC Meeting_Minutes_Details_Save_SP @MasterId, @ProductServiceId, @Quantity, @Unit",
                                    new SqlParameter("@MasterId", masterId),
                                    new SqlParameter("@ProductServiceId", detail.ProductServiceId),
                                    new SqlParameter("@Quantity", detail.Quantity),
                                    new SqlParameter("@Unit", detail.Unit)
                                );
                            }

                            await transaction.CommitAsync();

                            TempData["SuccessMessage"] = "Meeting details saved successfully!";
                            return RedirectToAction("Index");
                        }
                        catch (SqlException ex)
                        {
                            TempData["ErrorMessage"] = "Database error occurred.";
                            return RedirectToAction("Index");
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            TempData["ErrorMessage"] = "An unexpected error occurred.";
                            return RedirectToAction("Index");
                        }
                    }
                }

                TempData["ErrorMessage"] = "Meeting details are required.";
                return RedirectToAction("Index");                
            }

            TempData["ErrorMessage"] = "There are validation errors. Please correct them and try again.";
            return RedirectToAction("Index");
        }
    }
}