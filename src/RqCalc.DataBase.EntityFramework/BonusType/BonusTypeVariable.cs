using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("BonusTypeVariable")]
    public partial class BonusTypeVariable : PersistentDomainObjectBase
    {
        public virtual Stat MultiplicityStat { get; set; }
        
        public virtual BonusType BonusType { get; set; }

        public virtual Formula MulFormula { get; set; }


        public int? MultiplicityValue { get; set; }

        public bool HasSign { get; set; }

        public int Index { get; set; }

        


        [Column("BonusType_Id")]
        public int? BonusTypeId { get; set; }

        [Column("MultiplicityStat_Id")]
        public int? MultiplicityStatId { get; set; }

        [Column("MulFormula_Id")]
        public int? MulFormulaId { get; set; }
    }

    public partial class BonusTypeVariable : IBonusTypeVariable
    {
        IStat IBonusTypeVariable.MultiplicityStat => this.MultiplicityStat;

        IBonusType IBonusTypeVariable.BonusType => this.BonusType;

        IFormula IBonusTypeVariable.MulFormula => this.MulFormula;
    }
}