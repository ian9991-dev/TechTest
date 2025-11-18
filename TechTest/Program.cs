using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using TechTest.Auth;
using TechTest.Controllers;
using TechTest.Models;
using TechTest.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
var jwt = builder.Configuration.GetSection("Jwt").Get<JwtSettings>() ?? new JwtSettings();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret)),
        ValidateIssuer = true,
        ValidIssuer = jwt.Issuer,
        ValidateAudience = true,
        ValidAudience = jwt.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
           

            var details = context.ModelState
                .Where(kvp => kvp.Value != null && kvp.Value.Errors.Count > 0)
                .SelectMany(kvp =>
                {
                    var field = kvp.Key;

                    return kvp.Value.Errors.Select(e =>
                    {
                        var message = string.IsNullOrWhiteSpace(e.ErrorMessage)
                            ? e.Exception?.Message ?? "Invalid value"
                            : e.ErrorMessage;

                        return new TechTest.Models.Detail
                        {
                            Field = field,
                            Message = message
                        };
                    });
                })
                .ToArray();

            var payload = new TechTest.Models.BadRequestErrorResponse
            {
                Message = "One or more validation errors occurred.",
                Details = details
            };

            var result = new BadRequestObjectResult(payload);
            result.ContentTypes.Add("application/problem+json");
            return result;
        };
    })
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
 

builder.Services.AddScoped<TechTest.Services.IAccountService, TechTest.Services.AccountService>();
builder.Services.AddScoped<TechTest.Services.IUserService, TechTest.Services.UserService>();
builder.Services.AddScoped<IUserValidateService, UserValidateService>();
builder.Services.AddSingleton<TechTest.Data.IDataAccessLayer, TechTest.Data.DataAccessLayer>();
builder.Services.AddSingleton<TechTest.Services.ICodesGenerator, TechTest.Services.CodesGenerator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
