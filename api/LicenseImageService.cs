
using System.Security.Policy;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace Motto.Api;

public interface IMinioService
{
    Task<string> UploadImageAsync(IFormFile image, string fileName);
    Task<MemoryStream> GetImageAsync(string fileName);
}

public class LicenseImageService : IMinioService
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName = "delivery-driver-images";

    public LicenseImageService(IConfiguration configuration)
    {
        var minioEndpoint = configuration["Minio:Endpoint"];
        var rootUser = configuration["Minio:RootUser"];
        var rootPassword = configuration["Minio:RootPassword"];

        _minioClient = new MinioClient()
        .WithEndpoint(new Uri(minioEndpoint))
        .WithCredentials(rootUser, rootPassword)
        .Build();
    }

    private async Task<bool> VerifyAndCreateBucket()
    {
        try
        {
            // Verifica se o bucket já existe
            var bucketExistsArgs = new BucketExistsArgs()
                .WithBucket(_bucketName);

            bool bucketExists = await _minioClient.BucketExistsAsync(bucketExistsArgs);

            if (!bucketExists)
            {
                // Se o bucket não existe, cria o novo bucket
                await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));

                Console.WriteLine($"Bucket '{_bucketName}' criado com sucesso.");
                return true;
            }
            else
            {
                Console.WriteLine($"O bucket '{_bucketName}' já existe.");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao verificar ou criar o bucket '{_bucketName}': {ex.Message}");
        }
    }

    public async Task<MemoryStream> GetImageAsync(string fileName)
    {

        try
        {
            StatObjectArgs statObjectArgs = new StatObjectArgs()
                                           .WithBucket(_bucketName)
                                           .WithObject(fileName);
            await _minioClient.StatObjectAsync(statObjectArgs);

            var memStream = new MemoryStream();
            GetObjectArgs getObjectArgs = new GetObjectArgs()
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
            // Trata o erro de MinioException
            Console.WriteLine($"Erro ao obter imagem '{fileName}' do MinIO: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            // Trata outros tipos de exceções
            Console.WriteLine($"Erro ao obter imagem '{fileName}': {ex.Message}");
            throw;
        }
    }

    public async Task<string> UploadImageAsync(IFormFile image, string fileName)
    {

        // remover daqui para o startup do docker =[
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
