using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain.CollectedStatistic;

namespace RqCalc.DataBase.EntityFramework.CollectedStatistic;

[Table("Trophy")]
public partial class Trophy : DirectoryBase
{
}

public partial class Trophy : ITrophy
{
}