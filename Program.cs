using TestBlazorGridEnumJson.Services;
using TestBlazorGridEnumJson.Components;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDevExpressBlazor(options =>
{
    options.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5;
});
builder.Services.AddMvc()
     .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(item: new JsonStringEnumConverter()))
    ;

builder.Services.AddSingleton<WeatherForecastService>();


// Server Side Blazor doesn't register HttpClient by default
if (!builder.Services.Any(x => x.ServiceType == typeof(HttpClient)))
{
    // Setup HttpClient for server side in a client side compatible fashion
    builder.Services.AddScoped<HttpClient>(s =>
    {
        // Creating the URI helper needs to wait until the JS Runtime is initialized, so defer it.
        var uriHelper = s.GetRequiredService<NavigationManager>();
        return new HttpClient
        {
            BaseAddress = new Uri(uriHelper.BaseUri)
        };
    });
}
builder.Services.AddControllers();

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

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();

app.Run();