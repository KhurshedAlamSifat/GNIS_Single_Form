using GNIS_Single_Form.Models;
using System.ComponentModel.DataAnnotations;

namespace GNIS_Single_Form.ViewModels
{
    public class MeetingViewModel
    {
        [Required]
        public int SelectedCustomerId { get; set; }

        [Required]
        public string SelectedCustomerType { get; set; }

        [Required]
        public DateTime MeetingDate { get; set; }

        [Required]
        public TimeSpan MeetingTime { get; set; }

        [Required]
        public string MeetingPlace { get; set; }

        [Required]
        public string AttendsClient { get; set; }

        [Required]
        public string AttendsHost { get; set; }

        [Required]
        public string MeetingAgenda { get; set; }

        [Required]
        public string MeetingDiscussion { get; set; }

        [Required]
        public string MeetingDecision { get; set; }

        public string MeetingDetailsJson { get; set; }
    }

}
