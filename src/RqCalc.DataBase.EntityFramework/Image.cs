using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain._Base;

namespace RqCalc.DataBase.EntityFramework;

[Table("Image")]
public class Image : PersistentDomainObjectBase, IImage
{
    public byte[] Data { get; set; } = null!;
}