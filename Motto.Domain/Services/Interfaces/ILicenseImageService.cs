using Microsoft.AspNetCore.Http;

namespace Motto.Services.Interfaces;

public interface ILicenseImageService
{
    Task<MemoryStream> GetImageAsync(string fileName);
    Task<string> UploadImageAsync(IFormFile image, string fileName);
}