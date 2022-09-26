using System.Runtime.Serialization;

namespace Optsol.EventDriven.Components.Infra.Cache.Redis.Exceptions;

public class SettingsNullException : Exception
{
    public SettingsNullException(string objectName)
        : base($"O Atributo {objectName} está nulo")
    { }

    protected SettingsNullException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    { }
}
