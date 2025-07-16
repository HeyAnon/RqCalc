using Framework.Core;
using RqCalc.Domain._Base;

namespace RqCalc.Infrastructure;

public static class DomainTypeResolver
{
    public static readonly ITypeResolver<string> Default = TypeResolverHelper.Create(GetDefaultTypeSource(), TypeSearchMode.Name).WithCache().WithLock();

    private static ITypeSource GetDefaultTypeSource()
    {
        var types = typeof(IPersistentDomainObjectBase).Assembly.GetTypes()
            .Where(type => type.IsInterface && typeof(IPersistentDomainObjectBase).IsAssignableFrom(type) && !type.IsGenericTypeDefinition)
            .Except([
                typeof(IPersistentDomainObjectBase), typeof(IPersistentIdentityDomainObjectBase), typeof(IDirectoryBase), typeof(IBonus), typeof(IBonusBase),
                typeof(IImageDirectoryBase)
            ]).ToList();

        return new TypeSource(types);
    }
}