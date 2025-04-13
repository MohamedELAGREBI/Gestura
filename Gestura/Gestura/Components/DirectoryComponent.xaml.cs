using Gestura.Interfaces;
using Gestura.Models;
using Gestura.ViewModels;

namespace Gestura.Components;

public partial class DirectoryComponent : ContentView
{
    private static IImageDirectoryService _imageDirectoryService;

    public static readonly BindableProperty DirectoryProperty =
        BindableProperty.Create(nameof(Directory), typeof(ImageDirectory), typeof(DirectoryComponent), propertyChanged: OnDirectoryChanged);

    public ImageDirectory Directory
    {
        get => (ImageDirectory)GetValue(DirectoryProperty);
        set => SetValue(DirectoryProperty, value);
    }

    public static readonly BindableProperty AnimatedExpandProperty =
        BindableProperty.Create(nameof(AnimatedExpand), typeof(Action<bool>), typeof(DirectoryComponent));

    public Action<bool> AnimatedExpand
    {
        get => (Action<bool>)GetValue(AnimatedExpandProperty);
        set => SetValue(AnimatedExpandProperty, value);
    }

    public DirectoryComponent()
    {
        InitializeComponent();
        SetImageDirectoryService();
    }

    private static void SetImageDirectoryService()
    {
        _imageDirectoryService = MauiProgram.Services.GetService<IImageDirectoryService>();
    }

    private static void OnDirectoryChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (DirectoryComponent)bindable;
        if (newValue is ImageDirectory dir)
        {
            view.BindingContext = new DirectoryComponentViewModel(_imageDirectoryService, dir)
            {
                AnimatedExpand = view.AnimatedExpand
            };
        }
    }

    private async void AnimateExpansion(bool isExpanding)
    {
        if (isExpanding)
        {
            await this.FadeTo(0, 100);
            await this.FadeTo(1, 300);
        }
        else
        {
            await this.ScaleTo(0.95, 100);
            await this.ScaleTo(1, 100);
        }
    }
}