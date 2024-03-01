using Microsoft.AspNetCore.Http;

namespace api.files.Services.Abstractions;

public interface IFileShareService
{
    void FileUpload(string subPath, IFormFile file);
}
