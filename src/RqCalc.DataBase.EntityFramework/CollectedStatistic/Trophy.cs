using System.ComponentModel.DataAnnotations.Schema;
using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("Trophy")]
    public partial class Trophy : DirectoryBase
    {
    }

    public partial class Trophy : ITrophy
    {
    }
}