using eCommerce.Services;
using eCommerce.Model.Responses;
using eCommerce.Model.SearchObjects;
using Microsoft.AspNetCore.Mvc;
using eCommerce.Model.Requests;

namespace eCommerce.WebAPI.Controllers;

public class ChatController : BaseCRUDController<ChatResponse, ChatSearchObject, ChatInsertRequest, ChatUpdateRequest, IChatService>
{
    public ChatController(IChatService chatService) : base(chatService)
    {
    }
}
