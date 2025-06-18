using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;

namespace RqCalc.DataBase.EntityFramework;

[Table("StaticImage")]
public partial class StaticImage : DirectoryBase
{
    public byte[] Data { get; set; } = null!;
}

public partial class StaticImage : IStaticImage
{

}