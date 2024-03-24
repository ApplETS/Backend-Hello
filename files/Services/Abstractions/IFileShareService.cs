namespace api.files.Services.Abstractions;

public interface IFileShareService
{
    Uri FileGetDownloadUri(string fileName);

    void FileUpload(string subPath, string fileName, Stream streamFile);
}
