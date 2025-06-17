using System.Reflection;
using Framework.Core;

namespace RqCalc.DataBase.EntityFramework._DBContext
{
    public class ImplementTypeResolver : ITypeResolver<Type>
    {
        private readonly Assembly assembly;


        public ImplementTypeResolver(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            this.assembly = assembly;
        }


        public Type Resolve(Type projection)
        {
            if (projection == null) throw new ArgumentNullException(nameof(projection));

            return this.GetSourceTypes().Single(t => !t.IsAbstract && projection.IsAssignableFrom(t) && projection.Name.Skip("I", true) == t.Name);
        }

        public IEnumerable<Type> GetSourceTypes()
        {
            return this.assembly.GetTypes();
        }


        public static readonly ImplementTypeResolver Default = new ImplementTypeResolver(typeof(ImplementTypeResolver).Assembly);
    }
}