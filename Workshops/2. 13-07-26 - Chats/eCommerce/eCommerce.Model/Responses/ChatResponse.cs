namespace eCommerce.Model.Responses;

public class ChatResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int User1Id { get; set; }
    public int User2Id { get; set; }


    public int OtherUserId { get; set; }

    public string OtherUserFirstName { get; set; } = string.Empty;
    public string OtherUserLastName { get; set; } = string.Empty;


    public string? LastMessageContent { get; set; } 
    public DateTime? LastMessageCreatedAt { get; set; }

}
