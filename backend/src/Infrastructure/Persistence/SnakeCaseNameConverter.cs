using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence;

public static class SnakeCaseNameConverter
{
    private static readonly Regex PascalCase = new("([a-z0-9])([A-Z])", RegexOptions.Compiled);

    public static string Convert(string name) => PascalCase.Replace(name, "$1_$2").ToLower();

    public static ValueConverter<string, string> ToSnakeCase() =>
        new(v => Convert(v), v => ConvertCamelToPascal(v));

    private static string ConvertCamelToPascal(string snake)
    {
        var parts = snake.Split('_', StringSplitOptions.RemoveEmptyEntries);
        return string.Concat(parts.Select(p => char.ToUpperInvariant(p[0]) + p[1..]));
    }
}
