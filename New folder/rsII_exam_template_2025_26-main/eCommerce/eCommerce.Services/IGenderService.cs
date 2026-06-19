using eCommerce.Model.Requests;
using eCommerce.Model.Responses;
using eCommerce.Model.SearchObjects;

namespace eCommerce.Services
{
    public interface IGenderService : IBaseCRUDService<GenderResponse, GenderSearch, GenderInsertRequest, GenderUpdateRequest>
    {
    }
}
