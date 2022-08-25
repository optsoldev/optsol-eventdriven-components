using Optsol.EventDriven.Components.Infra.Cache.Redis.Exceptions;

namespace Optsol.EventDriven.Components.Infra.Cache.Redis.Configuration;

public abstract class BaseSettings
{
    public abstract void Validate();

    public static void ShowingException(string objectName)
    {
        throw new SettingsNullException(objectName);
    }
}
