using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderCheck.DAL.Interfaces;
using OrderCheck.Models;
using OrderCheck.Web.Services;

namespace OrderCheck.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IEstateRepository _estateRepository;
        private readonly IUserService _userService;
        private readonly IImageService _imageService;

        public DocumentController(IDocumentRepository documentRepository, IUserService userService, IOrganizationRepository organizationRepository, IEstateRepository estateRepository, IImageService imageService)
        {
            _documentRepository = documentRepository;
            _userService = userService;
            _organizationRepository = organizationRepository;
            _estateRepository = estateRepository;
            _imageService = imageService;
        }


        [HttpGet]
        public async Task<ActionResult<List<Document>>> GetItems()
        {
            try
            {
                var items = await _documentRepository.DocumentsByOwnerIdAsync(_userService.GetUserAsync().Result.Id);

                return items.OrderByDescending(e => e.DocumentId).ToList();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("{guid}")]
        public async Task<IActionResult> GetImage(Guid guid, [FromQuery] string type, [FromQuery] string size)
        {
            try
            {
                var item = await _documentRepository.DocumentByGuidAsync(guid);

                if (item == null)
                    return NotFound();

                string filePath = "";

                if (type == "doc")
                    filePath = item.ImagePath;
                else
                    filePath = item.CheckImagePath;

                var fi = new FileInfo(filePath);

                if (size == "thumb")
                {
                    filePath = Path.Combine(fi.DirectoryName, Path.GetFileNameWithoutExtension(filePath) + "_thumb" + fi.Extension);
                    fi = new FileInfo(filePath);
                }

                if (!fi.Exists)
                    return BadRequest("Файл документа не найден");

                var fileStream = System.IO.File.OpenRead(fi.FullName);

                return File(fileStream, "image/jpeg");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Document>> CreateItem([FromForm] Document item, [FromForm] IFormFile docFile, [FromForm] IFormFile checkFile)
        {
            if (docFile == null || docFile.Length == 0)
                return BadRequest(new { error = "Не выбран файл" });

            if (item.EstateId == 0 || !_estateRepository.ExistsAsync(item.EstateId).Result)
                return BadRequest(new { error = "Не указан объект" });

            if (item.OrganizationId == 0 || !_organizationRepository.ExistsAsync(item.OrganizationId).Result)
                return BadRequest(new { error = "Не указана организация" });

            try
            {
                // сохранение файла...вынести в отдельный сервис.
                var newItem = new Document()
                {
                    Guid = Guid.NewGuid(),

                    Paid = item.Paid,
                    Debt = item.Debt,
                    Note = item.Note,

                    DateEnd = item.DateEnd,
                    DateStart = item.DateStart,

                    OrganizationId = item.OrganizationId,
                    EstateId = item.EstateId,

                    OwnerId = _userService.GetUserAsync().Result.Id,
                    Created = DateTime.Now,
                    QrInfo = await _imageService.QrInfo(docFile)
                };

                newItem.ImagePath = _imageService.CropAndSaveImage(newItem.Guid, docFile);
                

                if (checkFile != null && checkFile.Length > 0)
                    newItem.CheckImagePath = _imageService.CropAndSaveImage(newItem.Guid, checkFile, "_check");

                await _documentRepository.CreateAsync(newItem);

                return newItem;
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult<Document>> UpdateItem([FromForm] Document item, [FromForm] IFormFile docFile, [FromForm] IFormFile checkFile)
        {
            var existItem = await _documentRepository.DocumentByGuidAsync(item.Guid);

            if (existItem == null)
                return BadRequest(new { error = "Документ с указанным GUID не найден" });

            var organization = await _organizationRepository.FindByIdAsync(item.OrganizationId);

            if (organization == null)
                return BadRequest(new { error = "Организация не найдена" });

            var estate = await _estateRepository.FindByIdAsync(item.EstateId);

            if (estate == null)
                return BadRequest(new { error = "Объект не найден" });

            try
            {
                if (existItem == null)
                    return NotFound();

                existItem.Organization = organization;
                existItem.Estate = estate;

                existItem.Note = item.Note;

                existItem.Paid = item.Paid;
                existItem.Debt = item.Debt;

                existItem.DateEnd = item.DateEnd;
                existItem.DateStart = item.DateStart;

                if (docFile != null && docFile.Length > 0)
                {
                    existItem.ImagePath = _imageService.CropAndSaveImage(existItem.Guid, docFile);
                    existItem.QrInfo = await _imageService.QrInfo(docFile);
                }

                if (checkFile != null && checkFile.Length > 0)
                    existItem.CheckImagePath = _imageService.CropAndSaveImage(existItem.Guid, checkFile, "_check");

                await _documentRepository.EditAsync(existItem);

                return existItem;
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("info")]
        public async Task<Object> GetQrInfo([FromForm] IFormFile docFile)
        {
            try
            {
                var result = new { info = await _imageService.QrInfo(docFile) };
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteItem([FromForm] Guid guid)
        {
            try
            {
                var existItem = await _documentRepository.DocumentByGuidAsync(guid);

                if (existItem == null)
                    return NotFound();

                if (existItem.OwnerId != _userService.GetUserAsync().Result.Id)
                    return BadRequest("Ошибка доступа к записи - вы не владелец");

                await _documentRepository.RemoveAsync(existItem);

                return Ok(new { });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
