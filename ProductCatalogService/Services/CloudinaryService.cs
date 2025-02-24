using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace ProductCatalogService.Services;

public class CloudinaryService(ICloudinary cloudinary) : IStorageService
{
    public async Task<string> UploadImage(IFormFile image, string folder)
    {
        var uploadImageParams = new ImageUploadParams
        {
            File = new FileDescription(image.FileName, image.OpenReadStream()),
            Folder = folder
        };

        var uploadResult = await cloudinary.UploadAsync(uploadImageParams);
        return uploadResult.Url.ToString();
    }

    public async Task<string> UploadImage(IFormFile image, Guid entityId, string folder)
    {
        var uploadImageParams = new ImageUploadParams
        {
            File = new FileDescription(image.FileName, image.OpenReadStream()),
            Folder = folder,
            PublicId = entityId.ToString(),
        };

        var uploadResult = await cloudinary.UploadAsync(uploadImageParams);
        return uploadResult.Url.ToString();
    }

    public async Task<List<string>> UploadImages(IFormFileCollection images, string folder)
    {
        var urls = new List<string>();

        await Parallel.ForEachAsync(images, async (image, _) =>
        {
            var url = await UploadImage(image, folder);
            lock (urls)
            {
                urls.Add(url);
            }
        });

        return urls;
    }

    public async Task DeleteImage(string imageId)
    {
        await cloudinary.DestroyAsync(new DeletionParams(imageId));
    }

    public async Task DeleteImages(IEnumerable<string> imageIds)
    {
        await Parallel.ForEachAsync(imageIds, async (publicId, _) =>
        {
            await DeleteImage(publicId);
        });

    }
}
