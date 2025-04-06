using AspireWithGarnet.Web;
using AspireWithGarnet.Web.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//builder.Services.AddOutputCache();
builder.AddRedisOutputCache("cache");
builder.AddSqlServerDbContext<SampleDbContext>("sampleDb");

builder.Services.AddHttpClient<WeatherApiClient>(client =>
    {
        // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
        // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
        client.BaseAddress = new("https+http://apiservice");
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();
app.MapGet("/members",
    [OutputCache(Duration = 60)]
(SampleDbContext db) =>
    {
        return db.Members.AsAsyncEnumerable();
    });

app.MapPost("/members",async 
    ([FromBody]Member member,
    SampleDbContext sampleDbContext,
    ILogger<Program> logger) =>
    {
        logger.LogInformation("Adding member {member}", member);
        sampleDbContext.Members.Add(member);
        await sampleDbContext.SaveChangesAsync();
        return Results.Created($"/members/{member.Id}", member);
    });

app.Run();
