using Microsoft.EntityFrameworkCore;
using PlusOne.Api.Data.Entities;
using PlusOne.Contracts.Models;

namespace PlusOne.Api.Data;

public sealed class PlusOneDbContext(DbContextOptions<PlusOneDbContext> options)
    : DbContext(options)
{
    public DbSet<Offer> Offers => Set<Offer>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var offer = modelBuilder.Entity<Offer>();

        offer.ToTable("Offers");
        offer.HasKey(item => item.Id);
        offer.Property(item => item.CompanyName).HasMaxLength(120).IsRequired();
        offer.Property(item => item.Title).HasMaxLength(160).IsRequired();
        offer.Property(item => item.Description).HasMaxLength(1000).IsRequired();
        offer.Property(item => item.Category).HasMaxLength(60).IsRequired();
        offer.Property(item => item.SourceType).HasConversion<string>().HasMaxLength(20);
        offer.Property(item => item.SourceUrl).HasMaxLength(500).IsRequired();
        offer.Property(item => item.PromoCode).HasMaxLength(50);
        offer.Property(item => item.RedemptionInstructions).HasMaxLength(500);

        var role = modelBuilder.Entity<Role>();

        role.ToTable("Roles");
        role.HasKey(item => item.Id);
        role.Property(item => item.Name).HasMaxLength(30).IsRequired();

        var user = modelBuilder.Entity<User>();

        user.ToTable("Users");
        user.HasKey(item => item.Id);
        user.Property(item => item.UserName).HasMaxLength(80).IsRequired();
        user.Property(item => item.Login).HasMaxLength(50).IsRequired();
        user.Property(item => item.Password).HasMaxLength(100).IsRequired();
        user.HasIndex(item => item.Login).IsUnique();
        user.HasOne(item => item.Role)
            .WithMany()
            .HasForeignKey(item => item.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        offer.HasData(
            new Offer
            {
                Id = 1,
                CompanyName = "Добрый хлеб",
                Title = "Скидка 20% на вечернюю выпечку",
                Description = "Выберите свежую выпечку после 18:00 и получите студенческую скидку на кассе.",
                Category = "Food",
                SourceType = OfferSourceType.Partner,
                SourceUrl = "https://example.com/offers/bakery",
                ValidUntil = new DateOnly(2026, 12, 31),
                PromoCode = "STUDENT20",
                RedemptionInstructions = "Покажите промокод кассиру вместе со студенческим билетом."
            },
            new Offer
            {
                Id = 2,
                CompanyName = "Open Mobility",
                Title = "Первая поездка бесплатно",
                Description = "Активируйте промокод перед первой поездкой и получите до 30 минут бесплатного проката.",
                Category = "Transport",
                SourceType = OfferSourceType.Partner,
                SourceUrl = "https://example.com/offers/transport",
                ValidUntil = new DateOnly(2026, 11, 30),
                PromoCode = "PLUSRIDE",
                RedemptionInstructions = "Введите код в разделе «Промокоды» мобильного приложения до начала поездки."
            },
            new Offer
            {
                Id = 3,
                CompanyName = "Открытый университет",
                Title = "Бесплатный курс по программированию",
                Description = "Получите бесплатный доступ к вводному курсу и практическим заданиям на четыре недели.",
                Category = "Education",
                SourceType = OfferSourceType.OpenSource,
                SourceUrl = "https://opensource.example.com/courses/programming",
                PromoCode = "STARTPLUS",
                RedemptionInstructions = "Укажите промокод при регистрации на странице курса."
            },
            new Offer
            {
                Id = 4,
                CompanyName = "Кино для всех",
                Title = "Билет на студенческий кинопоказ",
                Description = "Посетите специальный показ классического кино и обсуждение с кинокритиком.",
                Category = "Entertainment",
                SourceType = OfferSourceType.OpenSource,
                SourceUrl = "https://opensource.example.com/events/cinema",
                ValidUntil = new DateOnly(2026, 10, 31),
                PromoCode = "CAMPUSFILM",
                RedemptionInstructions = "Назовите промокод на стойке регистрации и покажите студенческий билет."
            },
            new Offer
            {
                Id = 5,
                CompanyName = "Фермерский рынок",
                Title = "Скидка 15% на готовые наборы",
                Description = "Выберите готовый набор сезонных овощей и фруктов по специальной студенческой цене.",
                Category = "Food",
                SourceType = OfferSourceType.Partner,
                SourceUrl = "https://example.com/offers/vegetables",
                ValidUntil = new DateOnly(2026, 12, 15),
                PromoCode = "CAMPUS15",
                RedemptionInstructions = "Покажите код продавцу перед оплатой набора."
            },
            new Offer
            {
                Id = 6,
                CompanyName = "Городской музей",
                Title = "Два билета по цене одного",
                Description = "Приходите вдвоём на постоянную экспозицию и оплатите только один входной билет.",
                Category = "Entertainment",
                SourceType = OfferSourceType.Partner,
                SourceUrl = "https://example.com/offers/museum",
                ValidUntil = new DateOnly(2027, 1, 31),
                PromoCode = "PLUS1MUSEUM",
                RedemptionInstructions = "Назовите промокод в кассе музея и предъявите действующий студенческий билет."
            });
    }
}
