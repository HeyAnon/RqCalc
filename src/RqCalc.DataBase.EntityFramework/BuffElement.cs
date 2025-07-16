using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;
using RqCalc.Domain._Base;

namespace RqCalc.DataBase.EntityFramework;

public  abstract partial class BuffElement : PersistentDomainObjectBase
{
    public virtual BuffDescription Description { get; set; } = null!;


    public decimal? Value { get; set; }

    public int OrderIndex { get; set; }


    [Column("Description_Id")]
    public int? DescriptionId { get; set; }
}

public partial class BuffElement : IBuffDescriptionElement
{
    IBuffDescription IBuffDescriptionElement.Description => this.Description;
}