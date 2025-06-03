using System;
using System.Linq;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("Formula")]
    public partial class Formula : PersistentDomainObjectBase
    {
        public virtual ICollection<FormulaVariable> Variables { get; set; }
        

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