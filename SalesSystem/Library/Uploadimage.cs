using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Library
{
    public class Uploadimage
    {
        public async Task<byte[]> ByteAvatarImageAsync(IFormFile AvatarImage, IWebHostEnvironment environment, string image)
        {
            if (AvatarImage != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await AvatarImage.CopyToAsync(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            else
            {
                var archivoOrigen = $"{environment.ContentRootPath}/wwwroot/{image}";
                return File.ReadAllBytes(archivoOrigen);
            }
        }
    }
}
