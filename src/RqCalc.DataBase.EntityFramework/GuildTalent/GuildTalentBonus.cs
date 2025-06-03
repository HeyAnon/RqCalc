using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using Framework.Core;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("GuildTalentBonus")]
    public partial class GuildTalentBonus : PersistentDomainObjectBase
    {
        public virtual ICollection<GuildTalentBonusVariable> Variables { get; set; }

        public virtual GuildTalent Talent { get; set; }

        public virtual BonusType Type { get; set; }


        [Column("Talent_Id")]
        public int? TalentId { get; set; }

        [Column("Type_Id")]
        public int? TypeId { get; set; }
        

        public override string ToString()
        {
            return $"BonusType: {this.Type.Template} | Values: "
                 + this.Variables.Join(", ", bonusVar => this.Talent.Variables.Where(talentVar => talentVar.Index == bonusVar.TalentVariableIndex).OrderBy(talentVar => talentVar.Points).Select(talentVar => talentVar.Value)
                       .Pipe(pointVars => $"({pointVars}) [{bonusVar.Index}]"));
        }
    }

    public partial class GuildTalentBonus : IGuildTalentBonus
    {
        IEnumerable<IGuildTalentBonusVariable> IGuildTalentBonus.Variables => this.Variables;

        IGuildTalent IGuildTalentBonus.Talent => this.Talent;

        IBonusType Framework.Persistent.ITypeObject<IBonusType>.Type => this.Type;
    }
}