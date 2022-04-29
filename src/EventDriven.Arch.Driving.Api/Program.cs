using EventDriven.Arch.Application;
using EventDriven.Arch.Driven.Infra.Data;
using MediatR;
using Optsol.EventDriven.Components.Driven.Infra.Data.Notification;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(typeof(ApplicationMediatREntryPoint).Assembly);
builder.Services.RegisterNotification();
builder.Services.RegisterInfraData();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.Run();
