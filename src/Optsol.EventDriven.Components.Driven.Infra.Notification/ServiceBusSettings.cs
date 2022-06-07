namespace Optsol.EventDriven.Components.Driven.Infra.Notification;

public class ServiceBusSettings
{
    public string? ConnectionString { get; set; }

    public string? UserName { get; set; }
    public string? Password { get; set; }
    public int? Port { get; set; }
    public string? HostName { get; set; }
    public string? Exchange { get; set; }
}