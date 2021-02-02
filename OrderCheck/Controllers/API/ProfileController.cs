using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderCheck.DAL.Interfaces;
using OrderCheck.DAL.ViewModels;

namespace OrderCheck.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public ProfileController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<ActionResult<UserView>> GetProfile()
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                return await _userRepository.GetProfileByNameAsync(userName);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult<UserView>> SaveProfile([FromForm]UserView userView)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                var user = await _userRepository.GetUserByNameAsync(userName);
                user.DisplayName = userView.DisplayName;

                await _userRepository.EditAsync(user);

                return new UserView()
                {
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    Id = user.Id
                };
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }

        }
    }
}
