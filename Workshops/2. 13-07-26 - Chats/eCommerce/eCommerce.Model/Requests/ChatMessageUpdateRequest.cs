using System.ComponentModel.DataAnnotations;

namespace eCommerce.Model.Requests
{
    public class ChatMessageUpdateRequest
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
