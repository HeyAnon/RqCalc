using Framework.Core;
using Framework.DataBase.TypeBuilder;

namespace Framework.DataBase._Extensions;

public class MemoryTypeCache<TPersistentDomainObjectBase>
{
    private readonly ITypeSource typeSource;

    private readonly Lock locker = new();

    private IReadOnlyDictionary<Type, Type>? implTypes;


    public MemoryTypeCache(ITypeSource typeSource)
        : this(typeSource, DefaultAnonymousTypeBuilderStorage)
    {
    }

    public MemoryTypeCache(ITypeSource typeSource, IAnonymousTypeBuilderStorage anonymousTypeBuilderStorage)
    {
        if (typeSource == null) throw new ArgumentNullException(nameof(typeSource));
        if (anonymousTypeBuilderStorage == null) throw new ArgumentNullException(nameof(anonymousTypeBuilderStorage));

        this.typeSource = typeSource;
        this.TypeBuilder = new ImplTypeBuilder<TPersistentDomainObjectBase>(anonymousTypeBuilderStorage, typeSource).WithCache();
    }


    internal readonly IAnonymousTypeBuilder<InterfaceTypeMap> TypeBuilder;


    internal IReadOnlyDictionary<Type, Type> ImplTypes
    {
        get
        {
            lock (this.locker)
            {
                return this.implTypes ??= this.typeSource.GetTypes().ToDictionary(
                    type => type,
                    type => this.TypeBuilder.GetAnonymousType(new InterfaceTypeMap(type, $"Impl{type.Name.Skip("I")}",
                        type.GetAllInterfaceProperties().ToDictionary(property => property.Name, property => property.PropertyType))));
            }
        }
    }



    public static readonly IAnonymousTypeBuilderStorage DefaultAnonymousTypeBuilderStorage = new AnonymousTypeBuilderStorage($"LoadToMemory_{typeof(TPersistentDomainObjectBase).FullName}");
}