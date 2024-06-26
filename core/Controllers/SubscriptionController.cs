using api.core.Data;
using api.core.Data.requests;
using api.core.Services.Abstractions;

using Microsoft.AspNetCore.Mvc;

namespace api.core.Controllers;

/// <summary>
/// Manage subscription to organizer posts. This controller is part of the public API
/// allowing users to subscribe and unsubscribe from organizer posts using an email adress.
/// </summary>
/// <param name="subscriptionService">Used to manage the subscriptions</param>
[ApiController]
[Route("api/subscriptions")]
public class SubscriptionController(ISubscriptionService subscriptionService) : ControllerBase
{
    /// <summary>
    /// Subscribe to an organizer posts by providing an email address and an organizer id.
    /// </summary>
    /// <param name="request">The object containing both parameter to subscribe</param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult Subscribe([FromBody] SubscribeRequestDTO request)
    {
        subscriptionService.Subscribe(request);
        return Ok(new Response<object>
        {
            Data = "Subscribed successfully to this organizer posts."
        });
    }

    /// <summary>
    /// Unsubscribe from an organizer posts by providing the unsubscription token in the email received.
    /// </summary>
    /// <param name="request">the subscription token</param>
    /// <returns></returns>
    [HttpDelete]
    public IActionResult Unsubcribe([FromBody] UnsubscribeRequestDTO request)
    {
        subscriptionService.Unsubscribe(request);
        return Ok(new Response<object>
        {
            Data = "Unsubscribed successfully from this organizer posts."
        });
    }
}
