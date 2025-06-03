
using Framework.Reactive;
using Framework.Reactive.ObservableRecurse;

using Anon.RQ_Calc.Domain;
using Anon.RQ_Calc.Logic;


namespace Anon.RQ_Calc.WPF
{
    public class SharedAuraModel : ActiveImageChangeModel<IAura>
    {
        public SharedAuraModel(IApplicationContext context, IAura aura)
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