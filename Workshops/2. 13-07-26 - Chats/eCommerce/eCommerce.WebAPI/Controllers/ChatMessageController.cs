using eCommerce.Services;
using eCommerce.Model.Responses;
using eCommerce.Model.SearchObjects;
using Microsoft.AspNetCore.Mvc;
using eCommerce.Model.Requests;

namespace eCommerce.WebAPI.Controllers;

public class ChatMessageController : BaseCRUDController<ChatMessageResponse, ChatMessageSearchObject, ChatMessageInsertRequest, ChatMessageUpdateRequest, IChatMessageService>
{
    public ChatMessageController(IChatMessageService chatMessageService) : base(chatMessageService)
    {
    }
}
