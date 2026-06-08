using GiroDaCopa.Domain.Entities;
using GiroDaCopa.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GiroDaCopa.Persistence.Seed;

public static class CountrySeeder
{
    public static async Task SeedAsync(GiroDaCopaDbContext context)
    {
        if (await context.Countries.AnyAsync())
            return;

        var countries = new[]
        {
            new Country("Brazil", "BR", "BRA", "🇧🇷"),
            new Country("Argentina", "AR", "ARG", "🇦🇷"),
            new Country("United States", "US", "USA", "🇺🇸"),
            new Country("Mexico", "MX", "MEX", "🇲🇽"),
            new Country("Canada", "CA", "CAN", "🇨🇦"),
            new Country("France", "FR", "FRA", "🇫🇷"),
            new Country("Germany", "DE", "GER", "🇩🇪"),
            new Country("Japan", "JP", "JPN", "🇯🇵"),
            new Country("Spain", "ES", "ESP", "🇪🇸"),
            new Country("England", "GB", "ENG", "🏴󠁧󠁢󠁥󠁮󠁧󠁿"),
            new Country("Croatia", "HR", "CRO", "🇭🇷"),
            new Country("Morocco", "MA", "MAR", "🇲🇦"),
            new Country("Portugal", "PT", "POR", "🇵🇹"),
            new Country("Netherlands", "NL", "NED", "🇳🇱"),
            new Country("Senegal", "SN", "SEN", "🇸🇳"),
            new Country("Colombia", "CO", "COL", "🇨🇴"),
            new Country("South Africa", "ZA", "RSA", "🇿🇦"),
            new Country("South Korea", "KR", "KOR", "🇰🇷"),
            new Country("Czech Republic", "CZ", "CZE", "🇨🇿"),
            new Country("Bosnia & Herzegovina", "BA", "BIH", "🇧🇦"),
            new Country("Qatar", "QA", "QAT", "🇶🇦"),
            new Country("Switzerland", "CH", "SUI", "🇨🇭"),
            new Country("Haiti", "HT", "HAI", "🇭🇹"),
            new Country("Scotland", "GB", "SCO", "🏴󠁧󠁢󠁳󠁣󠁴󠁿"),
            new Country("Paraguay", "PY", "PAR", "🇵🇾"),
            new Country("Australia", "AU", "AUS", "🇦🇺"),
            new Country("Turkey", "TR", "TUR", "🇹🇷"),
            new Country("Curaçao", "CW", "CUW", "🇨🇼"),
            new Country("Ivory Coast", "CI", "CIV", "🇨🇮"),
            new Country("Ecuador", "EC", "ECU", "🇪🇨"),
            new Country("Sweden", "SE", "SWE", "🇸🇪"),
            new Country("Tunisia", "TN", "TUN", "🇹🇳"),
            new Country("Belgium", "BE", "BEL", "🇧🇪"),
            new Country("Egypt", "EG", "EGY", "🇪🇬"),
            new Country("Iran", "IR", "IRN", "🇮🇷"),
            new Country("New Zealand", "NZ", "NZL", "🇳🇿"),
            new Country("Cape Verde", "CV", "CPV", "🇨🇻"),
            new Country("Saudi Arabia", "SA", "KSA", "🇸🇦"),
            new Country("Uruguay", "UY", "URU", "🇺🇾"),
            new Country("Iraq", "IQ", "IRQ", "🇮🇶"),
            new Country("Norway", "NO", "NOR", "🇳🇴"),
            new Country("Algeria", "DZ", "ALG", "🇩🇿"),
            new Country("Austria", "AT", "AUT", "🇦🇹"),
            new Country("Jordan", "JO", "JOR", "🇯🇴"),
            new Country("DR Congo", "CD", "COD", "🇨🇩"),
            new Country("Uzbekistan", "UZ", "UZB", "🇺🇿"),
            new Country("Ghana", "GH", "GHA", "🇬🇭"),
            new Country("Panama", "PA", "PAN", "🇵🇦")
        };

        context.Countries.AddRange(countries);
        await context.SaveChangesAsync();
    }
}
