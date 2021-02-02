using Microsoft.AspNetCore.Http;
using OrderCheck.DAL.Interfaces;
using OrderCheck.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OrderCheck.Web.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public UserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<User> GetUserAsync()
        {
            var userName = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _userRepository.GetUserByNameAsync(userName);
        }
    }
}
