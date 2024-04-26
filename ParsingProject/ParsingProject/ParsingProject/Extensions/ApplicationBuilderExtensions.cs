using Microsoft.EntityFrameworkCore;
using ParsingProject.DAL.Context;

namespace ParsingProject.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void UseParsingProjectContext(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
        using var context = scope?.ServiceProvider.GetRequiredService<ParsingProjectContext>();
        if (context != null /*&& context.Database.GetPendingMigrations().Any()*/)
        {
            //temp clear db for dev purposes
            //bool wasDeleted = context.Database.EnsureDeleted();
            
            //context.Database.Migrate();
        }
    }
}