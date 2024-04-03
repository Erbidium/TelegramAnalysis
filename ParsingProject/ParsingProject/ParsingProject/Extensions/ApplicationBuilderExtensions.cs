using Microsoft.EntityFrameworkCore;
using ParsingProject.DAL.Context;

namespace ParsingProject.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void UseParsingProjectContext(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
        using var context = scope?.ServiceProvider.GetRequiredService<ParsingProjectContext>();
        if (context != null && context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
    }
}