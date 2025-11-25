using Microsoft.EntityFrameworkCore;
using AuthApi.Data;
var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// DB connection
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
if (string.IsNullOrEmpty(connectionString)){
    connectionString = "Host=db;Port=5432;Database=UsersJC;Username=postgres;Password=postgres;SslMode=Require";
}
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connectionString));
var app = builder.Build();

// Migrations at startup
using (var scope = app.Services.CreateScope())
{
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
db.Database.Migrate();
}
app.MapControllers();
app.Run();