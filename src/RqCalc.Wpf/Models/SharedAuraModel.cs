using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Wpf.Models._Base;

namespace RqCalc.Wpf.Models
{
    public class SharedAuraModel : ActiveImageChangeModel<IAura>
    {
        public SharedAuraModel(IServiceProvider context, IAura aura)
            : base(context)
        {
            this.SelectedObject = aura;

            this.WithTalents = true;

            this.SubscribeExplicit(rule => rule.Subscribe(model => model.WithTalents, this.RecalcBonuses));
            
            this.RecalcBonuses();
        }

        public bool WithTalents
        {
            get { return this.GetValue(v => v.WithTalents); }
            set { this.SetValue(v => v.WithTalents, value); }
        }

        public IBonusContainer<IBonusBase> Bonuses
        {
            get { return this.GetValue(v => v.Bonuses); }
            private set { this.SetValue(v => v.Bonuses, value); }
        }

        private void RecalcBonuses()
        {
            var t = this.Context.AurasSharedBonuses[this.SelectedObject];

            this.Bonuses = this.WithTalents ? t.Item2 : t.Item1;
        }
    }
}