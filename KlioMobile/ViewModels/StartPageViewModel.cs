namespace KlioMobile.ViewModels;

public partial class StartPageViewModel : AppViewModelBase
{
    private int nextPageNum = 1;
    private string searchTerm = "";

    [ObservableProperty]
    private ObservableCollection<Movie> klioVideos;

    public StartPageViewModel(IApiService appApiService) : base(appApiService)
    {
        this.Title = "Медійний архів 'KLIO'";
    }

    public override async void OnNavigatedTo(object parameters)
    {
        Search();
    }

    private async Task Search()
    {
        SetDataLodingIndicators(true);

        LoadingText = "Пошук інформації...";

        KlioVideos = new();

        try
        {
            //Search for videos
            await GetKlioVideos();

            DataLoaded = true;
        }
        catch (InternetConnectionException iex)
        {
            this.IsErrorState = true;
            this.ErrorMessage = "Надто повільне або відсутнє з'єднання з Інтернетои." + Environment.NewLine + "Перевірте своє інтернет-з'єднання та спробуйте знову.";
            ErrorImage = "nointernet.png";
        }
        catch (Exception ex)
        {
            this.IsErrorState = true;
            this.ErrorMessage = "Наразі сервіси медійного архіву не працють." + Environment.NewLine + $"Повідомте про проблему на нашу адресу: {Constants.EmailAddress}." + Environment.NewLine + Environment.NewLine + "Деталі: " + ex.Message;
            ErrorImage = "error.png";
        }
        finally
        {
            SetDataLodingIndicators(false);
        }
    }

    private async Task GetKlioVideos()
    {
        //Search the videos
        var videoSearchResult = await _appApiService.SearchVideos(searchTerm, nextPageNum);

        nextPageNum = videoSearchResult.NextPageNumber;

        //Add the Videos for Display
        KlioVideos.AddRange(videoSearchResult.Items);
    }

    [RelayCommand]
    private async void OpenSettingPage()
    {
        await PageService.DisplayAlert("Налаштування", "Налаштовувати можна буде колись потім.", "Зрозуміло!");
    }
}
