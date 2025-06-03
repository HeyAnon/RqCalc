using Framework.Core;

namespace Framework.DataBase._Extensions;

internal static class TypeExtensions
{
    public static bool IsAssignableToInterface(this Type type, Type interfaceType)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (interfaceType == null) throw new ArgumentNullException(nameof(interfaceType));

        return type.IsInterface && type.GetAllInterfaces().Contains(interfaceType);
    }
}