using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("GuildTalent")]
    public partial class GuildTalent : ImageDirectoryBase
    {
        public virtual ICollection<GuildTalentBonus> Bonuses { get; set; }

        public virtual ICollection<GuildTalentVariable> Variables { get; set; }
        

        public virtual GuildTalentBranch Branch { get; set; }

        public virtual Image GrayImage { get; set; }



        public string Description { get; set; }


        public bool Active { get; set; }

        public int OrderIndex { get; set; }


        [Column("GrayImage_Id")]
        public int? GrayImageId { get; set; }

        [Column("Branch_Id")]
        public int? BranchId { get; set; }
    }

    public partial class GuildTalent : IGuildTalent
    {
        IEnumerable<IGuildTalentBonus> IBonusContainer<IGuildTalentBonus>.Bonuses => this.Bonuses;

        IGuildTalentBranch IGuildTalent.Branch => this.Branch;

        IEnumerable<IGuildTalentVariable> IGuildTalent.Variables => this.Variables;
        
        IImage IGuildTalent.GrayImage => this.GrayImage;
    }
}