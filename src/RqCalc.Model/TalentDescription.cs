namespace RqCalc.Model;

public class TalentDescription : ITalentDescription
{
    public ITextTemplate Body { get; set; }

    public ITextTemplate[] Buffs { get; set; }


    IEnumerable<ITextTemplate> ITalentDescription.Buffs => this.Buffs;
}