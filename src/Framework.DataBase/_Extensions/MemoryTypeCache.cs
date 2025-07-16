using Framework.Core;
using Framework.DataBase.TypeBuilder;

namespace Framework.DataBase._Extensions;

public class MemoryTypeCache<TPersistentDomainObjectBase>(ITypeSource typeSource, IAnonymousTypeBuilderStorage anonymousTypeBuilderStorage)
{
    private readonly Lock locker = new();

    private IReadOnlyDictionary<Type, Type>? implTypes;


    public MemoryTypeCache(ITypeSource typeSource)
        : this(typeSource, DefaultAnonymousTypeBuilderStorage)
    {
    }

    internal readonly IAnonymousTypeBuilder<InterfaceTypeMap> TypeBuilder = new ImplTypeBuilder<TPersistentDomainObjectBase>(anonymousTypeBuilderStorage, typeSource).WithCache();


    internal IReadOnlyDictionary<Type, Type> ImplTypes
    {
        get
        {
            lock (this.locker)
            {
                return this.implTypes ??= typeSource.GetTypes().ToDictionary(
                    type => type,
                    type => this.TypeBuilder.GetAnonymousType(new InterfaceTypeMap(type, $"Impl{type.Name.Skip("I")}",
                                                                                   type.GetAllInterfaceProperties().ToDictionary(property => property.Name, property => property.PropertyType))));
            }
        }
    }



    public static readonly IAnonymousTypeBuilderStorage DefaultAnonymousTypeBuilderStorage = new AnonymousTypeBuilderStorage($"LoadToMemory_{typeof(TPersistentDomainObjectBase).FullName}");
}
