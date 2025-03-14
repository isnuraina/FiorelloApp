using System.ComponentModel.DataAnnotations;

namespace FiorelloApp.Areas.Admin.ViewModels.Category
{
    public class CategoryUpdateVM
    {
        [Required]
        [MaxLength(10)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }

    }
}
