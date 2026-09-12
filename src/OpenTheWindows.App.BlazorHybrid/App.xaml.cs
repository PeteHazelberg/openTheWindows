namespace OpenTheWindows.App.BlazorHybrid;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new MainPage()) { Title = "OpenTheWindows.App.BlazorHybrid" };
	}
}
