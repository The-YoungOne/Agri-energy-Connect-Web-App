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
        public ProductCategory Category { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public DateTime ProductionDate { get; set; }
        [Required]
        [Url]
        public string ImageUrl { get; set; }

        //foreign farmer id referrence
        [Required]
        public int FarmerId { get; set; }
        [ForeignKey("FarmerId")]
        public FarmerModel Farmer { get; set; }
    }

    public enum ProductCategory
    {
        Vegetable,
        Fruit,
        Grain,
        Protein,
        Dairy,
        Oil,
        Fat,
        Sugar,
        Beverage
    }
}
