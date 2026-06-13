using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using JsonTagger.Lib.Services;
using JsonTagger.Lib.Models;

using System.Collections.ObjectModel;
using JsonTagger.UIServices;

namespace JsonTagger.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IJsonFileService _jsonService;
    private readonly IDialogService _dialogService;
    private readonly INotificationService _notifications;

    public MainViewModel(
        IJsonFileService jsonService,
        IDialogService dialogService,
        INotificationService notifications)
    {
        _jsonService = jsonService;
        _dialogService = dialogService;
        _notifications = notifications;

        Tags.Add(new JsonTagModel());
    }

    // ── Collections ─────────────────────────────────────────────────────────

    public ObservableCollection<JsonFileModel> Files { get; } = [];
    public ObservableCollection<JsonTagModel> Tags { get; } = [];

    // ── Observable properties ────────────────────────────────────────────────

    [ObservableProperty] private string statusMessage = "Ready.";
    [ObservableProperty] private bool statusIsSuccess = true;
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string jsonPreview = string.Empty;
    [ObservableProperty] private bool isPreviewVisible;

    // ── Commands ─────────────────────────────────────────────────────────────

    [RelayCommand(CanExecute = nameof(IsNotBusy))]
    private async Task BrowseFilesAsync()
    {
        var paths = _dialogService.OpenJsonFiles();
        if (paths.Count == 0) return;

        await LoadFilesAsync(paths);
    }

    [RelayCommand(CanExecute = nameof(IsNotBusy))]
    private async Task SelectFolderAsync()
    {
        var files = _dialogService.OpenFolder();
        if (files.Count == 0) return;

        await LoadFilesAsync(files);
    }

    [RelayCommand]
    private void AddTag() => Tags.Add(new JsonTagModel());

    [RelayCommand]
    private void RemoveTag(JsonTagModel tag)
    {
        if (Tags.Contains(tag))
            Tags.Remove(tag);
    }

    [RelayCommand]
    private void ClearAllTags() => Tags.Clear();

    [RelayCommand(CanExecute = nameof(HasFiles))]
    private async Task PreviewAsync()
    {
        var file = Files.First();

        try
        {
            var tags = Tags.Select(t => new TagEntry
            {
                Section = t.Section,
                Key = t.Key,
                Value = t.Value
            });

            JsonPreview = await _jsonService.GetFormattedPreviewAsync(file.FilePath, tags);
        }
        catch (Exception ex)
        {
            JsonPreview = $"// Preview failed: {ex.Message}";
        }

        IsPreviewVisible = true;
    }

    [RelayCommand(CanExecute = nameof(CanApply))]
    private async Task ApplyTagsAsync()
    {
        var validTags = Tags
            .Where(t => !string.IsNullOrWhiteSpace(t.Key))
            .Select(t => new TagEntry
            {
                Section = t.Section,
                Key = t.Key,
                Value = t.Value
            })
            .ToList();

        if (validTags.Count == 0)
        {
            SetStatus("No valid tags to apply — Key must not be empty.", success: false);
            return;
        }

        IsBusy = true;
        NotifyCommandsCanExecuteChanged();

        try
        {
            var failed = new List<string>();

            foreach (var file in Files)
            {
                var result = await _jsonService.AppendTagsAsync(file.FilePath, validTags);

                if (!result.IsSuccess)
                    failed.Add($"{file.FileName}: {result.Message}");
            }

            if (failed.Count == 0)
            {
                SetStatus($"Successfully updated {Files.Count} file(s).", success: true);
                _notifications.ShowSuccess(StatusMessage);

                // Refresh preview if visible
                if (IsPreviewVisible)
                    await PreviewAsync();
            }
            else
            {
                var msg = $"{Files.Count - failed.Count}/{Files.Count} file(s) updated. Errors: {string.Join("; ", failed)}";
                SetStatus(msg, success: false);
                _notifications.ShowError(msg);
            }
        }
        catch (Exception ex)
        {
            SetStatus(ex.Message, success: false);
            _notifications.ShowError(ex.Message);
        }
        finally
        {
            IsBusy = false;
            NotifyCommandsCanExecuteChanged();
        }
    }

    // ── Private helpers ──────────────────────────────────────────────────────

    private async Task LoadFilesAsync(IReadOnlyList<string> paths)
    {
        IsBusy = true;
        NotifyCommandsCanExecuteChanged();

        try
        {
            Files.Clear();
            JsonPreview = string.Empty;
            IsPreviewVisible = false;

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

            var invalidCount = Files.Count(f => !f.IsValid);
            var msg = invalidCount == 0
                ? $"{Files.Count} file(s) loaded."
                : $"{Files.Count} file(s) loaded — {invalidCount} invalid.";

            SetStatus(msg, success: invalidCount == 0);
        }
        catch (Exception ex)
        {
            SetStatus(ex.Message, success: false);
        }
        finally
        {
            IsBusy = false;
            NotifyCommandsCanExecuteChanged();
        }
    }

    private void SetStatus(string message, bool success)
    {
        StatusMessage = message;
        StatusIsSuccess = success;
    }

    private bool IsNotBusy => !IsBusy;
    private bool HasFiles => Files.Count > 0;
    private bool CanApply => Files.Count > 0 && !IsBusy;

    private void NotifyCommandsCanExecuteChanged()
    {
        BrowseFilesCommand.NotifyCanExecuteChanged();
        SelectFolderCommand.NotifyCanExecuteChanged();
        PreviewCommand.NotifyCanExecuteChanged();
        ApplyTagsCommand.NotifyCanExecuteChanged();
    }
}
