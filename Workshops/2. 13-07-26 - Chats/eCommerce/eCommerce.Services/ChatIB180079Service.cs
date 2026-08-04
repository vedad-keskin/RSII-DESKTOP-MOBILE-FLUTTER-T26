using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Model.Exceptions;
using eCommerce.Model.Requests;
using eCommerce.Model.Responses;
using eCommerce.Model.SearchObjects;
using eCommerce.Services.Database;
using FluentValidation;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Services
{
    public class ChatIB180079Service
        : BaseCRUDService<ChatIB180079, ChatIB180079Response, ChatIB180079Search, ChatIB180079InsertRequest, ChatIB180079UpdateRequest>,
            IChatIB180079Service
    {
        public ChatIB180079Service(
            ECommerceDbContext dbContext,
            IMapper mapper,
            IValidator<ChatIB180079InsertRequest> insertValidator,
            IValidator<ChatIB180079UpdateRequest> updateValidator)
            : base(dbContext, mapper, insertValidator, updateValidator)
        {
        }

        protected override IEnumerable<ChatIB180079> ApplyFilters(IEnumerable<ChatIB180079> query, ChatIB180079Search? search)
        {
            if (search != null)
            {
                if (search.UserId.HasValue)
                {
                    query = query.Where(c => c.User1Id == search.UserId.Value || c.User2Id == search.UserId.Value);
                }

                if (search.OtherUserId.HasValue)
                {
                    query = query.Where(c => c.User1Id == search.OtherUserId.Value || c.User2Id == search.OtherUserId.Value);
                }
            }

            return query;
        }


        public override async Task<PageResult<ChatIB180079Response>> GetAllAsync(ChatIB180079Search? search = null)
        {
            var chats = await _dbContext.ChatsIB180079
                .Include(c => c.User1)
                .Include(c => c.User2)
                .Include(c => c.Messages)
                .ToListAsync();

            var currentUserId = search?.UserId ?? 0;

            var items = ApplyFilters(chats, search)
                .Select(c =>
                {
                    var other = c.User1Id == currentUserId ? c.User2 : c.User1;
                    var lastMessage = c.Messages.OrderByDescending(m => m.CreatedAt).FirstOrDefault();

                    return new ChatIB180079Response
                    {
                        Id = c.Id,
                        Name = c.Name,
                        CreatedAt = c.CreatedAt,
                        User1Id = c.User1Id,
                        User2Id = c.User2Id,
                        OtherUserId = other.Id,
                        OtherUserFirstName = other.FirstName,
                        OtherUserLastName = other.LastName,
                        LastMessageContent = lastMessage?.Content,
                        LastMessageCreatedAt = lastMessage?.CreatedAt
                    };
                })
                .OrderByDescending(r => r.LastMessageCreatedAt ?? r.CreatedAt)
                .ToList();

            return new PageResult<ChatIB180079Response> { Items = items };
        }

        public override async Task<ChatIB180079Response> InsertAsync(ChatIB180079InsertRequest request)
        {
            var exists = await _dbContext.ChatsIB180079.AnyAsync(c =>
                (c.User1Id == request.User1Id && c.User2Id == request.User2Id) ||
                (c.User1Id == request.User2Id && c.User2Id == request.User1Id));

            if (exists)
            {
                throw new ClinetException("Razgovor već postoji.");
            }

            return await base.InsertAsync(request);
        }
    }
}
