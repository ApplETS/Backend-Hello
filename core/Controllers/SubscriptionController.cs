using api.core.Data;
using api.core.Data.requests;
using api.core.Services.Abstractions;

using Microsoft.AspNetCore.Mvc;

namespace api.core.Controllers;

[ApiController]
[Route("api/subscriptions")]
public class SubscriptionController(ISubscriptionService subscriptionService) : ControllerBase
{
    [HttpPost]
    public IActionResult Subscribe([FromBody] SubscribeRequestDTO request)
    {
        subscriptionService.Subscribe(request);
        return Ok(new Response<object>
        {
            Data = "Subscribe successfully to this organizer posts."
        });
    }

    [HttpDelete]
    public IActionResult Unsubcribe([FromBody] UnsubscribeRequestDTO request)
    {
        subscriptionService.Unsubscribe(request);
        return Ok(new Response<object>
        {
            Data = "Unsubscribe successfully from this organizer posts."
        });
    }
}
