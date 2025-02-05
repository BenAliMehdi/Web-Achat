using System.ComponentModel.DataAnnotations;

namespace WebAchat.Models.ViewModels
{
    public class TempProductViewModel
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int ManufacturerId { get; set; }
    }
}
