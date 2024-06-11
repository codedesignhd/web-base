using System.Net;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.FeatureManagement;
using Asp.Versioning.ApiExplorer;
using Serilog;
using Microsoft.AspNetCore.HttpOverrides;
using CodeDesignWebAPI.Services.Files;
using CodeDesignWebAPI.Services.Auth;
using CodeDesignDtos.Validators;
using CodeDesignWebAPI.Extensions;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddFeatureManagement();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwashbuckleSwagger();

//builder.Services.AddSwaggerGen();
builder.Services.AddApiVersioning(x =>
{
    //x.DefaultApiVersion = new ApiVersion(1, 0);
    //x.AssumeDefaultVersionWhenUnspecified = true;
    //x.ReportApiVersions = true;
    //x.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
    //    new HeaderApiVersionReader("x-api-version"),
    //    new MediaTypeApiVersionReader("x-api-version"));
}).AddApiExplorer(config =>
{
    config.GroupNameFormat = "'v'VVV";
    config.SubstituteApiVersionInUrl = true;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicies();
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICDValidatorFactory, ValidatorFactory>();
builder.Services.AddScoped<ServicesPool>();
builder.Services.AddSingleton<IFileService, FileService>();
builder.Services.AddGoogleService();
builder.Services.AddCouchbase();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
Log.Information("Starting web");

builder.Logging.ClearProviders()
    .AddLog4Net("log4net.config")
    .AddSerilog();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Typing",
    builder =>
    {
        builder.WithOrigins("http://localhost:4200", "http://www.contoso.com");
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var apiVersionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwashbuckleSwagger(apiVersionProvider);
    //app.UseSwagger();
    //app.UseSwaggerUI();
}
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
app.UseExceptionHandler(options => options.Run(async (context) =>
{
    Exception error = context.Features.Get<IExceptionHandlerFeature>().Error;
    context.Response.ContentType = "application/json";
    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
    await context.Response.WriteAsJsonAsync(error);
}));

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("Typing");
app.MapControllers();

app.Run();
