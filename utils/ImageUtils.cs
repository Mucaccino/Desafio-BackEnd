using Microsoft.AspNetCore.StaticFiles;

namespace Motto.Utils;

public static class ImageUtils
{
    public static string GetMimeType(string fileName)
    {
        const string DefaultContentType = "application/octet-stream";

        var provider = new FileExtensionContentTypeProvider();

        if (!provider.TryGetContentType(fileName, out string contentType))
        {
            contentType = DefaultContentType;
        }

        return contentType;
    }

}