using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Reactive.ObservableRecurse;

public static class ExpressionExtensions
{
    public static PropertyInfo GetProperty<TSource, TProperty> (this Expression<Func<TSource, TProperty>> selector)
    {
        if (selector.Body is MemberExpression)
        {
            var memberExpression = (MemberExpression)selector.Body;

            if (memberExpression.Member is PropertyInfo)
            {
                return (PropertyInfo)memberExpression.Member;
            }
        }

        throw new InvalidOperationException ();
    }
}
