using System.Reflection.Emit;

using Framework.Core;
using Framework.DataBase._Extensions;

namespace Framework.DataBase.TypeBuilder;

internal class ImplTypeBuilder<TPersistentDomainObjectBase>(IAnonymousTypeBuilderStorage storage, ITypeSource typeSource)
    : AnonymousTypeByPropertyBuilder<InterfaceTypeMap, TypeMapMember>(storage)
{
    protected override System.Reflection.Emit.TypeBuilder DefineType(InterfaceTypeMap typeMap)
    {
        var typeBuilder = base.DefineType(typeMap);

        typeBuilder.AddInterfaceImplementation(typeMap.SourceType);

        return typeBuilder;
    }

    protected override PropertyBuilder ImplementMember(System.Reflection.Emit.TypeBuilder typeBuilder, TypeMapMember member)
    {
        this.CheckType(member.Type);

        return base.ImplementMember(typeBuilder, member);
    }

    private void CheckType(Type type)
    {
        var actualType = type.GetCollectionElementType() ?? type;

        if (actualType.IsInterface)
        {
            if (actualType.IsAssignableToInterface(typeof(TPersistentDomainObjectBase)))
            {
                if (!typeSource.GetTypes().Contains(actualType))
                {
                    throw new Exception($"Missed Impl Type for {actualType}");
                }
            }
            else if (type.IsInterfaceImplementation(typeof(IReadOnlyDictionary<,>)))
            {
                var args = type.GetInterfaceImplementationArguments(typeof(IReadOnlyDictionary<,>));

                args.Foreach(this.CheckType);
            }
            else
            {
                throw new Exception($"Missed base interfaceType");
            }
        }
    }
}