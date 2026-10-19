namespace PlusOne.Api.Data;

public static class SqliteText
{
    // SQLite lower() и LIKE не меняют регистр кириллицы, поэтому сравнение
    // делаем по заранее нормализованной строке.
    public static string Fold(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        var buffer = new char[value.Length];
        var length = 0;

        foreach (var character in value)
        {
            if (char.IsWhiteSpace(character))
            {
                continue;
            }

            buffer[length++] = char.ToLowerInvariant(character);
        }

        return new string(buffer, 0, length);
    }
}
