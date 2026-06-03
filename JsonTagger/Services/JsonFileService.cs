using JsonTagger.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.IO;
using System.Text.Json;
using System.Xml;

namespace JsonTagger.Services;

public class JsonFileService : IJsonFileService
{
    public async Task<(bool IsValid, string Content)> LoadFileAsync(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
                return (false, string.Empty);

            var content = await File.ReadAllTextAsync(filePath);
            var isValid = ValidateJson(content);
            return (isValid, content);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public async Task<OperationResult> AppendTagsAsync(string filePath, IEnumerable<TagEntry> tags)
    {
        try
        {
            var tagList = tags.ToList();
            if (tagList.Count == 0)
                return OperationResult.Fail("No tags to append.");

            var content = await File.ReadAllTextAsync(filePath);

            JObject json;
            try
            {
                json = JObject.Parse(content);
            }
            catch (Newtonsoft.Json.JsonException ex)
            {
                return OperationResult.Fail("Invalid JSON file.", ex.Message);
            }

            var addedKeys = new List<string>();
            var skippedKeys = new List<string>();

            foreach (var tag in tagList)
            {
                if (string.IsNullOrWhiteSpace(tag.Key))
                    continue;

                if (!string.IsNullOrEmpty(tag.Section))
                {
                    // Insert into a section / nested object
                    var section = json[tag.Section] as JObject;
                    if (section == null)
                    {
                        section = new JObject();
                        json[tag.Section] = section;
                    }

                    if (section.ContainsKey(tag.Key))
                        skippedKeys.Add($"{tag.Section}.{tag.Key} (already exists)");
                    else
                    {
                        section[tag.Key] = tag.Value;
                        addedKeys.Add($"{tag.Section}.{tag.Key}");
                    }
                }
                else
                {
                    if (json.ContainsKey(tag.Key))
                        skippedKeys.Add($"{tag.Key} (already exists, updated)");

                    json[tag.Key] = tag.Value;
                    addedKeys.Add(tag.Key);
                }
            }

            var updatedContent = json.ToString(Newtonsoft.Json.Formatting.Indented);

            await File.WriteAllTextAsync(filePath, updatedContent);

            var details = addedKeys.Count > 0
                ? $"Added/Updated: {string.Join(", ", addedKeys)}"
                : null;

            if (skippedKeys.Count > 0)
                details += $"\nSkipped: {string.Join(", ", skippedKeys)}";

            return OperationResult.Ok($"Successfully processed {addedKeys.Count} tag(s).", details);
        }
        catch (Exception ex)
        {
            return OperationResult.Fail($"Failed to write file: {ex.Message}");
        }
    }

    public async Task<string> GetFormattedPreviewAsync(string filePath, IEnumerable<TagEntry> tags)
    {
        try
        {
            var content = await File.ReadAllTextAsync(filePath);
            var json = JObject.Parse(content);

            foreach (var tag in tags)
            {
                if (string.IsNullOrWhiteSpace(tag.Key)) continue;

                if (!string.IsNullOrEmpty(tag.Section))
                {
                    var section = json[tag.Section] as JObject ?? new JObject();
                    section[tag.Key] = tag.Value;
                    json[tag.Section] = section;
                }
                else
                {
                    json[tag.Key] = tag.Value;
                }
            }

            return json.ToString(Newtonsoft.Json.Formatting.Indented);
        }
        catch
        {
            return "// Preview unavailable — invalid JSON";
        }
    }

    public bool ValidateJson(string content)
    {
        if (string.IsNullOrWhiteSpace(content)) return false;
        try
        {
            JToken.Parse(content);
            return true;
        }
        catch
        {
            return false;
        }
    }
}