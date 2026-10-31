namespace ConstellationRestoreApi.Dto;

public sealed class ConstellationRestoreResponse
{
    public string MissingSymbols { get; set; } = string.Empty;
    public bool IsComplete { get; set; }
}