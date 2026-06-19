using System.ComponentModel.DataAnnotations;

namespace eCommerce.Model.Requests
{
    public class UserActivityInsertRequest
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int ActivityId { get; set; }

        public string? Note { get; set; }
    }
}
