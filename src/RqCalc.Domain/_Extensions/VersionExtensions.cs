using RqCalc.Domain._Base;

namespace RqCalc.Domain._Extensions;

public static class VersionExtensions
{
    public static IEnumerable<T> WhereVersion<T>(this IEnumerable<T> versionObjects, IVersion version)
        where T : IVersionObject =>
        versionObjects.Where(versionObject => versionObject.Contains(version));

    public static bool Contains(this IVersionObject versionObject, IVersion version)
    {
        if (versionObject == null) throw new ArgumentNullException(nameof(versionObject));
        if (version == null) throw new ArgumentNullException(nameof(version));

        return  (versionObject.StartVersion == null || versionObject.StartVersion.Id <= version.Id)
                && (versionObject.EndVersion == null || version.Id <= versionObject.EndVersion.Id);
    }
}