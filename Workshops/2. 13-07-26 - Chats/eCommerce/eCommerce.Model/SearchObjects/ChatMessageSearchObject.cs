namespace eCommerce.Model.SearchObjects
{
    public class ChatMessageSearchObject : BaseSearchObject
    {
        public int? ChatId { get; set; }
        public int? SenderId { get; set; }
    }
}