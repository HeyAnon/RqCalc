using Framework.Reactive;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.WPF
{
    public class DisplayStatModel : NotifyModelBase
    {
        public IStat Stat
        {
            get { return this.GetValue(v => v.Stat); }
            set { this.SetValue(v => v.Stat, value); }
        }

        public decimal Value
        {
            get { return this.GetValue(v => v.Value); }
            set { this.SetValue(v => v.Value, value); }
        }

        public decimal? DescriptionValue
        {
            get { return this.GetValue(v => v.DescriptionValue); }
            set { this.SetValue(v => v.DescriptionValue, value); }
        }
    }
}