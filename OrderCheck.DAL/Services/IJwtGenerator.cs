using OrderCheck.Models;

namespace OrderCheck.DAL.Services
{
    public interface IJwtGenerator
    {
        string CreateToken(User user);
    }
}
