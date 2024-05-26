using Minio.DataModel.Args;
using Minio.Exceptions;
using Minio;
using Motto.Repositories.Interfaces;
using Motto.Services.Interfaces;

namespace Motto.Services;

public class LicenseImageService : ILicenseImageService
{
    private readonly IMinioClient _minioClient;
    private readonly IDeliveryDriverRepository _deliveryDriverRepository;
    private readonly string _bucketName = "delivery-driver-images";

    public LicenseImageService(IConfiguration configuration, IDeliveryDriverRepository deliveryDriverRepository)
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
