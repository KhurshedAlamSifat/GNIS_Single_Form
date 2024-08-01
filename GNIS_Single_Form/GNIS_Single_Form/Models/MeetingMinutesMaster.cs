using System.ComponentModel.DataAnnotations;

namespace GNIS_Single_Form.Models
{
    public class MeetingMinutesMaster
    {
        [Key]
        public int Id { get; set; }

        public int CustomerId { get; set; }

        [Required]
        public string CustomerType { get; set; } // Corporate or Individual

        [Required]
        public DateTime MeetingDate { get; set; }

        [Required]
        public string MeetingTime { get; set; } // Store as string for formatted time

        [Required]
        [StringLength(200)]
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

        public ICollection<MeetingMinutesDetail> MeetingMinutesDetails { get; set; }
    }
}
