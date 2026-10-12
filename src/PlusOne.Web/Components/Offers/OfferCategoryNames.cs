namespace PlusOne.Web.Components.Offers;

public static class OfferCategoryNames
{
    public static string Get(string category) => category.Trim().ToLowerInvariant() switch
    {
        "food" => "Еда",
        "transport" => "Транспорт",
        "education" => "Образование",
        "entertainment" => "Развлечения",
        _ => category
    };
}
