using RqCalc.Domain;

namespace RqCalc.Application;

public interface IAuraServiceFactory
{
    IAuraService Create(IVersion version);
}
