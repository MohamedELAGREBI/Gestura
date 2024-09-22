namespace Gestura.Interfaces
{
    public interface INotificationService
    {
        Task ShowErrorAsync(string message);
        Task ShowWarningAsync(string message);
        Task ShowSuccessAsync(string message);
        Task ShowInfoAsync(string message);
    }
}
