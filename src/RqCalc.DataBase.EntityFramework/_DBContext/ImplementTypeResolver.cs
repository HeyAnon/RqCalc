using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.Core;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    public class ImplementTypeResolver : ITypeResolver<Type>
    {
        private readonly Assembly _assembly;


        public ImplementTypeResolver(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            this._assembly = assembly;
        }


        public Type Resolve(Type projection)
        {
            if (projection == null) throw new ArgumentNullException(nameof(projection));

            return this.GetSourceTypes().Single(t => !t.IsAbstract && projection.IsAssignableFrom(t) && projection.Name.Skip("I", true) == t.Name);
        }

        public IEnumerable<Type> GetSourceTypes()
        {
            return this._assembly.GetTypes();
        }


        public static readonly ImplementTypeResolver Default = new ImplementTypeResolver(typeof(ImplementTypeResolver).Assembly);
    }
}