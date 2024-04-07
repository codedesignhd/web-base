using System.Net;
using System.Text;
using CodeDesign.DTO.Validators;
using CodeDesign.WebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DemoAPI",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
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
//builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<RegisterValidator>();
builder.Services.AddSingleton<AppValidator>();
builder.Services.AddSingleton<AppUserProvider>();
builder.Services.AddSingleton<AppDependencyProvider>();
builder.Logging.ClearProviders()
    .AddLog4Net("log4net.config");
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Typing",
    builder =>
    {
        builder.WithOrigins("http://localhost:4200",
    "http://www.contoso.com");
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

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
