namespace KlioMobile.Views.Desktop;

public partial class DesktopStartPage : ContentPage, IStartPage
{
	public DesktopStartPage()
	{
		InitializeComponent();

        this.BindingContext = new StartPageViewModel(this.Navigation);
    }
}