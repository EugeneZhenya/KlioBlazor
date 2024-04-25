namespace KlioMobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(ViewServices.ResolvePage<IStartPage>());
        }
    }
}
