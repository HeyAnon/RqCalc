namespace RqCalc.Domain._Extensions;

public static class WeaponInfoExtensions
{
    public static WeaponInfo? Average(this IEnumerable<WeaponInfo> weaponInfos)
    {
        if (weaponInfos == null) throw new ArgumentNullException(nameof(weaponInfos));

        var cache = weaponInfos.ToArray();

        if (cache.Any())
        {
            var sum = cache.Aggregate((state, info) => new WeaponInfo(-1, state.Attack + info.Attack, state.AttackSpeed + info.AttackSpeed));

            return new WeaponInfo(-1, sum.Attack / cache.Length, sum.AttackSpeed);
        }
        else
        {
            return null;
        }
    }
}