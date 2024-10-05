using DataAccess;
using Microsoft.EntityFrameworkCore;
using ServiceFactory;
using WebApi.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(option =>
{
   // option.Filters.Add<CustomExceptionFilter>();
    option.Filters.Add<AuthenticationFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddServices();

builder.Services.AddDbContext<SmartHomeContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("DataAccess")));


builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();