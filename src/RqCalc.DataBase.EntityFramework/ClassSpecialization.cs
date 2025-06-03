using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("ClassSpecialization")]
    public partial class ClassSpecialization : PersistentDomainObjectBase
    {
        public int? MaxLevel { get; set; }

        public int BonusTalentCount { get; set; }
    }

    public partial class ClassSpecialization :  IClassSpecialization
    {

    }
}