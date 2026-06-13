namespace JsonTagger.ViewModels;

public partial class TagEntryViewModel : ObservableObject
{
    [ObservableProperty]
    private string _key = string.Empty;

    [ObservableProperty]
    private string _value = string.Empty;

    [ObservableProperty]
    private string _section = string.Empty;

    [ObservableProperty]
    private bool _isValid;

    public Guid Id { get; } = Guid.NewGuid();

    public event Action<TagEntryViewModel>? RemoveRequested;

    partial void OnKeyChanged(string value) => ValidateEntry();
    partial void OnValueChanged(string value) => ValidateEntry();

    private void ValidateEntry()
    {
        IsValid = !string.IsNullOrWhiteSpace(Key) && !string.IsNullOrWhiteSpace(Value);
    }

    [RelayCommand]
    private void Remove() => RemoveRequested?.Invoke(this);

    public TagEntry ToModel() => new()
    {
        Id = Id,
        Key = Key.Trim(),
        Value = Value.Trim(),
        Section = string.IsNullOrWhiteSpace(Section) ? null : Section.Trim()
    };
}