using System.ComponentModel.DataAnnotations;
using RqCalc.Domain;
using RqCalc.Domain._Extensions;
using RqCalc.Domain.Talent;

namespace RqCalc.Model;

public class TalentBuildSource : ITalentBuildSource, IEquatable<TalentBuildSource>
{
    [Required] public IClass Class { get; set; } = null!;

    //[IntValueValidator(Min = 1, Max = 60)]
    public int Level { get; set; }

    public List<ITalent> Talents { get; set; } = [];


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

    public override bool Equals(object obj) => this.Equals(obj as TalentBuildSource);

    public override int GetHashCode() => this.Talents.Count;
}