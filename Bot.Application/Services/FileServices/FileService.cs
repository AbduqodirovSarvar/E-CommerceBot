using Bot.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Application.Services.FileServices
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;
        public FileService(ILogger<FileService> logger)
        {
            _logger = logger;
        }
        public async Task<string?> Save(IFormFile file)
        {
            try
            {
                string folderPath = Directory.GetCurrentDirectory();
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(folderPath, "..", "Bot.Application", "Files", "Images", fileName);
                string fp = Path.GetFullPath(filePath);
                using (var stream = new FileStream(fp, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return fileName;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(message: "Error in saving file process: {ex.Message}", ex.Message);
                return null;
            }
        }

        public Stream? Get(string fileName)
        {
            try
            {
                using var stream = File.OpenRead(fileName);
                return stream;
            }
            catch
            {
                return null;
            }
        }
    }
}
