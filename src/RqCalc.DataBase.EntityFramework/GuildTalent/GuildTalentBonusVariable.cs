using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("GuildTalentBonusVariable")]
    public partial class GuildTalentBonusVariable : PersistentDomainObjectBase
    {
        public virtual GuildTalentBonus GuildTalentBonus { get; set; }

        
        public int Index { get; set; }

        public int TalentVariableIndex { get; set; }


        [Column("GuildTalentBonus_Id")]
        public int? GuildTalentBonusId { get; set; }
    }

    public partial class GuildTalentBonusVariable : IGuildTalentBonusVariable
    {
        IGuildTalentBonus IGuildTalentBonusVariable.GuildTalentBonus => this.GuildTalentBonus;
    }
}