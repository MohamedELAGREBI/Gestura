using Gestura.Interfaces;
using Gestura.Models;
using Gestura.ViewModels;

namespace Gestura.Views;

public partial class DrawingSessionPage : ContentPage
{
    private readonly DrawingSessionViewModel _viewModel;

    public DrawingSessionPage(IDrawingSessionService drawingSessionService, DrawingSession session)
    {
        InitializeComponent();
        BindingContext = new DrawingSessionViewModel(drawingSessionService, session);
        _viewModel = (DrawingSessionViewModel)BindingContext;

        StartBlinkingTimer();
    }

    private async void StartBlinkingTimer()
    {
        while (true)
        {
            if (_viewModel.RemainingTime.TotalSeconds <= 5)
            {
                await TimerLabel.FadeTo(0, 250);
                await TimerLabel.FadeTo(1, 250);
                TimerLabel.TextColor = TimerLabel.TextColor == Colors.White ? Colors.Red : Colors.White;
            }
            else
            {
                TimerLabel.TextColor = Colors.White;
            }

            await Task.Delay(500);
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await FadeInImage();
    }

    private async Task FadeInImage()
    {
        var image = (Image)FindByName("CurrentImage");
        image.Opacity = 0;
        await image.FadeTo(1, 1000);
    }

    private async void OnNextImage(object sender, EventArgs e)
    {
        if (_viewModel.CurrentImageIndex <= _viewModel.Images.Count - 1)
        {
            await SlideToNextImage();
        }
    }

    private async Task SlideToNextImage()
    {
        var image = (Image)FindByName("CurrentImage");
        await image.TranslateTo(-Width, 0, 500);
        image.TranslationX = Width;
        await image.TranslateTo(0, 0, 500);
    }

    private async void OnPreviousImage(object sender, EventArgs e)
    {
        if (_viewModel.CurrentImageIndex > 0)
        {
            await SlideToPreviousImage();
        }
    }

    private async Task SlideToPreviousImage()
    {
        var image = (Image)FindByName("CurrentImage");
        await image.TranslateTo(Width, 0, 500);
        image.TranslationX = -Width;
        await image.TranslateTo(0, 0, 500);
    }
}