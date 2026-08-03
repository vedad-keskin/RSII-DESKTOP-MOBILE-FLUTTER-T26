using eCommerce.Model.Requests;
using eCommerce.Model.Responses;
using eCommerce.Model.SearchObjects;
using eCommerce.Services;

namespace eCommerce.WebAPI.Controllers;

public class ChatMessageIB180079Controller
    : BaseCRUDController<ChatMessageIB180079Response, ChatMessageIB180079Search, ChatMessageIB180079InsertRequest, ChatMessageIB180079UpdateRequest, IChatMessageIB180079Service>
{
    public ChatMessageIB180079Controller(IChatMessageIB180079Service messageService) : base(messageService)
    {
    }
}
