using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AgriConnect_MVC.Models
{
    public class FarmerModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FarmerId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Number { get; set; }


        // Navigation property for farmer products
        public List<ProductModel>? Products { get; set; } = new List<ProductModel>();
    }
}
