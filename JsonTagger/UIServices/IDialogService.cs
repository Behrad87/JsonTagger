namespace JsonTagger.UIServices;

public interface IDialogService
{
    IReadOnlyList<string> OpenJsonFiles();

    IReadOnlyList<string> OpenFolder();
}