using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Model.Exceptions;
using eCommerce.Model.Requests;
using eCommerce.Model.Responses;
using eCommerce.Model.SearchObjects;
using eCommerce.Services.Database;
using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;

namespace eCommerce.Services
{
    public class ChatMessageIB180079Service
        : BaseCRUDService<ChatMessageIB180079, ChatMessageIB180079Response, ChatMessageIB180079Search, ChatMessageIB180079InsertRequest, ChatMessageIB180079UpdateRequest>,
            IChatMessageIB180079Service
    {
        public ChatMessageIB180079Service(
            ECommerceDbContext dbContext,
            IMapper mapper,
            IValidator<ChatMessageIB180079InsertRequest> insertValidator,
            IValidator<ChatMessageIB180079UpdateRequest> updateValidator)
            : base(dbContext, mapper, insertValidator, updateValidator)
        {
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
            var validationResult = await _insertValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => _mapper.Map<ValidationFailure>(e));
                throw new ValidationException(errors);
            }

            var chat = await _dbContext.ChatsIB180079.FindAsync(request.ChatId);
            if (chat == null || (chat.User1Id != request.SenderUserId && chat.User2Id != request.SenderUserId))
            {
                throw new ClinetException("Poruku može poslati samo učesnik razgovora.");
            }

            var entity = MapInsertRequestToEntity(request);
            entity.Content = request.Content.Trim();
            entity.CreatedAt = System.DateTime.UtcNow;

            _dbContext.ChatMessagesIB180079.Add(entity);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<ChatMessageIB180079Response>(entity);
        }
    }
}
