namespace ProductCatalogService.Services;

public interface IStorageService
{
    Task<string> UploadImage(IFormFile image, string folder);
    Task<string> UploadImage(IFormFile image, Guid entityId, string folder);
    Task<List<string>> UploadImages(IFormFileCollection images, string folder);
    Task DeleteImage(string imageId);
    Task DeleteImages(IEnumerable<string> imageIds);
}
