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
            // Obtenir le chemin de stockage des images depuis la configuration
            _imagesPath = configuration["ImagesStorage:Path"] ?? "wwwroot/images/cocktails";

            // Vérifier que le répertoire existe, sinon le créer
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

            // Vérifier le type de fichier
            string extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(extension) || !IsImageFile(extension))
            {
                throw new ArgumentException("Le fichier n'est pas une image valide.");
            }

            // Générer un nom de fichier unique pour éviter les collisions
            string uniqueFileName = $"{Guid.NewGuid()}{extension}";
            string filePath = Path.Combine(_imagesPath, uniqueFileName);

            // Enregistrer le fichier
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            // Retourner le chemin relatif de l'image
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
                // Extraire le nom de fichier du chemin
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
                // Gérer les exceptions (journalisation possible ici)
            }

            return false;
        }

        private bool IsImageFile(string extension)
        {
            // Liste des extensions d'image autorisées
            return extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif";
        }
    }
}