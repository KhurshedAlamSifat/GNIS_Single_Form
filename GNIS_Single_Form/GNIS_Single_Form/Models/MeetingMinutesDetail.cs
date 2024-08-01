using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GNIS_Single_Form.Models
{
    public class MeetingMinutesDetail
    {
        [Key]
        public int Id { get; set; }

        public int MeetingMinutesMasterId { get; set; }

        [ForeignKey("MeetingMinutesMasterId")]
        public MeetingMinutesMaster MeetingMinutesMaster { get; set; }

        [Required]
        public int ProductServiceId { get; set; }

        [ForeignKey("ProductServiceId")]
        public ProductService ProductService { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [StringLength(50)]
        public string Unit { get; set; }
    }
}
