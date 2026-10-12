using PlusOne.Contracts.Models;

namespace PlusOne.Api.Data.Entities;

public sealed class Offer
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public OfferSourceType SourceType { get; set; }
    public string SourceUrl { get; set; } = string.Empty;
    public DateOnly? ValidUntil { get; set; }
    public string? PromoCode { get; set; }
    public string? RedemptionInstructions { get; set; }
}
