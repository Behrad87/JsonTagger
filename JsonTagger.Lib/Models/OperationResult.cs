namespace JsonTagger.Lib.Models;

/// <summary>
/// Represents the outcome of a file-write operation,
/// carrying success/failure state, a summary message, and optional details.
/// </summary>
public sealed class OperationResult
{
    public bool IsSuccess { get; private init; }
    public string Message { get; private init; } = string.Empty;
    public string? Details { get; private init; }

    public static OperationResult Ok(string message, string? details = null)
        => new() { IsSuccess = true, Message = message, Details = details };

    public static OperationResult Fail(string message, string? details = null)
        => new() { IsSuccess = false, Message = message, Details = details };
}
