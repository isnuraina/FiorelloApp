using System.ComponentModel.DataAnnotations;

namespace FiorelloApp.Areas.Admin.ViewModels.Adventure
{
    public class AdventureCreateVM
    {

        [Required]
        [MaxLength(10)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Price 0 və ya daha böyük olmalıdır!")]

        public int Price { get; set; }
        public string Image { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
    }
}
