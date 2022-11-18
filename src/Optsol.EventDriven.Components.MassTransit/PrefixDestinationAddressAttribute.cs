namespace Optsol.EventDriven.Components.MassTransit;

public sealed class PrefixDestinationAddressAttribute : Attribute
{
    public string? Prefix { get; private set; }

    public PrefixDestinationAddressAttribute(string? prefix)
    {
        Prefix = prefix;
    }
}