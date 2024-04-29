namespace KlioMobile;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        //Enable Version Tracking
        VersionTracking.Track();

        MainPage = new AppShell();
    }
}
