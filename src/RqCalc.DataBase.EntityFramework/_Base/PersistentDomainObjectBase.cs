using System.ComponentModel.DataAnnotations;

using RqCalc.Domain._Base;

namespace RqCalc.DataBase.EntityFramework._Base;

public abstract partial class PersistentDomainObjectBase
{
    [Key]
    public int Id { get; set; }
}

public abstract partial class PersistentDomainObjectBase : IPersistentIdentityDomainObjectBase
{
        
}