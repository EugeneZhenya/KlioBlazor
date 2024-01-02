
namespace KlioBlazor.Helpers
{
    public interface IHttpService
    {
        Task<HttpResponseWrapper<object>> Post<T>(string url, T data);
    }
}
