using Framework.Core;

namespace RqCalc.DataBase.EntityFramework._Extensions;

internal static class TypeExtensions
{
    private static readonly HashSet<Type> DictionaryTypes = new[]
    {
        typeof(IReadOnlyDictionary<,>),
        typeof(IDictionary<,>),
        typeof(Dictionary<,>)
    }.Pipe(Enumerable.ToHashSet);


    public static Type GetCollectionElementType(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.GetCollectionType() != null ? type.GetGenericArguments().Single() : null;
    }

    public static bool IsDictionaryType(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.IsGenericType && DictionaryTypes.Contains(type.GetGenericTypeDefinition());
    }
}