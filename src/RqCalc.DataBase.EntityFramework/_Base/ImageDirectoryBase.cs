using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    public abstract partial class ImageDirectoryBase : DirectoryBase
    {
        public virtual Image Image { get; set; }

        [Column("Image_Id")]
        public int? ImageId { get; set; }
    }

    public abstract partial class ImageDirectoryBase : IImageDirectoryBase
    {
        IImage IImageObject.Image => this.Image;
    }
}