using eCommerce.Model.Exceptions;
using eCommerce.Model.Requests;
using eCommerce.Model.Responses;
using eCommerce.Model.SearchObjects;
using eCommerce.Services.Database;
using FluentValidation;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eCommerce.Services
{
    public class ChatMessageService : BaseCRUDService<ChatMessageIB180079, ChatMessageResponse, ChatMessageSearchObject, ChatMessageInsertRequest, ChatMessageUpdateRequest>, IChatMessageService
    {

        private readonly IAuthenticatedUserAccessor _userAccessor;
        public ChatMessageService(ECommerceDbContext dbContext,
            MapsterMapper.IMapper mapper,
            IValidator<ChatMessageInsertRequest> insertValidator,
            IValidator<ChatMessageUpdateRequest> updateValidator,
            IAuthenticatedUserAccessor userAccessor
            ) : base(dbContext, mapper, insertValidator, updateValidator)
        {
            _userAccessor = userAccessor;
        }

        protected override IEnumerable<ChatMessageIB180079> ApplyFilters(IEnumerable<ChatMessageIB180079> query, ChatMessageSearchObject? search)
        {
            if (search != null)
            {


                if (search.ChatId.HasValue)
                {
                    query = query.Where(u => u.ChatId == search.ChatId.Value );
                }

                if (search.SenderId.HasValue)
                {
                    query = query.Where(u => u.SenderId == search.SenderId.Value );
                }


            }

            return query;
        }




        public override async Task<ChatMessageResponse> InsertAsync(ChatMessageInsertRequest request)
        {

            var chat = await _dbContext.Chats.FindAsync(request.ChatId);



            if ( chat.User1Id != request.SenderId && chat.User2Id != request.SenderId  )
            {
                throw new ClinetException("Participant is not in this conversation.");

            }

            return await base.InsertAsync(request);
        }

        public override async Task DeleteAsync(int id)
        {

            var chatMessage = await _dbContext.ChatMessages.FindAsync(id);

            var userId = _userAccessor.GetUserId();

            if(userId == null || userId != chatMessage.SenderId)
            {

                throw new ClinetException("You can only delete messages you wrote.");

            }




            await base.DeleteAsync(id);
        }








    }
}
