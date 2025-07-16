using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain.CollectedStatistic;

namespace RqCalc.DataBase.EntityFramework.CollectedStatistic;

[Table("CollectedStatistic")]
public partial class CollectedStatistic : DirectoryBase
{
    public int OrderIndex { get; set; }
}

public partial class CollectedStatistic : ICollectedStatistic
{
}