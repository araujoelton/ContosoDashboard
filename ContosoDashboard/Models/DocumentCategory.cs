namespace ContosoDashboard.Models;

public static class DocumentCategory
{
    public const string ProjectDocuments = "Documentos de Projeto";
    public const string TeamResources = "Recursos da Equipe";
    public const string PersonalFiles = "Arquivos Pessoais";
    public const string Reports = "Relatórios";
    public const string Presentations = "Apresentações";
    public const string Other = "Outros";

    public static readonly IReadOnlyList<string> All = new[]
    {
        ProjectDocuments,
        TeamResources,
        PersonalFiles,
        Reports,
        Presentations,
        Other
    };
}
