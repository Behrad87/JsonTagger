namespace JsonTagger.Models;

public class TagEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Section { get; set; }  // optional: nest inside a section

    public override string ToString() => string.IsNullOrEmpty(Section)
        ? $"\"{Key}\": \"{Value}\""
        : $"[{Section}] \"{Key}\": \"{Value}\"";
}
