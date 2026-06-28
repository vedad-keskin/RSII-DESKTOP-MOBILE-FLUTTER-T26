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

        public DateTime DateAssigned { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Assigned";
    }
}
