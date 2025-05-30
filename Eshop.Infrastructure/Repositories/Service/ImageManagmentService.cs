using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eshop.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace Eshop.Infrastructure.Repositories.Service
{
    public class ImageManagmentService : IImageManagmentService
    {
        //IFileProvider >>  tovreading files path
        private readonly IFileProvider _fileProvider;
        public ImageManagmentService(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }
        public async Task<List<string>> AddImageAsync(IFormFileCollection files, string src)
        {
            var SaveImageSrc = new List<string>();
            var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            //  Ensure wwwroot exists
            if (Directory.Exists(wwwRootPath) is not true)
            {
                Directory.CreateDirectory(wwwRootPath);
            }

            var ImageDirectory = Path.Combine("wwwroot", "Images", src);

            //  Ensure wwwroot/Images/src exists
            if (Directory.Exists(ImageDirectory) is not true)
            {
                Directory.CreateDirectory(ImageDirectory);
            }
            //loop
            foreach (var item in files)
            {
                if(item.Length > 0)
                {
                    //get image name
                    var ImageName = item.FileName;
                    var ImageSrc = $"Images/{src}/{ImageName}";
                    var root = Path.Combine(ImageDirectory, ImageName);
                    using (FileStream stream = new FileStream(root,FileMode.Create))
                    {
                        await item.CopyToAsync(stream);

                    }
                    SaveImageSrc.Add(ImageSrc);
                }
            }

            return SaveImageSrc;
        }

        public void DeleteImageAsync(string src)
        {
            var info = _fileProvider.GetFileInfo(src);
            var root = info.PhysicalPath;
            File.Delete(root);
        }
    }
}
