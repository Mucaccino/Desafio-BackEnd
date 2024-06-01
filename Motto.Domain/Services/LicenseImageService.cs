using Minio.DataModel.Args;
using Minio.Exceptions;
using Minio;
using Motto.Domain.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Motto.Data.Repositories.Interfaces;

namespace Motto.Domain.Services;

/// <summary>
/// Represents a service for handling license image operations.
/// </summary>
public class LicenseImageService : ILicenseImageService
{
    private readonly IMinioClient _minioClient;
    private readonly IDeliveryDriverUserRepository _deliveryDriverRepository;
    private readonly string _bucketName = "delivery-driver-images";

    /// <summary>
    /// Initializes a new instance of the <see cref="LicenseImageService"/> class.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <param name="deliveryDriverRepository">The delivery driver repository.</param>
    public LicenseImageService(IConfiguration configuration, IDeliveryDriverUserRepository deliveryDriverRepository)
    {
        _deliveryDriverRepository = deliveryDriverRepository;

        var minioEndpoint = configuration["Minio:Endpoint"];
        var rootUser = configuration["Minio:RootUser"];
        var rootPassword = configuration["Minio:RootPassword"];

        _minioClient = new MinioClient()
            .WithEndpoint(new Uri(minioEndpoint ?? ""))
            .WithCredentials(rootUser, rootPassword)
            .Build();
    }

    /// <summary>
    /// Verifies if a bucket exists and creates it if it doesn't.
    /// </summary>
    /// <returns>A boolean indicating if the bucket was created or already exists.</returns>
    private async Task<bool> VerifyAndCreateBucket()
    {
        try
        {
            var bucketExistsArgs = new BucketExistsArgs()
                .WithBucket(_bucketName);

            bool bucketExists = await _minioClient.BucketExistsAsync(bucketExistsArgs);

            if (!bucketExists)
            {
                await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));
                Console.WriteLine($"Bucket '{_bucketName}' criado com sucesso.");
                return true;
            }
            else
            {
                Console.WriteLine($"O bucket '{_bucketName}' já existe.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao verificar ou criar o bucket '{_bucketName}': {ex.Message}");
        }

        return false;
    }

    /// <summary>
    /// Retrieves an image from the specified file asynchronously.
    /// </summary>
    /// <param name="fileName">The name of the file containing the image.</param>
    /// <returns>A MemoryStream containing the image data.</returns>
    /// <exception cref="MinioException">Thrown if there is an error retrieving the image from MinIO.</exception>
    /// <exception cref="Exception">Thrown if there is an error retrieving the image.</exception>
    public async Task<MemoryStream> GetImageAsync(string fileName)
    {
        try
        {
            var statObjectArgs = new StatObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName);

            await _minioClient.StatObjectAsync(statObjectArgs);

            var memStream = new MemoryStream();
            var getObjectArgs = new GetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName)
                .WithCallbackStream(async (stream) =>
                {
                    await stream.CopyToAsync(memStream);
                });

            await _minioClient.GetObjectAsync(getObjectArgs);

            return memStream;
        }
        catch (MinioException ex)
        {
            Console.WriteLine($"Erro ao obter imagem '{fileName}' do MinIO: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao obter imagem '{fileName}': {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Uploads an image asynchronously.
    /// </summary>
    /// <param name="image">The image file to upload.</param>
    /// <param name="fileName">The name of the file to be saved.</param>
    /// <returns>The name of the saved file.</returns>
    /// <exception cref="Exception">Thrown if there is an error during the upload process.</exception>
    public async Task<string> UploadImageAsync(IFormFile image, string fileName)
    {
        await VerifyAndCreateBucket();

        try
        {
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                var contentType = image.ContentType;

                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(fileName)
                    .WithObjectSize(image.Length)
                    .WithStreamData(memoryStream)
                    .WithContentType(contentType);

                await _minioClient.PutObjectAsync(putObjectArgs);

                return fileName;
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Ocorreu um erro ao fazer o upload da imagem: {ex.Message}");
        }
    }
}
