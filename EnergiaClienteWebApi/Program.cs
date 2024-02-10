using System.Security.Claims;
using EnergiaClienteWebApi.Databases;
using EnergiaClienteWebApi.Databases.Interfaces;
using EnergiaClienteWebApi.Handlers;
using EnergiaClienteWebApi.Handlers.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthentication()
    .AddBearerToken(); 
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IEnergiaClienteHandler, EnergiaClienteHandler>();
builder.Services.AddSingleton<IEnergiaClienteDatabase, EnergiaClienteDatabase>();
builder.Services.AddSingleton<IDatabaseFunctions, DatabaseFunctions>();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapGet("/ping", () => "pong");
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
