using FiorelloApp.Models;
using System.ComponentModel.DataAnnotations;

namespace FiorelloApp.Areas.Admin.ViewModels.Product
{
    public class ProductUpdateVM
    {
        [Required]
        public string Name { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public int Count { get; set; }
        public IFormFile[]? Photos { get; set; }
        public List<ProductImage>? Images { get; set; }
    }
}
