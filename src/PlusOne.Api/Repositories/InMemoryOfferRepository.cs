using PlusOne.Contracts.Models;

namespace PlusOne.Api.Repositories;

public sealed class InMemoryOfferRepository : IOfferRepository
{
    private static readonly IReadOnlyList<OfferDto> Offers =
    [
        new()
        {
            Id = 1,
            CompanyName = "Добрый хлеб",
            Title = "Скидка на выпечку",
            Description = "Скидка 20% на свежую выпечку после 18:00.",
            Category = "Food",
            SourceType = OfferSourceType.Partner,
            SourceUrl = "https://example.com/offers/bakery",
            ValidUntil = new DateOnly(2026, 12, 31)
        },
        new()
        {
            Id = 2,
            CompanyName = "Городской транспорт",
            Title = "Льготный проезд",
            Description = "Промокод на первую поездку в городском транспорте.",
            Category = "Transport",
            SourceType = OfferSourceType.Partner,
            SourceUrl = "https://example.com/offers/transport",
            ValidUntil = new DateOnly(2026, 11, 30)
        },
        new()
        {
            Id = 3,
            CompanyName = "Открытый университет",
            Title = "Бесплатный курс по программированию",
            Description = "Вводный курс по основам программирования для начинающих.",
            Category = "Education",
            SourceType = OfferSourceType.OpenSource,
            SourceUrl = "https://opensource.example.com/courses/programming"
        },
        new()
        {
            Id = 4,
            CompanyName = "Кино для всех",
            Title = "Бесплатный кинопоказ",
            Description = "Открытый показ классического кино во дворе библиотеки.",
            Category = "Entertainment",
            SourceType = OfferSourceType.OpenSource,
            SourceUrl = "https://opensource.example.com/events/cinema",
            ValidUntil = new DateOnly(2026, 10, 31)
        },
        new()
        {
            Id = 5,
            CompanyName = "Фермерский рынок",
            Title = "Набор сезонных овощей",
            Description = "Специальная цена на набор овощей от местных фермеров.",
            Category = "Food",
            SourceType = OfferSourceType.Partner,
            SourceUrl = "https://example.com/offers/vegetables",
            ValidUntil = new DateOnly(2026, 12, 15)
        },
        new()
        {
            Id = 6,
            CompanyName = "Open Mobility",
            Title = "Велосипед напрокат",
            Description = "Открытая программа бесплатного проката велосипедов.",
            Category = "Transport",
            SourceType = OfferSourceType.OpenSource,
            SourceUrl = "https://opensource.example.com/mobility/bikes"
        }
    ];

    public Task<IReadOnlyList<OfferDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(Offers);
    }
}
