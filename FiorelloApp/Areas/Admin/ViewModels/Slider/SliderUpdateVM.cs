using System.ComponentModel.DataAnnotations;

namespace FiorelloApp.Areas.Admin.ViewModels.Slider
{
    public class SliderUpdateVM
    {
        [Required]
        public IFormFile Photo { get; set; }
        public string? ImageUrl { get; set; }
    }
}
