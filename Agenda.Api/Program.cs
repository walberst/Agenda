using Agenda.Application;
using Agenda.Domain.Interfaces;
using Agenda.Infrastructure;
using Agenda.Infrastructure.Logging;
using Agenda.Infrastructure.Messaging;
using Agenda.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

// Configura o Serilog usando appsettings.json e variáveis de ambiente
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

SerilogConfig.Configure(configuration);
Log.Information("Iniciando aplicação...");

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001");

    // Define Serilog como logger da aplicação
    builder.Host.UseSerilog();

    // Registro dos serviços da aplicação
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    // Configura Swagger com suporte a JWT
    builder.Services.AddSwaggerGen(c =>
    {
        var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
        c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Insira o token JWT"
        });

        c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    // Permite requisições CORS de qualquer origem
    builder.Services.AddCors(opt =>
    {
        opt.AddPolicy("CorsPolicy", policy =>
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader()
        );
    });

    // Registro de serviços de aplicação e infraestrutura
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("DefaultConnection")!);

    builder.Services.AddSingleton<IMessageBusService, RabbitMqService>();
    builder.Services.AddScoped<IJwtService, JwtService>();

    // Configuração de autenticação JWT
    var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

    var app = builder.Build();

    // Middleware e inicialização
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        // Aplica migrações automaticamente em ambiente de desenvolvimento
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<Agenda.Infrastructure.Data.AppDbContext>();
        db.Database.Migrate();
    }

    app.UseCors("CorsPolicy");
    app.UseHttpsRedirection();
    app.UseSerilogRequestLogging();

    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    var serverAddresses = app.Urls.Any()
        ? string.Join(", ", app.Urls)
        : "Endereço não disponível";

    Log.Information("Aplicação iniciada com sucesso em: {Urls}", serverAddresses);
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Erro fatal ao iniciar a aplicação");
}
finally
{
    Log.CloseAndFlush();
}
