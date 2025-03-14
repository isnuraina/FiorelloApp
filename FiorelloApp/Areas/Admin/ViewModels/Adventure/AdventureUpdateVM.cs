using System.ComponentModel.DataAnnotations;

namespace FiorelloApp.Areas.Admin.ViewModels.Adventure
{
    public class AdventureUpdateVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string? Image { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
    }
}
