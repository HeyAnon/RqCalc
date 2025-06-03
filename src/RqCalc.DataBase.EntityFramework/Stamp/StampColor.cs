using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("StampColor")]
    public partial class StampColor : ImageDirectoryBase
    {
        public virtual Image BigImage { get; set; }


        public string Argb { get; set; }


        [Column("BigImage_Id")]
        public int? BigImageId { get; set; }
    }

    public partial class StampColor : IStampColor
    {
        IImage IStampColor.BigImage => this.BigImage;
    }
}