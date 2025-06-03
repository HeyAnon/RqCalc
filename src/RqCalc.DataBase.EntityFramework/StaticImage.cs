using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("StaticImage")]
    public partial class StaticImage : DirectoryBase
    {
        public byte[] Data { get; set; }
    }

    public partial class StaticImage : IStaticImage
    {

    }
}