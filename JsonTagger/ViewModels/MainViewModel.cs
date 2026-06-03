using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using JsonTagger.Services;

using System.Collections.ObjectModel;

namespace JsonTagger.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IJsonFileService _jsonService;
    private readonly IDialogService _dialogService;

    public MainViewModel(
        IJsonFileService jsonService,
        IDialogService dialogService)
    {
        _jsonService = jsonService;
        _dialogService = dialogService;

        Tags.Add(new JsonTagModel());
    }

    public ObservableCollection<JsonFileModel> Files { get; } = [];

    public ObservableCollection<JsonTagModel> Tags { get; } = [];

    [ObservableProperty]
    private string statusMessage = string.Empty;

    [ObservableProperty]
    private bool statusIsSuccess;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string jsonPreview = string.Empty;

    [ObservableProperty]
    private bool isPreviewVisible;

    [RelayCommand]
    private async Task BrowseFilesAsync()
    {
        var paths = _dialogService.OpenJsonFiles();

        if (paths.Count == 0)
            return;

        IsBusy = true;

        try
        {
            Files.Clear();

            foreach (var path in paths)
            {
                var (isValid, _) = await _jsonService.LoadFileAsync(path);

                var info = new FileInfo(path);

                Files.Add(new JsonFileModel
                {
                    FilePath = path,
                    IsValid = isValid,
                    FileSizeBytes = info.Length
                });
            }

            StatusMessage = $"{Files.Count} file(s) loaded.";
            StatusIsSuccess = true;
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
            StatusIsSuccess = false;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void AddTag()
    {
        Tags.Add(new JsonTagModel());
    }

    [RelayCommand]
    private void RemoveTag(JsonTagModel tag)
    {
        if (Tags.Contains(tag))
            Tags.Remove(tag);
    }

    [RelayCommand]
    private void ClearAllTags()
    {
        Tags.Clear();
    }

    [RelayCommand]
    private async Task PreviewAsync()
    {
        if (Files.Count == 0)
            return;

        var file = Files.First();

        JsonPreview = await File.ReadAllTextAsync(file.FilePath);

        IsPreviewVisible = true;
    }

    [RelayCommand]
    private async Task ApplyTagsAsync()
    {
        if (Files.Count == 0)
            return;

        IsBusy = true;

        try
        {
            foreach (var file in Files)
            {
                var tagEntries = Tags
                    .Select(t => new TagEntry
                    {
                        Section = t.Section,
                        Key = t.Key,
                        Value = t.Value
                    });

                await _jsonService.AppendTagsAsync(
                    file.FilePath,
                    tagEntries);
            }

            StatusMessage =
                $"Successfully updated {Files.Count} file(s).";

            StatusIsSuccess = true;
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
            StatusIsSuccess = false;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task SelectFolderAsync()
    {
        var folder = _dialogService.OpenFolder();

        if (string.IsNullOrWhiteSpace(folder))
            return;

        IsBusy = true;

        try
        {
            Files.Clear();

            var jsonFiles = Directory.GetFiles(
                folder,
                "*.json",
                SearchOption.AllDirectories);

            foreach (var file in jsonFiles)
            {
                var (isValid, _) =
                    await _jsonService.LoadFileAsync(file);

                var info = new FileInfo(file);

                Files.Add(new JsonFileModel
                {
                    FilePath = file,
                    IsValid = isValid,
                    FileSizeBytes = info.Length
                });
            }

            StatusMessage =
                $"{Files.Count} JSON file(s) discovered.";

            StatusIsSuccess = true;
        }
        finally
        {
            IsBusy = false;
        }
    }
}