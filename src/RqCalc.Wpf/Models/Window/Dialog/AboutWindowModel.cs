using Framework.Reactive;

using RqCalc.Domain;

namespace RqCalc.Wpf.Models.Window.Dialog;

public class AboutWindowModel(WpfApplicationSettings wpfApplicationSettings, IVersion lastVersion) : NotifyModelBase
{
    public Version Version => wpfApplicationSettings.Version;

    public int SerializerVersion => lastVersion.Id;
}