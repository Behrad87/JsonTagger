namespace JsonTagger.Services;

public interface IDialogService
{
    IReadOnlyList<string> OpenJsonFiles();

    IReadOnlyList<string> OpenFolder();
}