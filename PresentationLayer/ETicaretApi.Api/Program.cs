using ETicaretApi.Application;
using ETicaretApi.Application.Validators.Product;
using ETicaretApi.Infranstructure;
using ETicaretApi.Infranstructure.Enums;
using ETicaretApi.Infranstructure.Filters;
using ETicaretApi.Infranstructure.Services.Storage.Azure;
using ETicaretApi.Presistence;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();

//builder.Services.AddStorage(StorageType.Local);
builder.Services.AddStorage<AzureStorage>();


builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod()
));

builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
    .AddFluentValidation(configuration => 
    configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin", options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true,      //oluþturulacak token deðerini kimlerin/hangi originlerin/sitelerin kullanacaðýný belirleyen deðerdir.
            ValidateIssuer = true,        // oluþturulacak token deðerini kimin daðýttýðýný ifade edeceðimiz alandýr.
            ValidateLifetime = true,       // oluþturulan token deðerinin süresini doðrulayacak ve sorgulayacak olan deðerdir.
            ValidateIssuerSigningKey = true,    //üretilen token deðerinin uygulamamýza ait bir mdeðer olduðunu ifade eden security key verisinin doðrulamasýdýr.

            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false
        };
    });

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
