using api.core.Data.requests;
using api.core.Data.Responses;

namespace api.core.Services.Abstractions;

public interface ISubscriptionService
{
    public void Subscribe(SubscribeRequestDTO subscribeRequest);

    public void Unsubscribe(UnsubscribeRequestDTO unsubscribeRequest);
}