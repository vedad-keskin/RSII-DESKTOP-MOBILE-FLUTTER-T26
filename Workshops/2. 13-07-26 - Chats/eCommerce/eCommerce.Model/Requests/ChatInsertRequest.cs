using System.ComponentModel.DataAnnotations;

namespace eCommerce.Model.Requests
{
    public class ChatInsertRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public int User1Id { get; set; }
        public int User2Id { get; set; }

    }
}
