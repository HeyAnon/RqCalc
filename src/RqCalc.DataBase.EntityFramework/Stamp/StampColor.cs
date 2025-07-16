using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain._Base;
using RqCalc.Domain.Stamp;

namespace RqCalc.DataBase.EntityFramework.Stamp;

[Table("StampColor")]
public partial class StampColor : ImageDirectoryBase
{
    public virtual Image BigImage { get; set; } = null!;


    public string Argb { get; set; } = null!;


    [Column("BigImage_Id")]
    public int? BigImageId { get; set; }
}

public partial class StampColor : IStampColor
{
    IImage IStampColor.BigImage => this.BigImage;
}