namespace OnlineBusinessManagementService.Services
{
    public interface IImageService
    {
        List<string>? GetImagesByPath(string directory);
        bool DeleteImage(string directory);
        string? CreateDirectory(string directory);
        bool DeleteDirectory(string directory);
        string? AddLogo(string directory, IFormFile logo);
        string? AddImage(string directory, IFormFile image);
        bool AddImageTo(string directory, IFormFile image);
        string? AddImages(string directory, List<IFormFile> images);
        bool AddImagesTo(string directory, List<IFormFile> images);
    }
}
