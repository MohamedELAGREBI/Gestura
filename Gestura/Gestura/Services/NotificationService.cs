using Gestura.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestura.Services
{
    public class NotificationService : INotificationService
    {
        public async Task ShowErrorAsync(string message)
        {
            // Utilisation d'une modale pour les erreurs graves
            await Shell.Current.DisplayAlert("Erreur", message, "OK");
        }

        public async Task ShowInfoAsync(string message)
        {
            // Barre de notification pour signaler des informations
            await ShowNotificationAsync(message, Colors.Blue);
        }

        public async Task ShowSuccessAsync(string message)
        {
            // Barre de notification pour signaler un succès
            await ShowNotificationAsync(message, Colors.Green);
        }

        public async Task ShowWarningAsync(string message)
        {
            // Barre de notification pour les erreurs mineures
            await ShowNotificationAsync(message, Colors.Orange);
        }

        private async Task ShowNotificationAsync(string message, Color backgroundColor)
        {
            // Barre de notification temporaire
            var notificationLabel = new Label
            {
                Text = message,
                BackgroundColor = backgroundColor,
                TextColor = Colors.White,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(10)
            };

            // Ajout à la vue actuelle
            var currentPage = Shell.Current.CurrentPage as ContentPage;
            var stackLayout = currentPage?.Content as StackLayout;
            if (stackLayout != null)
            {
                stackLayout.Children.Insert(0, notificationLabel);

                // Attendre quelques secondes avant de retirer la notification
                await Task.Delay(3000);

                // Retrait de la notification
                stackLayout.Children.Remove(notificationLabel);
            }
        }
    }
}
