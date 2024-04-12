using api.core.data.entities;
using api.core.Data.Enums;
using api.core.Data.Exceptions;
using api.core.services.abstractions;
using api.files.Services.Abstractions;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace api.core.Services;

public class ImageService(IFileShareService fileShareService) : IImageService
{
    private const double TOLERANCE_ACCEPTABILITY = 0.01;

    /// <summary>
    /// width/height ratio acceptance size for different image types
    /// </summary>
    private readonly Dictionary<ImageType, double> _imageRatioAcceptance = new()
    {
        { ImageType.Publication, 2.0 },
        { ImageType.Avatar, 1.0 }
    };

    /// <summary>
    /// width/height ratio acceptance size for different image types
    /// </summary>
    private readonly Dictionary<ImageType, Size> _imageRatioAcceptanceSize = new()
    {
        { ImageType.Publication, new Size(400, 200) },
        { ImageType.Avatar, new Size(100, 100) }
    };

    /// <summary>
    /// A function to handle the image resizing and storing in the fileshare service
    /// </summary>
    /// <param name="directory">the directory to store the file if valid</param>
    /// <param name="imageFile">The form file definition of the file to store</param>
    /// <param name="type">the type of the file to handle multiple configuration behavior</param>
    /// <param name="name">If null, will take the formfile image</param>
    /// <exception cref="BadParameterException{Event}"></exception>
    public void EnsureImageSizeAndStore(string directory, IFormFile imageFile, ImageType type, string? name)
    {
        byte[] imageBytes = [];
        try
        {
            using var image = Image.Load(imageFile.OpenReadStream());
            int width = image.Size.Width;
            int height = image.Size.Height;

            if (Math.Abs((width / height) - _imageRatioAcceptance[type]) > TOLERANCE_ACCEPTABILITY)
                throw new BadParameterException<Event>(nameof(image), $"Invalid image aspect ratio {width}/{height}");

            image.Mutate(c => c.Resize(_imageRatioAcceptanceSize[type]));
            using var outputStream = new MemoryStream();
            image.SaveAsWebp(outputStream);
            outputStream.Position = 0;

            fileShareService.FileUpload(directory, imageFile.FileName, outputStream);
        }
        catch (Exception e)
        {
            throw new BadParameterException<Event>(nameof(imageFile), $"Invalid image metadata: {e.Message}");
        }
    }
}
