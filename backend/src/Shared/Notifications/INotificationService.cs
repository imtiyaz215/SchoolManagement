namespace Shared.Notifications;

public record NotificationMessage(
    string Title,
    string Body,
    IReadOnlyList<string> Recipients,
    IDictionary<string, string>? Data = null);

public interface INotificationService
{
    Task SendAsync(NotificationMessage message, CancellationToken ct = default);
}
