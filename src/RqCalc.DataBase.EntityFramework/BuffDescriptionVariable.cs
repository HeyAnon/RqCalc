using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RqCalc.Domain;

namespace RqCalc.DataBase.EntityFramework
{
    [Table("BuffDescriptionVariable")]
    public partial class BuffDescriptionVariable
    {
        public virtual BuffDescription BuffDescription { get; set; }


        public decimal Value { get; set; }
        
        public TextTemplateVariableType Type { get; set; }


        [Key]
        [Column(Order = 0)]
        public int Index { get; set; }

        [Key]
        [Column("BuffDescription_Id", Order = 1)]
        public int? BuffDescriptionId { get; set; }
    }


    public partial class BuffDescriptionVariable : IBuffDescriptionVariable
    {
        IBuffDescription IBuffDescriptionVariable.BuffDescription => this.BuffDescription;
    }
}