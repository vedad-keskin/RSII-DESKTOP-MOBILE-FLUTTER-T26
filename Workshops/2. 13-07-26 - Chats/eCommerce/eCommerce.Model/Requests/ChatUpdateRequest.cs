using System.ComponentModel.DataAnnotations;

namespace eCommerce.Model.Requests
{
    public class ChatUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
