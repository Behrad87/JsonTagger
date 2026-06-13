using JsonTagger.Lib.Models;

namespace JsonTagger.Lib.Services;

public interface IJsonFileService
{
    Task<(bool IsValid, string Content)> LoadFileAsync(string filePath);
    Task<OperationResult> AppendTagsAsync(string filePath, IEnumerable<TagEntry> tags);
    Task<string> GetFormattedPreviewAsync(string filePath, IEnumerable<TagEntry> tags);
    bool ValidateJson(string content);
}
