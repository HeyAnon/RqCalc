using Framework.Reactive;

using RqCalc.Domain;

namespace RqCalc.Wpf.Models.Window.Dialog;

public class AboutWindowModel(Version version, IVersion serializerVersion) : NotifyModelBase
{
    public Version Version { get; } = version;

    public int SerializerVersion => serializerVersion.Id;
}