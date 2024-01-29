using EnergiaClienteWebApi.Database;
using EnergiaClienteWebApi.RequestModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapGet("/ping", () => "pong");
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();

record Product(string name, decimal cost, string code);