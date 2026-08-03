using System;

namespace eCommerce.Model.Responses
{
    public class ChatMessageIB180079Response
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public int SenderUserId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
