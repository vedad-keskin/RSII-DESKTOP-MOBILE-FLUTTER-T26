using System;
using System.Collections.Generic;
using System.Linq;
using eCommerce.Model.Requests;
using eCommerce.Model.Responses;
using eCommerce.Model.SearchObjects;
using eCommerce.Services.Database;
using FluentValidation;

namespace eCommerce.Services
{
    public class GenderService : BaseCRUDService<Gender, GenderResponse, GenderSearch, GenderInsertRequest, GenderUpdateRequest>, IGenderService
    {
        public GenderService(ECommerceDbContext dbContext, MapsterMapper.IMapper mapper, IValidator<GenderInsertRequest> insertValidator, IValidator<GenderUpdateRequest> updateValidator) : base(dbContext, mapper, insertValidator, updateValidator)
        {
        }

        protected override IEnumerable<Gender> ApplyFilters(IEnumerable<Gender> query, GenderSearch? search)
        {
            if (search != null)
            {
                if (!string.IsNullOrWhiteSpace(search.Name))
                {
                    query = query.Where(u => u.Name.Contains(search.Name, StringComparison.OrdinalIgnoreCase));
                }

            }

            return query;
        }
    }
}
