using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    public abstract partial class DirectoryBase : PersistentDomainObjectBase
    {
        public string Name { get; set; }


        public override string ToString()
        {
            return this.Name;
        }
    }

    public abstract partial class DirectoryBase : IDirectoryBase
    {

    }
}