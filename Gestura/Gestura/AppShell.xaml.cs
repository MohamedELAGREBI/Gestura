using Gestura.Views;

namespace Gestura
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ImageGalleryPage), typeof(ImageGalleryPage));
            Routing.RegisterRoute(nameof(DrawingSessionsManagerPage), typeof(DrawingSessionsManagerPage));
        }
    }
}
