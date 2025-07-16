using Framework.Persistent;

namespace RqCalc.Application._Extensions;

public static class EnumerableExtensions
{
    public static T GetByName<T>(this IEnumerable<T> source, string name)
        where T : IVisualIdentityObject =>
        source.Single(obj => obj.Name == name);

    public static T GetById<T>(this IEnumerable<T> source, int id)
        where T : IIdentityObject<int> =>
        source.Single(obj => obj.Id == id);
}