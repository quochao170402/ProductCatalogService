

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using ProductCatalogService.Extensions.Options;

namespace ProductCatalogService.Services.AWSServices;

public interface IAWSS3Service
{
    Task<string> UploadFileAsync(string filePath, string keyName);
    Task<byte[]> DownloadFileAsync(string keyName);
}

public class AWSS3Service(IConfiguration config, IAmazonS3 s3Client) : IAWSS3Service
{
    private readonly string _bucketName = config["AWS:BucketName"]!;


    public async Task<string> UploadFileAsync(string filePath, string keyName)
    {
        var putRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = keyName,
            FilePath = filePath,
            ContentType = "application/octet-stream"
        };

        var response = await s3Client.PutObjectAsync(putRequest);
        return response.HttpStatusCode.ToString();
    }

    public async Task<byte[]> DownloadFileAsync(string keyName)
    {
        var request = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = keyName
        };

        using var response = await s3Client.GetObjectAsync(request);
        using var stream = new MemoryStream();
        await response.ResponseStream.CopyToAsync(stream);
        return stream.ToArray();
    }
}
