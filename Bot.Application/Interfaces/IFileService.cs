using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Application.Interfaces
{
    public interface IFileService
    {
        Task<string?> Save(IFormFile file);
        Stream? Get(string fileName);
        
    }
}
