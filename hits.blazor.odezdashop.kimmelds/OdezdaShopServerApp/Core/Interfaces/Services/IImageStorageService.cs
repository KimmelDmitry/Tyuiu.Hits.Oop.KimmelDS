namespace OdezdaShopServerApp.Core.Interfaces.Services
{
    public interface IImageStorageService
    {
        Task<string> SaveProductImageAsync(string fileName, byte[] bytes);
        Task DeleteAsync(string relativePath);
    }
}
