
namespace KlioMobile.Services;

public class KlioService : RestServiceBase, IApiService
{
    public KlioService(IConnectivity connectivity, IBarrel cacheBarrel) : base(connectivity, cacheBarrel)
    {
        SetBaseURL(Constants.ApiServiceURL);
    }

    public async Task<VideoSearchResult> SearchVideos(string searchQuery, int nextPageNumber = 1)
    {
        var resourceUri = $"search?maxResults=10&type=video&q={WebUtility.UrlEncode(searchQuery)}&pageToken={nextPageNumber}";

        var result = await GetAsync<VideoSearchResult>(resourceUri, 4);

        return result;
    }
}
