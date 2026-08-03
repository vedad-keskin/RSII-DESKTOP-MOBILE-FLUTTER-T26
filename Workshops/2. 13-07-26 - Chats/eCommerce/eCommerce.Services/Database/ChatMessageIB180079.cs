using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.Services.Database
{
    public class ChatMessageIB180079
    {
        [Key]
        public int Id { get; set; }

        public int ChatId { get; set; }

        [ForeignKey("ChatId")]
        public ChatIB180079 Chat { get; set; } = null!;

        public int SenderUserId { get; set; }

        [ForeignKey("SenderUserId")]
        public User Sender { get; set; } = null!;

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
