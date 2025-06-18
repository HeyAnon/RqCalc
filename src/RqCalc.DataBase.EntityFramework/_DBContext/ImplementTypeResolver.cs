using System.Reflection;

using Framework.Core;

namespace RqCalc.DataBase.EntityFramework._DBContext;

public class ImplementTypeResolver(Assembly assembly) : ITypeResolver<Type>
{
    public const string Key = "Implement";

    public Type Resolve(Type projection)
    {
        return this.GetTypes().Single(t => !t.IsAbstract && projection.IsAssignableFrom(t) && projection.Name.Skip("I", true) == t.Name);
    }

    public IEnumerable<Type> GetTypes()
    {
        return assembly.GetTypes();
    }


    public static ImplementTypeResolver Default { get; } = new(typeof(ImplementTypeResolver).Assembly);
}