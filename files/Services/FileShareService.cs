using api.files.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace api.files.Services;

public class FileShareService : IFileShareService
{
    private readonly IConfiguration _config;

    public FileShareService(IConfiguration config)
    {
        _config = config;
    }

    public void FileUpload(string subPath, string fileName, Stream streamFile)
    {
        // Get the configurations and create share object
        var dir = Path.Combine(_config.GetValue<string>("CONTAINER_DIR"), subPath);
        
        Directory.CreateDirectory(dir);
        if (streamFile.Length > 0)
        {
            string filePath = Path.Combine(dir, fileName);
            using Stream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            streamFile.CopyTo(fileStream);
        }
    }
}
