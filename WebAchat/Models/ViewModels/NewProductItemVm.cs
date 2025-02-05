using System.ComponentModel.DataAnnotations;

namespace WebAchat.Models.ViewModels
{
    public class NewTempProductVm
    {
        [Required] public string Name { get; set; }
        public string Description { get; set; }
        [Required] public decimal Price { get; set; }
        [Required] public int Quantity { get; set; }
        [Required] public int CategoryId { get; set; }
        [Required] public int ManufacturerId { get; set; }
    }
}
