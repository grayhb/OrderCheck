using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace OrderCheck.Web.Services
{
    public interface IImageService
    {
        string CropAndSaveImage(Guid guid, IFormFile file, string suffixFileName = "");

        Task<string> QrInfo(IFormFile file);
    }
}
