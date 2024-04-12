using api.core.Data.Enums;
using api.core.Data.requests;
using api.core.Data.Responses;
using System.Collections.Generic;
using System;

namespace api.core.services.abstractions;

public interface IImageService
{
    /// <summary>
    /// A function to handle the image resizing and storing in the fileshare service
    /// </summary>
    /// <param name="directory">the directory to store the file if valid</param>
    /// <param name="imageFile">The form file definition of the file to store</param>
    /// <param name="name">If null, will take the formfile image</param>
    /// <param name="type">the type of the file to handle multiple configuration behavior</param>
    public void EnsureImageSizeAndStore(string directory, IFormFile imageFile, ImageType type, string? name);
}
