using EnergiaClienteWebApi.Databases;
using EnergiaClienteWebApi.Handlers;
using EnergiaClienteWebApi.RequestModels;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapGet("/ping", () => "pong");
    app.MapPost("BillingTest", (int id, int month, int year) => EnergiaClienteHandler.Billing(id, month, year));
    app.MapGet("GetHabitationIdsTest", () => EnergiaClienteHandler.GetHabitationIds());
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
