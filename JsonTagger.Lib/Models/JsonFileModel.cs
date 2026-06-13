namespace JsonTagger.Lib.Models;

public class JsonFileModel
{
    public string FilePath { get; set; } = string.Empty;
    public string FileName => System.IO.Path.GetFileName(FilePath);
    public string Directory => System.IO.Path.GetDirectoryName(FilePath) ?? string.Empty;
    public bool IsValid { get; set; }
    public long FileSizeBytes { get; set; }
    public string FileSizeDisplay => FileSizeBytes < 1024
        ? $"{FileSizeBytes} B"
        : FileSizeBytes < 1024 * 1024
            ? $"{FileSizeBytes / 1024.0:F1} KB"
            : $"{FileSizeBytes / (1024.0 * 1024):F1} MB";
}
