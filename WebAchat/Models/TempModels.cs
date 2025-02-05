using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAchat.Models
{
    public class TempBuyOrder
    {
        [Key]
        public int TempBuyOrderId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public List<TempOrderProduct> OrderProducts { get; set; } = new List<TempOrderProduct>();
    }

    public class TempOrderProduct
    {
        [Key]
        public int TempOrderProductId { get; set; }

        [ForeignKey("TempBuyOrder")]
        public int TempBuyOrderId { get; set; }
        public TempBuyOrder TempBuyOrder { get; set; }

        public int? ProductId { get; set; }
        public int? TempProductId { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("TempProductId")]
        public TempProduct TempProduct { get; set; }
    }

    public class TempProduct
    {
        [Key]
        public int TempProductId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public int ManufacturerId { get; set; }
    }
}