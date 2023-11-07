using System.ComponentModel.DataAnnotations;

namespace Simple_E_Com.Models.ViewModels
{
    public class ProductVM
    {

        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = default!;
        [Required]
        public string Unit { get; set; } = default!;
        public double Price { get; set; }
        public double Quantity { get; set; }
        public IFormFile? PictureFile { get; set; }  
        public string? Image { get; set; } = default!;
    }
}
