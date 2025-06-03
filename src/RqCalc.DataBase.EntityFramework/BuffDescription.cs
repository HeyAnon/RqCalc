using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("BuffDescription")]
    public partial class BuffDescription : DirectoryBase
    {
        public virtual ICollection<BuffDescriptionVariable> Variables { get; set; }


        public string Template { get; set; }

        public bool IsStack { get; set; }
    }

    public partial class BuffDescription : IBuffDescription
    {
        IEnumerable<IBuffDescriptionVariable> IBuffDescription.Variables => this.Variables;
    }
}