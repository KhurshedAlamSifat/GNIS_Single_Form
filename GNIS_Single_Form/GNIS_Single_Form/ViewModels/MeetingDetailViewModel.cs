using System.ComponentModel.DataAnnotations;

namespace GNIS_Single_Form.ViewModels
{
    public class MeetingDetailViewModel
    {
        [Required]
        public int ProductServiceId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [StringLength(50)]
        public string Unit { get; set; }
    }
}
