using System.ComponentModel.DataAnnotations;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    public abstract partial class PersistentDomainObjectBase
    {
        [Key]
        public int Id { get; set; }
    }

    public abstract partial class PersistentDomainObjectBase : IPersistentIdentityDomainObjectBase
    {
        
    }
}