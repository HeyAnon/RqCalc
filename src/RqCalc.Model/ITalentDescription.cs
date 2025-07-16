namespace RqCalc.Model;

public interface ITalentDescription
{
    ITextTemplate Body { get; }

    IEnumerable<ITextTemplate> Buffs { get; }
}