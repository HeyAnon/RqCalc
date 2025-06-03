using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("CollectedStatistic")]
    public partial class CollectedStatistic : DirectoryBase
    {
        public int OrderIndex { get; set; }
    }

    public partial class CollectedStatistic : ICollectedStatistic
    {
    }
}