using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MyRhSystem.API.Endpoints;
using MyRhSystem.API.Mapper;
using MyRhSystem.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// 1) Serviços básicos
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2) Infraestrutura (DbContext, etc.)
builder.Services.AddInfrastructure(builder.Configuration);

// 3) AutoMapper
builder.Services.AddAutoMapper(typeof(UsersProfile).Assembly);

// 4) Autenticação/Autorização (JWT)
// appsettings.json deve ter a seção "Jwt": { Issuer, Audience, Key, ExpireMinutes }
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var cfg = builder.Configuration.GetSection("Jwt");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = cfg["Issuer"],

            ValidateAudience = true,
            ValidAudience = cfg["Audience"],

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg["Key"])),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(2)
        };

        // Mantém o claim "sub" como está, útil para pegar o userId
        options.MapInboundClaims = false;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// 5) Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// **IMPORTANTE**: Authentication antes de Authorization
app.UseAuthentication();
app.UseAuthorization();

// 6) Endpoints
// Auth aberto (login retorna token)
app.MapGroup("/api/auth")
   .MapAuthEndpoints();

// Grupos protegidos (exigem Bearer token)
app.MapGroup("/api/users")
   .RequireAuthorization()
   .MapUsersEndpoints();

app.MapGroup("/api/companies")
   .RequireAuthorization()
   .MapCompaniesEndpoints();


app.MapControllers();

app.Run();
