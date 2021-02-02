using OrderCheck.Models;
using System.Threading.Tasks;

namespace OrderCheck.Web.Services
{
    public interface IUserService
    {
        Task<User> GetUserAsync();
    }
}
