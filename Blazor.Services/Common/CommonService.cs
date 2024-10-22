using Microsoft.Extensions.Hosting;
using CardManagement.Services.Interfaces;
using System.Text.RegularExpressions;
namespace CardManagement.Services.Common
{
    public class CommonService : ICommonService
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public CommonService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        #region Utilities
        private string GetFileType(string magicCheck)
        {
            //Set the contenttype based on File Extension
            switch (magicCheck)
            {
                case "FF-D8-FF-E1":
                    return ".jpg";
                case "FF-D8-FF-E0":
                    return ".jpg";
                case "2F-39-6A-2F":
                    return ".jpg";
                case "25-50-44-46":
                    return ".pdf";
                case "89-50-4E-47":
                    return ".png";
                case "D0-CF-11-E0-A1-B1-1A-E1":
                    return ".doc";
                case "50-4B-03-04":
                    return ".docx";
                default:
                    return string.Empty;
            }
        }
        #endregion

        public async Task<string> GetExtention(byte[] document)
        {
            string data_as_hex = BitConverter.ToString(document);
            string magicCheck = data_as_hex.Substring(0, 11);
            return await Task.FromResult<string>(GetFileType(magicCheck));
        }
        public async Task<string> UploadAttachmentAsync(string filename, byte[] bytesToWrite)
        {
            if (string.IsNullOrEmpty(filename) || bytesToWrite == null)
                return string.Empty;         
            var uploadFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "Uploads", DateTime.Now.ToString("ddMMyyy"));        
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }           
            var filePath = Path.Combine(uploadFolder, filename);
            await File.WriteAllBytesAsync(filePath, bytesToWrite);          
            var relativePath = $"/Uploads/{DateTime.Now:ddMMyyy}/{filename}";
            return relativePath;
        }
        public async Task<string> GetMimeTypeFromBase64(string base64String)
        {
            var match = Regex.Match(base64String, @"data:(?<type>.+?);base64,");
            if (match.Success)
            {
                return match.Groups["type"].Value;
            }

            throw new ArgumentException("String is not a valid base64 data URL.");
        }
        public async Task<string> GenerateFileNameFromMimeType(string mimeType)
        {
            var extension = mimeType switch
            {
                "image/jpeg" => ".jpg",
                "image/png" => ".png",
                "video/mp4" => ".mp4",
                // Add other cases as needed
                _ => throw new ArgumentException("Unsupported MIME type")
            };

            return $"file_{DateTime.UtcNow.Ticks}{extension}";
        }

        public async Task<string> AddFiles(string base64File)
        {
            if (string.IsNullOrEmpty(base64File))
                throw new Exception("Base64 string cannot be null or empty");

            // Extract the MIME type and convert the base64 string to a byte array
            var dataParts = base64File.Split(new[] { "base64," }, StringSplitOptions.None);
            if (dataParts.Length != 2)
                throw new Exception("Invalid base64 format");

            string mimeType = dataParts[0].Split(new[] { ":", ";" }, StringSplitOptions.None)[1];
            byte[] fileBytes = Convert.FromBase64String(dataParts[1]);

            string ext = GetExtensionFromMimeType(mimeType);
            string[] permittedExtensions = { ".jpg", ".jpeg", ".png", ".mp4" };
            if (!permittedExtensions.Contains(ext))
                throw new Exception("Invalid file format");

            var uploadFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "Uploads");
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadFolder, uniqueFileName);

            await File.WriteAllBytesAsync(filePath, fileBytes);

            var relativePath = $"/Uploads/{uniqueFileName}";
            return relativePath;
        }
       

        private string GetExtensionFromMimeType(string mimeType)
        {
            switch (mimeType.ToLower())
            {
                case "image/jpeg":
                    return ".jpeg";
                case "image/png":
                    return ".png";
                case "video/mp4":
                    return ".mp4";
                // Add more cases as needed
                default:
                    throw new Exception("Unsupported MIME type");
            }
        }

        public string GenerateCardNumber()
        {         
            return Guid.NewGuid().ToString("N").Substring(0, 16);
        }

        public string GenerateCVV()
        {
            var random = new Random();
            return random.Next(100, 999).ToString();
        }

        public string GeneratePin()
        {       
            var random = new Random();
            return random.Next(1000, 9999).ToString();
        }
    }
}
