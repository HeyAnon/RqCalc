using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("Talent")]
    public partial class Talent : ImageDirectoryBase
    {
        public virtual ICollection<TalentBonus> Bonuses { get; set; }

        public virtual ICollection<TalentVariable> Variables { get; set; }

        public virtual ICollection<TalentBuffDescription> BuffDescriptions { get; set; }


        public virtual TalentBranch Branch { get; set; }

        public virtual Image GrayImage { get; set; }



        public string Description { get; set; }

        public int VIndex { get; set; }

        public int HIndex { get; set; }

        public bool Active { get; set; }

        public string PassiveDescription { get; set; }

        public string EquipmentCondition { get; set; }


        [Column("GrayImage_Id")]
        public int? GrayImageId { get; set; }

        [Column("Branch_Id")]
        public int? BranchId { get; set; }
    }

    public partial class Talent : ITalent
    {
        IEnumerable<ITalentBonus> IBonusContainer<ITalentBonus>.Bonuses => this.Bonuses;

        ITalentBranch ITalent.Branch => this.Branch;

        IEnumerable<ITalentVariable> ITalent.Variables => this.Variables;

        IEnumerable<ITalentBuffDescription> ITalent.BuffDescriptions => this.BuffDescriptions;

        IImage ITalent.GrayImage => this.GrayImage;
    }
}