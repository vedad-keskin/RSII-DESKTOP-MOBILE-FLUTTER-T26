using eCommerce.Model.Requests;
using eCommerce.Model.Responses;
using eCommerce.Model.SearchObjects;
using System.Threading.Tasks;

namespace eCommerce.Services
{
    public interface IUserActivityService : IBaseCRUDService<UserActivityResponse, UserActivitySearch, UserActivityInsertRequest, UserActivityUpdateRequest>
    {
        Task<UserActivityResponse> ChangeStatusAsync(int id, string status);
    }
}
