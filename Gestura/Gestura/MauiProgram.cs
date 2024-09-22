using Gestura.Interfaces;
using Gestura.Repositories;
using Gestura.Services;
using Gestura.ViewModels;
using Microsoft.Extensions.Logging;

namespace Gestura
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Singletons
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "Gestura.db3");
            var databaseService = new DatabaseService(dbPath);
            databaseService.InitializeAsync().ConfigureAwait(false);
            builder.Services.AddSingleton(databaseService);

            builder.Services.AddSingleton<IImageRepository, ImageRepository>();
            builder.Services.AddSingleton<IDrawingSessionRepository, DrawingSessionRepository>();
            builder.Services.AddSingleton<IDrawingSessionImageReferenceRepository, DrawingSessionImageReferenceRepository>();

            builder.Services.AddSingleton<IImageService, ImageService>();
            builder.Services.AddSingleton<IDrawingSessionService, DrawingSessionService>();
            builder.Services.AddSingleton<INotificationService, NotificationService>();

            // ViewModels
            builder.Services.AddTransient<IAddOrUpdateSessionViewModel, AddOrUpdateSessionViewModel>();
            builder.Services.AddTransient<IDrawingSessionViewModel, DrawingSessionViewModel>();
            builder.Services.AddTransient<IImageSelectionViewModel, ImageSelectionViewModel>();
            builder.Services.AddTransient<DrawingSessionManagerViewModel>();
            builder.Services.AddTransient<ImageGalleryViewModel>();
            builder.Services.AddTransient<HomeViewModel>();

            var app = builder.Build();
            _serviceProvider = app.Services;

            return app;
        }

        private static IServiceProvider _serviceProvider;
        public static IServiceProvider Services => _serviceProvider;
    }
}
