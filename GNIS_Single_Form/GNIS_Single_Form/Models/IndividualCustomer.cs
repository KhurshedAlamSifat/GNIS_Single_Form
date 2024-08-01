using System.ComponentModel.DataAnnotations;

namespace GNIS_Single_Form.Models
{
    public class IndividualCustomer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
