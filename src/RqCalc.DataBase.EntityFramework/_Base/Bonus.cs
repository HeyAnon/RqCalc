using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RqCalc.Domain._Base;

namespace RqCalc.DataBase.EntityFramework._Base
{
    public abstract partial class Bonus : BonusBase
    {
        public int OrderIndex { get; set; }


        [Key]
        [Column("Type_Id", Order = 1)]
        public override int TypeId 
        {
            get { return base.TypeId; }
            set { base.TypeId = value; }
        }
    }

    public abstract partial class Bonus : IBonus
    {

    }
}