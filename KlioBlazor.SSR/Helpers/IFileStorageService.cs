
namespace KlioBlazor.SSR.Helpers
{
    public interface IFileStorageService
    {
        Task DeleteFile(string fileRoute, string containerName);
        Task<string> EditFile(byte[] content, string extension, string containerName, string fileRoute);
        Task<string> SaveFile(byte[] content, string filename, string containerName);
    }
}
