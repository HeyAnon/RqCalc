using Framework.Core;
using Framework.Reactive;

using RqCalc.Application;
using RqCalc.Domain;
using RqCalc.Domain._Extensions;

namespace RqCalc.Wpf.Models.Window.Dialog;

public class AurasWindowModel : NotifyModelBase
{
    public AurasWindowModel(
        IVersion lastVersion,
        IAuraService auraService,
        IClass currentClass,
        IAura? currentAura,
        int currentLevel,
        IReadOnlyDictionary<IAura, bool> startupSharedAuras)
    {
        this.Auras = currentClass.GetAuras(currentLevel, lastVersion).ToObservableCollection();

        this.SharedAuras = auraService.AurasSharedBonuses.OrderBy(aura => aura.Key.Name)
                                      .ToObservableCollection(pair => new SharedAuraModel(auraService, pair.Key)
                                                                      {
                                                                          Active = startupSharedAuras.ContainsKey(pair.Key),
                                                                          WithTalents = startupSharedAuras.GetValueOrDefault(pair.Key, true)
                                                                      });

        this.Aura = currentAura ?? this.Auras.FirstOrDefault();
    }


    public ObservableCollection<IAura> Auras { get => this.GetValue(v => v.Auras); set => this.SetValue(v => v.Auras, value); }

    public IAura? Aura { get => this.GetValue(v => v.Aura); set => this.SetValue(v => v.Aura, value); }

    public ObservableCollection<SharedAuraModel> SharedAuras
    {
        get => this.GetValue(v => v.SharedAuras);
        private set => this.SetValue(v => v.SharedAuras, value);
    }
}