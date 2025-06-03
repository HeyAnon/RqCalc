using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("FormulaVariable")]
    public partial class FormulaVariable : PersistentDomainObjectBase
    {
        public virtual Formula Formula { get; set; }

        public virtual Stat TypeStat { get; set; }
        


        public int Index { get; set; }
        
        public FormulaVariableType Type { get; set; }


        [Column("Formula_Id")]
        public int? FormulaId { get; set; }


        [Column("TypeStat_Id")]
        public int? TypeStatId { get; set; }
    }

    public partial class FormulaVariable : IFormulaVariable
    {
        IFormula IFormulaVariable.Formula => this.Formula;

        IStat IFormulaVariable.TypeStat => this.TypeStat;
    }
}