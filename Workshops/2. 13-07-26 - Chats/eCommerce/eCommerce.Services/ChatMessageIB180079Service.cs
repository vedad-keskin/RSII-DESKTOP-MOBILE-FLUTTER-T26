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

namespace eCommerce.Services
{
    public class ChatMessageIB180079Service
        : BaseCRUDService<ChatMessageIB180079, ChatMessageIB180079Response, ChatMessageIB180079Search, ChatMessageIB180079InsertRequest, ChatMessageIB180079UpdateRequest>,
            IChatMessageIB180079Service
    {
        private readonly IAuthenticatedUserAccessor _userAccessor;

        public ChatMessageIB180079Service(
            ECommerceDbContext dbContext,
            IMapper mapper,
            IValidator<ChatMessageIB180079InsertRequest> insertValidator,
            IValidator<ChatMessageIB180079UpdateRequest> updateValidator,
            IAuthenticatedUserAccessor userAccessor)
            : base(dbContext, mapper, insertValidator, updateValidator)
        {
            _userAccessor = userAccessor;
        }

        protected override IEnumerable<ChatMessageIB180079> ApplyFilters(IEnumerable<ChatMessageIB180079> query, ChatMessageIB180079Search? search)
        {
            if (search?.ChatId.HasValue == true)
            {
                query = query.Where(m => m.ChatId == search.ChatId.Value);
            }

            return query;
        }

        public override async Task<ChatMessageIB180079Response> InsertAsync(ChatMessageIB180079InsertRequest request)
        {
            var chat = await _dbContext.ChatsIB180079.FindAsync(request.ChatId);
            if (chat == null || (chat.User1Id != request.SenderUserId && chat.User2Id != request.SenderUserId))
            {
                throw new ClinetException("Poruku može poslati samo učesnik razgovora.");
            }

            return await base.InsertAsync(request);
        }

        public override async Task DeleteAsync(int id)
        {
            var entity = await _dbContext.ChatMessagesIB180079.FindAsync(id)
                ?? throw new KeyNotFoundException($"{nameof(ChatMessageIB180079)} with id {id} not found.");

            var userId = _userAccessor.GetUserId();
            if (userId == null || entity.SenderUserId != userId.Value)
            {
                throw new ClinetException("Možete obrisati samo vlastite poruke.");
            }

            _dbContext.ChatMessagesIB180079.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
