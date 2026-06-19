using System.ComponentModel.DataAnnotations;

namespace eCommerce.Model.Requests
{
    public class GenderInsertRequest
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

    }
}
