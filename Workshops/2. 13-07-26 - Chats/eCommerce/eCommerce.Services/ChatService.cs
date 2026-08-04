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
    public class ChatService : BaseCRUDService<ChatIB180079, ChatResponse, ChatSearchObject, ChatInsertRequest, ChatUpdateRequest>, IChatService
    {
        public ChatService(ECommerceDbContext dbContext,
            MapsterMapper.IMapper mapper,
            IValidator<ChatInsertRequest> insertValidator,
            IValidator<ChatUpdateRequest> updateValidator
            ) : base(dbContext, mapper, insertValidator, updateValidator)
        {
        }

        protected override IEnumerable<ChatIB180079> ApplyFilters(IEnumerable<ChatIB180079> query, ChatSearchObject? search)
        {
            if (search != null)
            {


                if (search.User1Id.HasValue)
                {
                    query = query.Where(u => u.User1Id == search.User1Id.Value || u.User2Id == search.User1Id.Value);
                }

                if (search.User2Id.HasValue)
                {
                    query = query.Where(u => u.User1Id == search.User2Id.Value || u.User2Id == search.User2Id.Value);
                }


            }

            return query;
        }

        public override async Task<PageResult<ChatResponse>> GetAllAsync(ChatSearchObject? search = null)
        {

            var chats = await _dbContext.Chats
                .Include(x => x.User1)
                .Include(x => x.User2)
                .Include(x => x.ChatMessages)
                .ToListAsync();

            var currentUser1Id = search?.User1Id ?? 0;

            var items = ApplyFilters(chats, search)
                .Select(c =>
                {

                    var otherUser = c.User1Id == currentUser1Id ? c.User2 : c.User1;

                    var lastMessage = c.ChatMessages.OrderByDescending(x => x.CreatedAt).FirstOrDefault();


                   return new ChatResponse
                   {
                       Id = c.Id,
                       CreatedAt = c.CreatedAt,
                       User1Id = c.User1Id,
                       User2Id = c.User2Id,
                       OtherUserId = otherUser.Id,
                       OtherUserFirstName = otherUser.FirstName,
                       OtherUserLastName = otherUser.LastName,
                       LastMessageContent = lastMessage?.Content,
                       LastMessageCreatedAt = lastMessage?.CreatedAt,

                   };


                })
                .OrderByDescending(x => x.LastMessageCreatedAt ?? x.CreatedAt )
                .ToList(); 



            return new PageResult<ChatResponse> { Items = items , TotalCount = items.Count() };

        }


        public override async Task<ChatResponse> InsertAsync(ChatInsertRequest request)
        {

            var exists = await _dbContext.Chats.AnyAsync(x => (x.User1Id == request.User1Id && x.User2Id == request.User2Id ) || 
            (x.User1Id == request.User2Id && x.User2Id == request.User1Id));

            if (exists)
            {
                throw new ClinetException("Chat already exists.");

            }

            return await base.InsertAsync(request);
        }





    }
}
