using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.Domain._Base;

namespace RqCalc.DataBase.EntityFramework._Base;

public abstract partial class ImageDirectoryBase : DirectoryBase
{
    public virtual Image? Image { get; set; }

    [Column("Image_Id")]
    public int? ImageId { get; set; }
}

public abstract partial class ImageDirectoryBase : IImageDirectoryBase
{
    IImage? IImageObject.Image => this.Image;
}