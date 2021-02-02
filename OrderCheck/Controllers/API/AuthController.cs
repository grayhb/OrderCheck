using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderCheck.DAL.Interfaces;
using OrderCheck.DAL.ViewModels;
using OrderCheck.Models;

namespace OrderCheck.Web.Controllers.API
{
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserView>> LoginAsync(string email, string password)
        {
            try
            {
                return await _userRepository.Login(email, password);
            }
            catch(Exception ex)
            {
                return BadRequest(new { error = ex.Message});
            }
        }

        [HttpPost("registration")]
        public async Task<ActionResult<UserView>> RegistrationAsync(User userData, string password)
        {
            try
            {
                return await _userRepository.Register(userData, password);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
