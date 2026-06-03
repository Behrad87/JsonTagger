namespace JsonTagger.Services;

public interface IDialogService
{
    IReadOnlyList<string> OpenJsonFiles();

    string? OpenFolder();
}