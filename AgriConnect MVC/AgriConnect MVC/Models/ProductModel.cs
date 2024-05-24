using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AgriConnect_MVC.Models
{
    public class ProductModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public DateTime ProductionDate { get; set; }

        //foreign farmer id referrence
        [Required]
        public int FarmerId { get; set; }
        [ForeignKey("FarmerId")]
        public FarmerModel Farmer { get; set; }
    }
}
