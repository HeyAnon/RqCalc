using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    public  abstract partial class BuffElement : PersistentDomainObjectBase
    {
        public virtual BuffDescription Description { get; set; }


        public decimal? Value { get; set; }

        public int OrderIndex { get; set; }


        [Column("Description_Id")]
        public int? DescriptionId { get; set; }
    }

    public partial class BuffElement : IBuffDescriptionElement
    {
        IBuffDescription IBuffDescriptionElement.Description => this.Description;
    }
}