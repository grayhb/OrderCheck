using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderCheck.DAL.Interfaces;
using OrderCheck.Models;
using OrderCheck.Web.Services;

namespace OrderCheck.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstatesController : ControllerBase
    {
        private readonly IEstateRepository _estateRepository;
        private readonly IUserService _userService;

        public EstatesController(IEstateRepository estateRepository, IUserService userService) {
            _estateRepository = estateRepository;
            _userService = userService;
        }


        [HttpGet]
        public async Task<ActionResult<List<Estate>>> GetItems()
        {
            try
            {
                var items = await _estateRepository.EstatesByOwnerIdAsync(_userService.GetUserAsync().Result.Id);

                foreach (var item in items)
                    item.EstateAddress = string.IsNullOrEmpty(item.EstateAddress) ? "" : item.EstateAddress;

                return items.OrderBy(e => e.EstateName).ToList();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Estate>> CreateItem([FromForm]Estate estate)
        {
            if (string.IsNullOrEmpty(estate.EstateName))
                return BadRequest(new { error = "Название обязательное поле" });

            try
            {
                var newEstate = new Estate() {
                    EstateName = estate.EstateName,
                    EstateAddress = estate.EstateAddress,
                    OwnerId = _userService.GetUserAsync().Result.Id
                };

                await _estateRepository.CreateAsync(newEstate);

                return newEstate;
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult<Estate>> UpdateItem([FromForm] Estate estate)
        {
            if (string.IsNullOrEmpty(estate.EstateName))
                return BadRequest(new { error = "Название обязательное поле" });

            try
            {
                var existItem = await _estateRepository.FindByIdAsync(estate.EstateId);

                if (existItem == null)
                    return NotFound();


                existItem.EstateAddress = estate.EstateAddress;
                existItem.EstateName = estate.EstateName;

                await _estateRepository.EditAsync(existItem);

                return existItem;
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteItem([FromForm]int EstateId)
        {
            try
            {
                var existItem = await _estateRepository.FindByIdAsync(EstateId);

                if (existItem == null)
                    return NotFound();

                if (existItem.OwnerId != _userService.GetUserAsync().Result.Id)
                    return BadRequest("Ошибка доступа к записи - вы не владелец");

                await _estateRepository.RemoveAsync(existItem.EstateId);

                return Ok(new { });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
