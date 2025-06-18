using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;

namespace RqCalc.DataBase.EntityFramework;

[Table("BuffDescription")]
public partial class BuffDescription : DirectoryBase
{
    public virtual HashSet<BuffDescriptionVariable> Variables { get; set; } = null!;


    public string Template { get; set; } = null!;

    public bool IsStack { get; set; }
}

public partial class BuffDescription : IBuffDescription
{
    IReadOnlyCollection<IBuffDescriptionVariable> IBuffDescription.Variables => this.Variables;
}