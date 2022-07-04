using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sample.Bff.Api.Hubs;
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
            .WithOrigins("http://localhost:3000") // hard code pra rodar o sample
            .AllowCredentials();
    });
});

//Colocar numa extension.
builder.Services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);

//Colocar numa extension.
builder.Services.AddMassTransit(bus =>
{
    //bus.AddConsumer<>
    bus.UsingRabbitMq();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("ClientPermission");

app.MapControllers();

app.MapHub<BookingNotificationHub>("/hubs/bookingNotification");

app.Run();
