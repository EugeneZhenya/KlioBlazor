namespace KlioMobile.ViewModels;

public partial class StartPageViewModel
{
    private INavigation _navigationService;

    public StartPageViewModel(INavigation navigation)
    {
        this._navigationService = navigation;
    }

    [RelayCommand]
    private async Task NavigateToSecondPage()
    {
        await _navigationService.PushAsync(ViewServices.ResolvePage<ISecondPage>("Test Data"));
    }
}
