using RqCalc.Domain._Base;

namespace RqCalc.DataBase.EntityFramework._Base;

public abstract partial class DirectoryBase : PersistentDomainObjectBase
{
    public string Name { get; set; } = null!;


    public override string ToString()
    {
        return this.Name;
    }
}

public abstract partial class DirectoryBase : IDirectoryBase
{

}