namespace eCommerce.Model.Requests
{
    public class ChatMessageIB180079InsertRequest
    {
        public int ChatId { get; set; }
        public int SenderUserId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
