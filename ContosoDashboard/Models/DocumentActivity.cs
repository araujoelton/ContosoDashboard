using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoDashboard.Models;

public class DocumentActivity
{
    [Key]
    public int DocumentActivityId { get; set; }

    [Required]
    public int DocumentId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    [MaxLength(50)]
    public string ActivityType { get; set; } = DocumentActivityType.Upload;

    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

    [MaxLength(1000)]
    public string? Details { get; set; }

    [ForeignKey("DocumentId")]
    public virtual Document Document { get; set; } = null!;

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
}

public static class DocumentActivityType
{
    public const string Upload = "Upload";
    public const string Download = "Download";
    public const string Preview = "Preview";
    public const string MetadataUpdated = "MetadataUpdated";
    public const string FileReplaced = "FileReplaced";
    public const string Deleted = "Deleted";
    public const string Shared = "Shared";
}
