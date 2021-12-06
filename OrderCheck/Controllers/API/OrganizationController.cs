using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderCheck.DAL.Interfaces;
using OrderCheck.Models;
using OrderCheck.Web.Services;

namespace OrderCheck.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOrganizationRepository _organizationRepository;

        public OrganizationController(IUserService userService, IOrganizationRepository organizationRepository)
        {
            _userService = userService;
            _organizationRepository = organizationRepository;
        }


        [HttpGet]
        public async Task<ActionResult<List<Organization>>> GetItems()
        {
            try
            {
                var items = await _organizationRepository.OrganizationsByOwnerIdAsync(_userService.GetUserAsync().Result.Id);

                return items.OrderBy(e => e.OrganizationName).ToList();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Organization>> CreateItem([FromForm] Organization item)
        {
            if (string.IsNullOrEmpty(item.OrganizationName))
                return BadRequest(new { error = "Название обязательное поле" });

            try
            {
                var newItem = new Organization()
                {
                    OrganizationName = item.OrganizationName,
                    OwnerId = _userService.GetUserAsync().Result.Id
                };

                newItem.Inn = item.Inn ?? "";

                await _organizationRepository.CreateAsync(newItem);

                return newItem;
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult<Organization>> UpdateItem([FromForm] Organization item)
        {
            if (string.IsNullOrEmpty(item.OrganizationName))
                return BadRequest(new { error = "Название обязательное поле" });

            try
            {
                var existItem = await _organizationRepository.FindByIdAsync(item.OrganizationId);

                if (existItem == null)
                    return NotFound();

                existItem.OrganizationName = item.OrganizationName;
                existItem.Inn = item.Inn ?? "";

                await _organizationRepository.EditAsync(existItem);

                return existItem;
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteItem([FromForm] int id)
        {
            try
            {
                var existItem = await _organizationRepository.FindByIdAsync(id);

                if (existItem == null)
                    return NotFound();

                if (existItem.OwnerId != _userService.GetUserAsync().Result.Id)
                    return BadRequest("Ошибка доступа к записи - вы не владелец");

                await _organizationRepository.RemoveAsync(id);

                return Ok(new { });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


    }
}
