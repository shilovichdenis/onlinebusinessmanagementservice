using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel;

namespace OnlineBusinessManagementService.Services
{
    public class ImageService : IImageService
    {
        public string? AddImage(string directory, IFormFile image)
        {
            string directoryPath = Path.Combine("wwwroot", "images", directory);
            string path = Path.Combine("images", directory);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (image != null)
            {
                string fileName = Path.GetFileName(image.FileName.Replace('"', ' ').Replace(" ", "").Replace("(", "").Replace(")", "").Replace(@"\", "").Replace("/", "").Replace("_", "").Replace("-", ""));
                using (FileStream stream = new FileStream(fileName, FileMode.Create))
                {
                    image.CopyTo(stream);
                    return Path.Combine(path, image.FileName);
                }
            }
            else
            {
                return null;
            }
        }
        public string? AddLogo(string directory, IFormFile logo)
        {
            string directoryPath = Path.Combine("wwwroot", "images", directory);
            string path = Path.Combine("images", directory);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (logo != null)
            {
                string fileName = Path.GetFileName(logo.FileName.Replace('"', ' ').Replace(" ", "").Replace("(", "").Replace(")", "").Replace(@"\", "").Replace("/", "").Replace("_", "").Replace("-", ""));
                using (FileStream stream = new FileStream(fileName, FileMode.Create))
                {
                    logo.CopyTo(stream);
                    return Path.Combine(path, logo.FileName);
                }
            }
            else
            {
                return null;
            }
        }

        public string? AddImages(string directory, List<IFormFile> images)
        {
            string directoryPath = Path.Combine("wwwroot", "images", directory);
            string path = Path.Combine("images", directory);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (images != null)
            {
                foreach (IFormFile image in images)
                {
                    string fileName = Path.GetFileName(image.FileName.Replace('"', ' ').Replace(" ", "").Replace("(", "").Replace(")", "").Replace(@"\", "").Replace("/", "").Replace("_", "").Replace("-", ""));
                    using (FileStream stream = new FileStream(Path.Combine(directoryPath, fileName), FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }
                }
                return path;
            }
            else
            {
                return null;
            }
        }

        public bool DeleteImage(string directory)
        {
            if (directory.Contains("defaultservice.png"))
            {
                return true;
            }

            if (directory == null)
            {
                throw new ArgumentNullException();
            }

            FileInfo image = new FileInfo(Path.Combine("wwwroot", directory));

            if (image.Exists)
            {
                image.Delete();
                return true;
            }
            else
            {
                return false;
            }
        }

        public string? CreateDirectory(string directory)
        {
            if (directory == null)
            {
                throw new ArgumentNullException("Directory is NULL");
            }

            var directoryPath = Path.Combine("wwwroot", "images", directory);
            string path = Path.Combine("images", directory);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                return path;
            }
            else
            {
                return null;
            }
        }

        public bool DeleteDirectory(string directory)
        {
            if (directory == null)
            {
                throw new ArgumentNullException();
            }

            var directoryPath = Path.Combine("wwwroot", directory);

            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<string>? GetImagesByPath(string directory)
        {
            var directoryPath = Path.Combine("wwwroot", directory);
            var files = new List<string> ();
            if (Directory.Exists(directoryPath))
            {
                files = Directory.GetFiles(directoryPath).ToList();
                for (int i = 0; i < files.Count; i++)
                {
                    files[i] = files[i].Remove(0, 8).Replace(@"\", "/");
                }
                return files;
            }
            else
            {
                return null;
            }
        }

        public bool AddImageTo(string directory, IFormFile image)
        {
            string directoryPath = Path.Combine("wwwroot", directory);
            if (!Directory.Exists(directoryPath))
            {
                throw new ArgumentNullException();
            }

            if (image != null)
            {
                string fileName = Path.GetFileName(image.FileName.Replace('"', ' ').Replace(" ", "").Replace("(", "").Replace(")", "").Replace(@"\", "").Replace("/", "").Replace("_", "").Replace("-", ""));
                string imagePath = Path.Combine(directoryPath, fileName);
                using (FileStream stream = new FileStream(imagePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddImagesTo(string directory, List<IFormFile> images)
        {
            string directoryPath = Path.Combine("wwwroot", directory);

            if (!Directory.Exists(directoryPath))
            {
                throw new ArgumentNullException();
            }

            if (images.Count != 0)
            {
                foreach (IFormFile image in images)
                {
                    string fileName = Path.GetFileName(image.FileName.Replace('"', ' ').Replace(" ", "").Replace("(", "").Replace(")", "").Replace(@"\", "").Replace("/", "").Replace("_", "").Replace("-", ""));
                    using (FileStream stream = new FileStream(Path.Combine(directoryPath, fileName), FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
