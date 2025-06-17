using System.ComponentModel.DataAnnotations.Schema;
using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain.Formula;

namespace RqCalc.DataBase.EntityFramework.Formula
{
    [Table("Formula")]
    public partial class Formula : PersistentDomainObjectBase
    {
        public virtual HashSet<FormulaVariable> Variables { get; set; }
        

        public string Value { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }

        public override string ToString()
        {
            return this.Value;
        }
    }

    public partial class Formula : IFormula
    {
        public Formula()
        {

        }


        IEnumerable<IFormulaVariable> IFormula.Variables => this.Variables;
    }
}