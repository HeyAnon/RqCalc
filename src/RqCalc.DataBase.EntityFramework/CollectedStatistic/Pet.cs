using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain.CollectedStatistic;

namespace RqCalc.DataBase.EntityFramework.CollectedStatistic;

[Table("Pet")]
public partial class Pet : ImageDirectoryBase
{
}

public partial class Pet : IPet
{
}