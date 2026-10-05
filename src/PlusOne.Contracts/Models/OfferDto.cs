namespace PlusOne.Contracts.Models;

public sealed class OfferDto
{
    public int Id { get; init; }
    public string CompanyName { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public OfferSourceType SourceType { get; init; }
    public string SourceUrl { get; init; } = string.Empty;
    public DateOnly? ValidUntil { get; init; }
}
