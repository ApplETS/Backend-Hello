namespace api.core.Services.Abstractions;

public interface INotificationService
{
    public void BulkAddNotificationForPublication(Guid publicationId);
    public Task<int> SendNewsForRemainingPublication();
}
