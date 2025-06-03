using System.ComponentModel.DataAnnotations;

using RqCalc.Domain._Extensions;
using RqCalc.Domain.Persistent;
using RqCalc.Domain.Persistent.Talent;

namespace RqCalc.Domain.Model;

public class TalentBuildSource : ITalentBuildSource, IEquatable<TalentBuildSource>
{
    [Required]
    public IClass Class { get; set; }

    //[IntValueValidator(Min = 1, Max = 60)]
    public int Level { get; set; }

    public List<ITalent> Talents { get; set; } = new List<ITalent>();


    IReadOnlyList<ITalent> ITalentBuildSource.Talents => this.Talents;



    public bool Equals(TalentBuildSource other)
    {
        if (other == null)
        {
            return false;
        }
            
        return this.Class == other.Class
               && this.Level == other.Level
               && this.Talents.OrderById().SequenceEqual(other.Talents.OrderById());
    }

    public override bool Equals(object obj)
    {
        return this.Equals(obj as TalentBuildSource);
    }

    public override int GetHashCode()
    {
        return this.Talents.Count;
    }
}