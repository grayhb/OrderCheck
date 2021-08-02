using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IO;

namespace OrderCheck.Web.Extensions
{
    public static class FileExtensions
    {
        public static async Task<string> SaveToTempFile(this IFormFile file)
        {
            var tempFileName = Path.GetTempFileName();

            if (file.Length > 0)
            {
                await using var stream = new FileStream(tempFileName, FileMode.Create);
                await file.CopyToAsync(stream);
            }

            return tempFileName;
        }
    }
}
