using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CocktailApp.Services
{
    public class ImageService
    {
        private readonly string _imagesPath;

        public ImageService(IConfiguration configuration)
        {
           
            _imagesPath = configuration["ImagesStorage:Path"] ?? "wwwroot/images/cocktails";

            
            if (!Directory.Exists(_imagesPath))
            {
                Directory.CreateDirectory(_imagesPath);
            }
        }

        public async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return null;
            }

            
            string extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(extension) || !IsImageFile(extension))
            {
                throw new ArgumentException("Le fichier n'est pas une image valide.");
            }

            
            string uniqueFileName = $"{Guid.NewGuid()}{extension}";
            string filePath = Path.Combine(_imagesPath, uniqueFileName);

           
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return $"/images/cocktails/{uniqueFileName}";
        }

        public bool DeleteImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                return false;
            }

            try
            {
                
                string fileName = Path.GetFileName(imagePath);
                string fullPath = Path.Combine(_imagesPath, fileName);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
            }
            catch (Exception)
            {
               
            }

            return false;
        }

        private bool IsImageFile(string extension)
        {
            
            return extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif";
        }
    }
}