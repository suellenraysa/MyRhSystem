using MyRhSystem.API.Endpoints;
using MyRhSystem.API.Mapper;
using MyRhSystem.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Program.cs (API)
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddAutoMapper(typeof(UsersProfile).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGroup("/api/users").MapUsersEndpoints();
app.MapGroup("/api/auth").MapAuthEndpoints();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
