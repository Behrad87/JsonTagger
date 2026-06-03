namespace JsonTagger.Models;

public class OperationResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Details { get; set; }

    public static OperationResult Ok(string message, string? details = null)
        => new() { Success = true, Message = message, Details = details };

    public static OperationResult Fail(string message, string? details = null)
        => new() { Success = false, Message = message, Details = details };
}