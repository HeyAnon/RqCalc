using System.ComponentModel.DataAnnotations.Schema;

using Framework.Core;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain.BonusType;
using RqCalc.Domain.GuildTalent;

namespace RqCalc.DataBase.EntityFramework.GuildTalent;

[Table("GuildTalentBonus")]
public partial class GuildTalentBonus : PersistentDomainObjectBase
{
    public virtual HashSet<GuildTalentBonusVariable> Variables { get; set; } = null!;

    public virtual GuildTalent Talent { get; set; } = null!;

    public virtual BonusType.BonusType Type { get; set; } = null!;


    [Column("Talent_Id")]
    public int? TalentId { get; set; }

    [Column("Type_Id")]
    public int? TypeId { get; set; }
        

    public override string ToString() =>
        $"BonusType: {this.Type.Template} | Values: "
        + this.Variables.Join(", ", bonusVar => this.Talent.Variables.Where(talentVar => talentVar.Index == bonusVar.TalentVariableIndex).OrderBy(talentVar => talentVar.Points).Select(talentVar => talentVar.Value)
                                                    .Pipe(pointVars => $"({pointVars}) [{bonusVar.Index}]"));
}

public partial class GuildTalentBonus : IGuildTalentBonus
{
    IReadOnlyCollection<IGuildTalentBonusVariable> IGuildTalentBonus.Variables => this.Variables;

    IGuildTalent IGuildTalentBonus.Talent => this.Talent;

    IBonusType Framework.Persistent.ITypeObject<IBonusType>.Type => this.Type;
}