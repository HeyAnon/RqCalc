namespace RqCalc.Domain.Model;

public interface ITalentDescription
{
    ITextTemplate Body { get; }

    IEnumerable<ITextTemplate> Buffs { get; }
}