using Url.Short_Api.Extensions;
using Url.Short_Api.Services.Authorization;
using Url.Short_Api.Services.ExpiredLinkDeletion;
using Url.Short_Api.Services.UrlShortener;
using Url.Short_Api.Services.UrlShortenRepository;
using Url.Short_Api.Services.UrlTypeRepository;

const string corsPolicyName = "allow";
var builder = WebApplication.CreateBuilder(args);
var tokenKey = builder.Configuration["TokenKey"]!;
builder.Services.AddCors(corsPolicyName);
builder.Services.AddPgsql(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuth(tokenKey);
builder.Services.AddSwagger();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<IUrlShortenerService, UrlShortenerService>();
builder.Services.AddScoped<IUrlShortenRepositoryService, UrlShortenRepositoryService>();
builder.Services.AddScoped<IUrlTypeRepositoryService, UrlTypeRepositoryService>();
builder.Services.AddHostedService<ExpiredLinkDeletionService>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(corsPolicyName);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();