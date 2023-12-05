using Bot.Application.Interfaces.FileServiceInterfaces;
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
                string currentFolderPath = Directory.GetCurrentDirectory();
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(currentFolderPath, "..", "Bot.Application", "Files", "Images", fileName);
                string fullPath = Path.GetFullPath(filePath);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return fullPath;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(message: "Error in saving file process: {ex.Message}", ex.Message);
                return null;
            }
        }

        public FileStream? Get(string fullFilePath)
        {
            try
            {
                using var stream = File.OpenRead(fullFilePath);
                return stream;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(message: "Error getting file process: {ex.Message}", ex.Message);
                return null;
            }
        }

    }
}
