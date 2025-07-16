using RqCalc.Domain;
using RqCalc.Domain._Base;

namespace RqCalc.Application;

public interface IAuraService
{
    IReadOnlyDictionary<IAura, Tuple<IBonusContainer<IBonusBase>, IBonusContainer<IBonusBase>>> AurasSharedBonuses
    {
        get;
    }
}
