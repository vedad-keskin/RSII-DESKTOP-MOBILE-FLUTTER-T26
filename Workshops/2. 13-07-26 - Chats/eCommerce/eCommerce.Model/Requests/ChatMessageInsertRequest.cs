using System.ComponentModel.DataAnnotations;

namespace eCommerce.Model.Requests
{
    public class ChatMessageInsertRequest
    {
        public int ChatId { get; set; }
        public int SenderId { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

    }
}
