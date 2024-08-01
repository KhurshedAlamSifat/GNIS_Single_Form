using System.ComponentModel.DataAnnotations;

namespace GNIS_Single_Form.Models
{
    public class ProductService
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Unit { get; set; }
    }
}
