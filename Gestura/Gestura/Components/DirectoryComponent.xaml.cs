using Gestura.ViewModels;

namespace Gestura.Components;

public partial class DirectoryComponent : ContentView
{
	public static readonly BindableProperty DirectoryNameProperty = BindableProperty.Create(nameof(DirectoryName), typeof(string), typeof(DirectoryComponent), string.Empty);

	public string DirectoryName
	{
		get => (string)GetValue(DirectoryNameProperty);
		set => SetValue(DirectoryNameProperty, value);
	}

	public DirectoryComponent()
	{
		InitializeComponent();
	}
}