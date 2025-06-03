using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("Image")]
    public class Image : PersistentDomainObjectBase, IImage
    {
        public byte[] Data { get; set; }
    }
}