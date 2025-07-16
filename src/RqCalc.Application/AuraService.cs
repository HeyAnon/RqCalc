using Framework.DataBase;

using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain._Extensions;
using RqCalc.Domain.VirtualBonus;

namespace RqCalc.Application;

public class AuraService : IAuraService
{
    public AuraService(IDataSource<IPersistentDomainObjectBase> dataSource, IVersion version)
    {
        var aurasRequest =

            from aura in dataSource.GetFullList<IAura>()

            where aura.Contains(version)

            let bc1 = new VirtualBonusBaseContainer(aura.GetBonuses(version, true, false))

            let bc2 = new VirtualBonusBaseContainer(aura.GetBonuses(version, true, true))

            where bc1.Bonuses.Any() || bc2.Bonuses.Any()

            select new
                   {
                       Key = aura,

                       Value = new Tuple<IBonusContainer<IBonusBase>, IBonusContainer<IBonusBase>>(bc1, bc2)
                   };

        this.AurasSharedBonuses = aurasRequest.ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    public IReadOnlyDictionary<IAura, Tuple<IBonusContainer<IBonusBase>, IBonusContainer<IBonusBase>>> AurasSharedBonuses { get; }
}
