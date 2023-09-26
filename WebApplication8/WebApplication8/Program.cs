using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));


builder.Host.ConfigureAppConfiguration((context, config) => {
    var settings = config.Build();
    var keyVaultURL = settings["KeyVaultConfiguration:KeyVaultURL"];
    var keyVaultClientId = settings["KeyVaultConfiguration:ClientId"];
    var keyVaultClientSecret = settings["KeyVaultConfiguration:ClientSecret"];
    config.AddAzureKeyVault(keyVaultURL, keyVaultClientId, keyVaultClientSecret, new DefaultKeyVaultSecretManager());
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ToDo API",
        Description = "A simple example ASP.NET Core Web API",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Shayne Boyer",
            Email ="ansaritf08@gmail.com",
            Url = new Uri("https://twitter.com/spboyer"),
        },
        License = new OpenApiLicense
        {
            Name = "Use under LICX",
            Url = new Uri("https://example.com/license"),
        }
    });

    // Set the comments path for the Swagger JSON and UI.
    //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    //c.IncludeXmlComments(xmlPath);
});

//builder.Services.ConfigureSwaggerGen(setup =>
//{
//    setup.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Title = "Weather Forecasts",
//        Version = "v1"
//    });
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();  
}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("v1/swagger.json", "MyAPI V1");
});
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
