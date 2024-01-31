using EnergiaClienteWebApi.Databases;
using EnergiaClienteWebApi.Handlers;
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

app.MapPost("BillingTest", (int id, int month, int year) => EnergiaClienteHandler.Billing(id, month, year));

app.Run();
