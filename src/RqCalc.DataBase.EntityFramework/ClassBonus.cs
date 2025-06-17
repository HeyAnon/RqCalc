using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;

namespace RqCalc.DataBase.EntityFramework
{
    [Table("ClassBonus")]
    public partial class ClassBonus : BonusBase
    {
        public virtual Class Class { get; set; }


        [Key]
        [Column("Type_Id", Order = 1)]
        public override int TypeId
        {
            get { return base.TypeId; }
            set { base.TypeId = value; }
        }

        [Key]
        [Column("Class_Id", Order = 0)]
        public int? ClassId { get; set; }
    }

    public partial class ClassBonus : IClassBonus
    {
        
    }
}