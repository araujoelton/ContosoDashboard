using System.ComponentModel.DataAnnotations;
using ContosoDashboard.Data;
using ContosoDashboard.Models;

namespace ContosoDashboard.Services;

public interface IDocumentService
{
    Task<Document> UploadDocumentAsync(DocumentUploadRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Document>> GetAccessibleDocumentsAsync(int userId, CancellationToken cancellationToken = default);
    Task<Document?> GetDocumentByIdAsync(int documentId, int requestingUserId, CancellationToken cancellationToken = default);
    Task<bool> CanAccessDocumentAsync(int documentId, int requestingUserId, CancellationToken cancellationToken = default);
}

public class DocumentService : IDocumentService
{
    private readonly ApplicationDbContext _context;
    private readonly IFileStorageService _fileStorageService;
    private readonly IDocumentSecurityScanService _securityScanService;

    public DocumentService(
        ApplicationDbContext context,
        IFileStorageService fileStorageService,
        IDocumentSecurityScanService securityScanService)
    {
        _context = context;
        _fileStorageService = fileStorageService;
        _securityScanService = securityScanService;
    }

    public Task<Document> UploadDocumentAsync(DocumentUploadRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Envio de documentos será implementado na História de Usuário 1.");
    }

    public Task<IReadOnlyList<Document>> GetAccessibleDocumentsAsync(int userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Consulta de documentos acessíveis será implementada na História de Usuário 2.");
    }

    public Task<Document?> GetDocumentByIdAsync(int documentId, int requestingUserId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Consulta detalhada de documento será implementada na História de Usuário 2.");
    }

    public Task<bool> CanAccessDocumentAsync(int documentId, int requestingUserId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Autorização de documentos será implementada nas histórias de usuário.");
    }
}

public class DocumentUploadRequest
{
    [Required]
    public int UploadedByUserId { get; set; }

    [Required]
    public Stream FileStream { get; set; } = Stream.Null;

    [Required]
    [MaxLength(255)]
    public string FileName { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string ContentType { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }

    [Required]
    [MaxLength(100)]
    public string Category { get; set; } = DocumentCategory.Other;

    [MaxLength(1000)]
    public string? Tags { get; set; }

    public int? ProjectId { get; set; }

    public int? TaskId { get; set; }
}
