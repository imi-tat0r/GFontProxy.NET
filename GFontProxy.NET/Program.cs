using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;

const string copyrightNotice = @"/********* GFontProxy.NET by ev0lve Digital. All rights reserved. *********
Web: https://googleapisproxy.com
GitHub: https://github.com/imi-tat0r/GFontProxy.NET
Docker: https://hub.docker.com/r/imitat0r/gfontproxy.net

GFontProxy.NET acts as a proxy in order to cache and provide Google fonts
in compliance with GDPR (if hosted in EU)
***************************************************************************/

";

var corsOrigin = Environment.GetEnvironmentVariable("CORS_ORIGIN") ?? "*";
var fontPath = Path.Combine(Directory.GetCurrentDirectory(), "fonts");

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();

// add CORS support (needed so fonts can get loaded from other domains)
builder.Services.AddCors(options => options.AddDefaultPolicy(p => p.WithOrigins(corsOrigin).AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();
app.UseCors(options => options.WithOrigins(corsOrigin).AllowAnyHeader().AllowAnyMethod());

// add static files and override default file provider
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(fontPath),
    RequestPath = new PathString("/fonts")
});

async Task<string> GetFont(string fullPath, string userAgent)
{
    // empty path
    if (fullPath.Length == 0)
        return "";

    // request from google fonts
    var client = new HttpClient();
    client.BaseAddress = new Uri("https://fonts.googleapis.com/");

    // user agent needs to be sent in order to get the correct font type
    client.DefaultRequestHeaders.Add("User-Agent", userAgent);
    var response = await client.GetAsync(fullPath).ConfigureAwait(false);

    // return empty string if request failed
    if (!response.IsSuccessStatusCode)
        return "";

    // get result and parse all fonts through regex
    var result = response.Content.ReadAsStringAsync().Result;
    var regex = new Regex(@"url\((.*)\)\s");
    var matches = regex.Matches(result);

    // loop through all matches
    foreach (Match match in matches)
    {
        // url is in group 1
        var fontUrl = match.Groups[1].Value;
        var fontName = fontUrl.Split("/").Last();

        // download font if needed
        if (!File.Exists(Path.Combine(fontPath, fontName)))
            File.WriteAllBytes(Path.Combine(fontPath, fontName),
                await client.GetByteArrayAsync(fontUrl).ConfigureAwait(false));

        // replace font url with our own
        result = result.Replace(fontUrl, $"{Environment.GetEnvironmentVariable("WEBSITE_URL")}/fonts/{fontName}");
    }

    return copyrightNotice + result;
}

// map endpoint
app.MapGet("/{fontType}", async (HttpRequest request, [FromServices]IMemoryCache cache, string fontType) =>
{
    // get UA and full request path including query string
    var userAgent = request.Headers["User-Agent"].ToString();
    var fullPath = fontType + request.QueryString.Value;

    // get css from cache or create it
    var cachedResult = await cache.GetOrCreateAsync(fullPath + userAgent, entry =>
    {
        // set expiration to 1 day
        entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24);
        
        // get font from google fonts
        return GetFont(fullPath, userAgent);
    });
    
    // return css
    return Results.Text(cachedResult, "text/css");
});

app.Run();