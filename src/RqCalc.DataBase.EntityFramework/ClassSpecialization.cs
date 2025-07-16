using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;

namespace RqCalc.DataBase.EntityFramework;

[Table("ClassSpecialization")]
public partial class ClassSpecialization : PersistentDomainObjectBase
{
    public int? MaxLevel { get; set; }

    public int BonusTalentCount { get; set; }
}

public partial class ClassSpecialization :  IClassSpecialization
{

}