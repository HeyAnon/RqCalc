using Framework.Reactive;
using RqCalc.Domain;

namespace RqCalc.Wpf.Models;

public class DisplayStatModel : NotifyModelBase
{
    public IStat Stat
    {
        get => this.GetValue(v => v.Stat);
        set => this.SetValue(v => v.Stat, value);
    }

    public decimal Value
    {
        get => this.GetValue(v => v.Value);
        set => this.SetValue(v => v.Value, value);
    }

    public decimal? DescriptionValue
    {
        get => this.GetValue(v => v.DescriptionValue);
        set => this.SetValue(v => v.DescriptionValue, value);
    }
}