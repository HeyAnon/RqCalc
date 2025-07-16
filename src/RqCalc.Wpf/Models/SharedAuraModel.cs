using Framework.Reactive;
using Framework.Reactive.ObservableRecurse;

using RqCalc.Application;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Wpf.Models._Base;

namespace RqCalc.Wpf.Models;

public class SharedAuraModel : ActiveImageChangeModel<IAura>
{
    private readonly IAuraService auraService;

    public SharedAuraModel(IAuraService auraService, IAura aura)
    {
        this.auraService = auraService;
        this.SelectedObject = aura;

        this.WithTalents = true;

        this.SubscribeExplicit(rule => rule.Subscribe(model => model.WithTalents, this.RecalcBonuses));

        this.RecalcBonuses();
    }

    public bool WithTalents
    {
        get => this.GetValue(v => v.WithTalents);
        set => this.SetValue(v => v.WithTalents, value);
    }

    public IBonusContainer<IBonusBase> Bonuses
    {
        get => this.GetValue(v => v.Bonuses);
        private set => this.SetValue(v => v.Bonuses, value);
    }

    private void RecalcBonuses()
    {
        var t = this.auraService.AurasSharedBonuses[this.SelectedObject!];

        this.Bonuses = this.WithTalents ? t.Item2 : t.Item1;
    }
}