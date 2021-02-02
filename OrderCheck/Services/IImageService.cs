using Microsoft.AspNetCore.Http;
using System;

namespace OrderCheck.Web.Services
{
    public interface IImageService
    {
        string CropAndSaveImage(Guid guid, IFormFile file, string suffixFileName = "");

    }
}
