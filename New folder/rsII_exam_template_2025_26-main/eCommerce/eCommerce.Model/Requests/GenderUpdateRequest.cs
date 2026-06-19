using System.ComponentModel.DataAnnotations;

namespace eCommerce.Model.Requests
{
    public class GenderUpdateRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

    }
}
