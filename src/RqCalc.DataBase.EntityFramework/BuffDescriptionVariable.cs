using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.Domain;

namespace RqCalc.DataBase.EntityFramework;

[Table("BuffDescriptionVariable")]
public partial class BuffDescriptionVariable
{
    public virtual BuffDescription BuffDescription { get; set; } = null!;


    public decimal Value { get; set; }
        
    public TextTemplateVariableType Type { get; set; }


    public int Index { get; set; }

    [Column("BuffDescription_Id")]
    public int BuffDescriptionId { get; set; }
}


public partial class BuffDescriptionVariable : IBuffDescriptionVariable
{
    IBuffDescription IBuffDescriptionVariable.BuffDescription => this.BuffDescription;
}