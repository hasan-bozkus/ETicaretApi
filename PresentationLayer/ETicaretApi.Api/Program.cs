using ETicaretApi.Api.Configurations.ColumnWriter;
using ETicaretApi.Api.Extensions;
using ETicaretApi.Application;
using ETicaretApi.Application.Validators.Product;
using ETicaretApi.Infranstructure;
using ETicaretApi.Infranstructure.Enums;
using ETicaretApi.Infranstructure.Filters;
using ETicaretApi.Infranstructure.Services.Storage.Azure;
using ETicaretApi.Presistence;
using ETicaretApi.SignalR;
using ETicaretApi.SignalR.Hubs;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.PostgreSQL;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor(); //Client'tan gelen request neticesinde oluşturulan HttpContext nesnesine katmanlardaki class'lar üzerinden(businness logic) erişebilmemizi sağlayan bir servistir. 
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
builder.Services.AddSignalRServices();

//builder.Services.AddStorage(StorageType.Local);
builder.Services.AddStorage<AzureStorage>();


builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials()
));

Logger log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log.txt")
    .WriteTo.PostgreSQL(builder.Configuration.GetConnectionString("PostgreSQL"), "logs", 
    needAutoCreateTable: true, columnOptions: new Dictionary<string, ColumnWriterBase>
    {
        { "message", new RenderedMessageColumnWriter() },
        { "message_template", new MessageTemplateColumnWriter() },
        { "level", new LevelColumnWriter() },
        { "time_stamp", new TimestampColumnWriter() },
        { "exception", new ExceptionColumnWriter() },
        { "log_event", new LogEventSerializedColumnWriter() },
        { "user_name", new UserNameColumnWriter() }
    })
    .WriteTo.Seq(builder.Configuration["Seq:serverUrl"])
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog(log);

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;

});

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
            ValidateAudience = true,      //oluşturulacak token değerini kimlerin/hangi originlerin/sitelerin kullanacağını belirleyen değerdir.
            ValidateIssuer = true,        // oluşturulacak token değerini kimin dağıttığını ifade edeceğimiz alandır.
            ValidateLifetime = true,       // oluşturulan token değerinin süresini doğrulayacak ve sorgulayacak olan değerdir.
            ValidateIssuerSigningKey = true,    //üretilen token değerinin uygulamamıza ait bir mdeğer olduğunu ifade eden security key verisinin doğrulamasıdır.

            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,

            NameClaimType = ClaimTypes.Name //jwt üzerinde Name Claime karşılık gelen değeri User.Identity.Name propertysinden elde edebiliriz.
        };
    });

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler<Program>(app.Services.GetRequiredService<ILogger<Program>>());

app.UseStaticFiles();

app.UseSerilogRequestLogging();

app.UseHttpLogging();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var username = context.User.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;
    LogContext.PushProperty("user_name", username);
    await next();
});

app.MapControllers();
app.MapHubs();

app.Run();
