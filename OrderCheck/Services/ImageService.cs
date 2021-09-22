using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using OrderCheck.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using ZXing;
using ZXing.Common;

namespace OrderCheck.Web.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _env;

        public ImageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string CropAndSaveImage(Guid guid, IFormFile file, string suffixFileName = "")
        {
            var tempFilePath = SaveOriginalFile(file);

            var fileName = $"{guid}{suffixFileName}.jpg";
            var fileThumbName = $"{guid}{suffixFileName}_thumb.jpg";

            var imagePath = Path.Combine(_env.WebRootPath, "uploads", fileName);
            var imageThumbPath = Path.Combine(_env.WebRootPath, "uploads", fileThumbName);

            Directory.CreateDirectory(new FileInfo(imagePath).DirectoryName);

            var scaledImage = ResizeImage(tempFilePath, 1920);
            var thumbImage = ResizeImage(tempFilePath, 300);

            var codecInfo = GetEncoderInfo("image/jpeg");
            var encoder = Encoder.Quality;
            var encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(encoder, 90L);

            scaledImage.Save(imagePath, codecInfo, encoderParameters);
            thumbImage.Save(imageThumbPath, codecInfo, encoderParameters);

            return imagePath;
        }

        private Image ResizeImage(string filePath, int maxSize)
        {
            var image = Image.FromFile(filePath);

            if (image.Height <= maxSize && image.Width <= maxSize)
                return image;

            var aspect = (double)image.Width / (double)image.Height;

            var w = maxSize;
            var h = maxSize;

            if (image.Width > image.Height)
                h = (int)(maxSize / aspect);
            else
                w = (int)(maxSize * aspect);


            var destRect = new Rectangle(0, 0, w, h);
            var destImage = new Bitmap(w, h);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;

            //var scaledImage = image.GetThumbnailImage(w, h, () => false, IntPtr.Zero);

            //return scaledImage;
        }

        private string SaveOriginalFile(IFormFile file)
        {
            var filePath = Path.GetTempFileName();

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return filePath;
        }

        private ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        public async Task<string> QrInfo(IFormFile file)
        {
            var tempFile = await file.SaveToTempFile();

            var reader = new BarcodeReader() {
                TryInverted = true,
                AutoRotate = true,
                Options = new DecodingOptions
                {
                    TryHarder = true,
                }
            };
            reader.Options.PossibleFormats = new List<BarcodeFormat>() { BarcodeFormat.QR_CODE };

            var img = Image.FromFile(tempFile);

            var result = reader.Decode(new Bitmap(img));

            return result?.Text ?? "";
        }
    }
}
