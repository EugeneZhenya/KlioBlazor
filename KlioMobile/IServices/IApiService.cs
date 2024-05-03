namespace KlioMobile.IServices;

public interface IApiService
{
    Task<VideoSearchResult> SearchVideos(string searchQuery, int nextPageNumber = 1);
}
