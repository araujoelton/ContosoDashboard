namespace ContosoDashboard.Services;

public interface IFileStorageService
{
    Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default);
    Task<Stream> DownloadAsync(string filePath, CancellationToken cancellationToken = default);
    Task DeleteAsync(string filePath, CancellationToken cancellationToken = default);
    Task<string> GetUrlAsync(string filePath, TimeSpan expiration);
    Task<bool> ExistsAsync(string filePath, CancellationToken cancellationToken = default);
}

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _basePath;
    private readonly string _basePathWithSeparator;

    public LocalFileStorageService(IConfiguration configuration, IHostEnvironment environment)
    {
        var configuredPath = configuration["DocumentStorage:BasePath"] ?? "AppData/uploads";
        _basePath = Path.GetFullPath(Path.Combine(environment.ContentRootPath, configuredPath));
        _basePathWithSeparator = _basePath.EndsWith(Path.DirectorySeparatorChar)
            ? _basePath
            : _basePath + Path.DirectorySeparatorChar;

        Directory.CreateDirectory(_basePath);
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default)
    {
        var extension = Path.GetExtension(fileName);
        var storedFileName = $"{Guid.NewGuid():N}{extension}";
        var relativePath = Path.Combine(
            DateTime.UtcNow.Year.ToString(),
            DateTime.UtcNow.Month.ToString("D2"),
            storedFileName);

        var fullPath = GetFullPath(relativePath);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

        await using var output = File.Create(fullPath);
        await fileStream.CopyToAsync(output, cancellationToken);

        return NormalizeRelativePath(relativePath);
    }

    public Task<Stream> DownloadAsync(string filePath, CancellationToken cancellationToken = default)
    {
        var fullPath = GetFullPath(filePath);

        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException("Arquivo de documento não encontrado.", filePath);
        }

        Stream stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return Task.FromResult(stream);
    }

    public Task DeleteAsync(string filePath, CancellationToken cancellationToken = default)
    {
        var fullPath = GetFullPath(filePath);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }

    public Task<string> GetUrlAsync(string filePath, TimeSpan expiration)
    {
        return Task.FromResult(filePath);
    }

    public Task<bool> ExistsAsync(string filePath, CancellationToken cancellationToken = default)
    {
        var fullPath = GetFullPath(filePath);
        return Task.FromResult(File.Exists(fullPath));
    }

    private string GetFullPath(string relativePath)
    {
        var normalizedPath = relativePath
            .Replace('/', Path.DirectorySeparatorChar)
            .TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

        var fullPath = Path.GetFullPath(Path.Combine(_basePath, normalizedPath));

        if (!fullPath.StartsWith(_basePathWithSeparator, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Caminho de arquivo fora do armazenamento permitido.");
        }

        return fullPath;
    }

    private static string NormalizeRelativePath(string relativePath)
    {
        return relativePath.Replace(Path.DirectorySeparatorChar, '/');
    }
}
