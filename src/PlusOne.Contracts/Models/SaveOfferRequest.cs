using System.ComponentModel.DataAnnotations;

namespace PlusOne.Contracts.Models;

public sealed class SaveOfferRequest
{
    [Required(ErrorMessage = "Укажите компанию.")]
    [StringLength(120, ErrorMessage = "Название компании должно содержать не более 120 символов.")]
    public string CompanyName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Укажите название предложения.")]
    [StringLength(160, ErrorMessage = "Название предложения должно содержать не более 160 символов.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Добавьте описание предложения.")]
    [StringLength(1000, ErrorMessage = "Описание должно содержать не более 1000 символов.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Выберите категорию.")]
    [StringLength(60, ErrorMessage = "Название категории должно содержать не более 60 символов.")]
    public string Category { get; set; } = string.Empty;

    [EnumDataType(typeof(OfferSourceType), ErrorMessage = "Укажите допустимый тип источника.")]
    public OfferSourceType SourceType { get; set; } = OfferSourceType.Partner;

    [Required(ErrorMessage = "Укажите ссылку на источник.")]
    [Url(ErrorMessage = "Укажите полную ссылку, начинающуюся с http:// или https://.")]
    [StringLength(500, ErrorMessage = "Ссылка должна содержать не более 500 символов.")]
    public string SourceUrl { get; set; } = string.Empty;

    public DateOnly? ValidUntil { get; set; }

    [StringLength(50, ErrorMessage = "Промокод должен содержать не более 50 символов.")]
    public string? PromoCode { get; set; }

    [StringLength(500, ErrorMessage = "Инструкция должна содержать не более 500 символов.")]
    public string? RedemptionInstructions { get; set; }
}
