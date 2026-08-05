namespace eCommerce.Model.Responses;

public class ChatMessageResponse
{
    public int Id { get; set; }

    public int ChatId { get; set; }
    public int SenderId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } 


}
