using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain.GuildTalent;

namespace RqCalc.DataBase.EntityFramework.GuildTalent;

[Table("GuildTalentBonusVariable")]
public partial class GuildTalentBonusVariable : PersistentDomainObjectBase
{
    public virtual GuildTalentBonus GuildTalentBonus { get; set; } = null!;

        
    public int Index { get; set; }

    public int TalentVariableIndex { get; set; }


    [Column("GuildTalentBonus_Id")]
    public int? GuildTalentBonusId { get; set; }
}

public partial class GuildTalentBonusVariable : IGuildTalentBonusVariable
{
    IGuildTalentBonus IGuildTalentBonusVariable.GuildTalentBonus => this.GuildTalentBonus;
}