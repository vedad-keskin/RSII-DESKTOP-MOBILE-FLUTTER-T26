using eCommerce.Model.Requests;
using eCommerce.Model.Responses;
using eCommerce.Model.SearchObjects;
using eCommerce.Services;

namespace eCommerce.WebAPI.Controllers;

public class ChatIB180079Controller
    : BaseCRUDController<ChatIB180079Response, ChatIB180079Search, ChatIB180079InsertRequest, ChatIB180079UpdateRequest, IChatIB180079Service>
{
    public ChatIB180079Controller(IChatIB180079Service chatService) : base(chatService)
    {
    }
}
