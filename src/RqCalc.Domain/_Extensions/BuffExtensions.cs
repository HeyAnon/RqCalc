using RqCalc.Domain.Talent;

namespace RqCalc.Domain._Extensions;

public static class BuffExtensions
{
    public static bool IsAllowed(this IBuff buff, int level, IEnumerable<ITalent> talents)
    {
        if (buff == null) throw new ArgumentNullException(nameof(buff));

        return buff.Level <= level && (buff.TalentCondition == null || talents.Contains(buff.TalentCondition));
    }

    public static bool IsShared(this IBuff buff)
    {
        if (buff == null) throw new ArgumentNullException(nameof(buff));

        return buff.Class == null && buff.Card == null && buff.Stamp == null;
    }
}