using api.core.Data;
using api.core.Data.Responses;
using api.core.services.abstractions;

using Microsoft.AspNetCore.Mvc;

namespace api.core.controllers;

[ApiController]
[Route("api/tags")]
public class TagsController(ITagService tagService) : ControllerBase
{
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
