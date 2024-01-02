namespace KlioBlazor.Helpers
{
    public interface IFileStorageService
    {
        Task<string> SaveFile(byte[] content, string filename, string containerName);
        Task DeleteFile(string fileRoute, string containerName);
        Task<string> EditFile(byte[] content, string fileName, string containerName, string fileRoute);
    }
}
