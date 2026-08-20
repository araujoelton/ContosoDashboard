namespace ContosoDashboard.Services;

public interface IDocumentSecurityScanService
{
    Task<DocumentSecurityScanResult> ScanAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default);
}

public sealed record DocumentSecurityScanResult(bool IsClean, string Status, string? Message = null)
{
    public static DocumentSecurityScanResult Clean()
    {
        return new DocumentSecurityScanResult(true, "Clean");
    }

    public static DocumentSecurityScanResult Blocked(string message)
    {
        return new DocumentSecurityScanResult(false, "Blocked", message);
    }
}

public class SimulatedDocumentSecurityScanService : IDocumentSecurityScanService
{
    private static readonly string[] SuspiciousTerms = ["virus", "malware", "infected", "eicar"];

    public Task<DocumentSecurityScanResult> ScanAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!fileStream.CanRead)
        {
            return Task.FromResult(DocumentSecurityScanResult.Blocked("O arquivo não pôde ser lido para verificação."));
        }

        var hasSuspiciousName = SuspiciousTerms.Any(term =>
            fileName.Contains(term, StringComparison.OrdinalIgnoreCase));

        if (hasSuspiciousName)
        {
            return Task.FromResult(DocumentSecurityScanResult.Blocked("A verificação simulada sinalizou o arquivo como suspeito."));
        }

        return Task.FromResult(DocumentSecurityScanResult.Clean());
    }
}
