using System.ComponentModel.DataAnnotations;

namespace FiorelloApp.Areas.Admin.ViewModels.Slider
{
    public class SliderCreateVM
    {
        [Required]
        public IFormFile Photo { get; set; }
    }
}
