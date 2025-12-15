using OdezdaShopServerApp.Core.Interfaces.Services;

namespace OdezdaShopServerApp.Core.Services;

public class FileSystemImageStorageService : IImageStorageService
{
    private readonly string _webRootPath; //content root + "wwwroot"
    private readonly string _imagesFolder;


    public FileSystemImageStorageService(string env, string imagesFolder = "images/products")
    {
        _webRootPath = env;
        _imagesFolder = imagesFolder;
    }


    public async Task DeleteAsync(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath)) return;

        var full = MapToFullPath(relativePath);

        if (File.Exists(full)) 
            File.Delete(full);

        await Task.CompletedTask;
    }


    public async Task<string> SaveProductImageAsync(string originalFileName, byte[] bytes)
    {
        if (bytes == null || bytes.Length == 0)
            throw new ArgumentException("Empty file", nameof(bytes));

        var extension = Path.GetExtension(originalFileName);
        if (string.IsNullOrWhiteSpace(extension))
            throw new ArgumentException("File extension is required", nameof(originalFileName));

        // Генерируем безопасное имя
        var fileName = $"{Guid.NewGuid():N}{extension.ToLowerInvariant()}";

        // Относительный путь (ТОЛЬКО URL, без дисков)
        const string relativeFolder = "images/products";
        var relativePath = $"/{relativeFolder}/{fileName}";

        // Абсолютный путь в wwwroot
        var absoluteFolder = Path.Combine(_webRootPath, "images", "products");
        var absolutePath = Path.Combine(absoluteFolder, fileName);

        if (!Directory.Exists(absoluteFolder))
            Directory.CreateDirectory(absoluteFolder);

        await File.WriteAllBytesAsync(absolutePath, bytes);

        return relativePath;
    }



    private string MapToFullPath(string relativePath)
    {
        var cleaned = relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);

        return Path.Combine(_webRootPath, cleaned);
    }
}
