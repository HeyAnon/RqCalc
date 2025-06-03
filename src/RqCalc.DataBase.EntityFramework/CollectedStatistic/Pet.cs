using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("Pet")]
    public partial class Pet : ImageDirectoryBase
    {
    }

    public partial class Pet : IPet
    {
    }
}