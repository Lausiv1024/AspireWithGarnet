using AspireWithGarnet.Web.DbInitializer;
using AspireWithGarnet.Web;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.AddSqlServerDbContext<SampleDbContext>("sampleDb");
builder.Services.AddHostedService<Worker>();

var host = builder.Build();

using var scope = host.Services.CreateScope();
await using (var dbContext = scope.ServiceProvider.GetRequiredService<SampleDbContext>())
{
    await dbContext.Database.MigrateAsync();
    if (!await dbContext.Members.AnyAsync())
    {
        await dbContext.Members.AddRangeAsync(
            new Member { StudentNumber = "123456", Name = "John Doe" },
            new Member { StudentNumber = "654321", Name = "Jane Smith" });
        await dbContext.SaveChangesAsync();
    }
}
host.Run();
