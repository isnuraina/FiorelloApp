using System.ComponentModel.DataAnnotations;

namespace FiorelloApp.Models
{
    public class Setting:BaseEntity
    {
        [Required]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
