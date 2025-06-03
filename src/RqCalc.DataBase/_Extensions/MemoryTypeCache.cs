using Anon.RQ_Calc.Domain;

using Framework.DataBase;

namespace Anon.RQ_Calc.DataBase
{
    public static class MemoryTypeCache
    {
        public static readonly MemoryTypeCache<IPersistentDomainObjectBase> Default = new MemoryTypeCache<IPersistentDomainObjectBase>(DomainTypeResolver.Default);
    }
}