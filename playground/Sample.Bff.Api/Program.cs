using System.Reflection;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Optsol.EventDriven.Components.MassTransit;
using Sample.Bff.Api.Hubs;
using Sample.Flight.Contracts;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Addicionar
builder.Host.UseSerilog((host, log) =>
{
    if (host.HostingEnvironment.IsProduction())
        log.MinimumLevel.Information();
    else
        log.MinimumLevel.Debug();

    log.WriteTo.Console();
});

builder.Configuration
    .AddUserSecrets(Assembly.GetExecutingAssembly(), true);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientPermission", policy =>
    {
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("https://localhost:3000") // hard code pra rodar o sample
            .AllowCredentials();
    });
});

//Colocar numa extension.
builder.Services.TryAddSingleton(new KebabCaseEndpointNameFormatter("Teste", false));

//Colocar numa extension.
builder.Services.RegisterMassTransit(builder.Configuration, bus =>
{
    bus.AddRequestClient<RequestClientSampleQuery>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("ClientPermission");

app.UseAuthorization();

app.MapControllers();

app.MapHub<BookingNotificationHub>("/hubs/bookingNotification");

app.Run();
