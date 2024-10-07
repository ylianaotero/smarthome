using ServiceFactory;
using WebApi.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(option =>
{
    option.Filters.Add<CustomExceptionFilter>();
    option.Filters.Add<AuthenticationFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddServices();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddConnectionString(connectionString);

builder.Services.AddControllers();

var app = builder.Build();

app.UseResponseCaching(); 

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