using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("GuildTalentBranch")]
    public partial class GuildTalentBranch : DirectoryBase
    {
        public virtual ICollection<GuildTalent> Talents { get; set; }

        public int MaxPoints { get; set; }
    }

    public partial class GuildTalentBranch : IGuildTalentBranch
    {
        IEnumerable<IGuildTalent> IGuildTalentBranch.Talents => this.Talents;
    }
}