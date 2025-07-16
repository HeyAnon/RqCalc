using Framework.Core;

namespace Framework.DataBase._Extensions;

internal static class TypeExtensions
{
    public static bool IsAssignableToInterface(this Type type, Type interfaceType) => type.IsInterface && type.GetAllInterfaces().Contains(interfaceType);
}