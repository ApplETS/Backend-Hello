using api.core.Data;
using api.core.Data.Responses;
using api.core.services.abstractions;

using Microsoft.AspNetCore.Mvc;

namespace api.core.controllers;

/// <summary>
/// A controller to handle all the tags related operations. This controller is also part of the public API.
/// 
/// Under the hood, this controller uses the ITagService to manage the data.
/// </summary>
/// <param name="tagService">Used to fetch and manage the tags</param>
[ApiController]
[Route("api/tags")]
public class TagsController(ITagService tagService) : ControllerBase
{
    /// <summary>
    /// Get all tags that are available to associate with an event.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult GetTags()
    {
        var tags = tagService.GetTags();
        return Ok(new Response<IEnumerable<TagResponseDTO>>
        {
            Data = tags,
        });
    }
}
