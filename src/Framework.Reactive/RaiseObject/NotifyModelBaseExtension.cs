using System.Linq.Expressions;

using Framework.Core;

namespace Framework.Reactive
{
    public static class NotifyModelBaseExtensions
    {
        public static TProperty GetValue<TSource, TProperty>(this TSource source, Expression<Func<TSource, TProperty>> propertyExpr)
            where TSource : NotifyModelBase
        {
            if (source == null) throw new ArgumentNullException("source");
            if (propertyExpr == null) throw new ArgumentNullException("propertyExpr");

            return source.GetGenericValue<TProperty>(propertyExpr.ToPath());
        }

        public static void SetValue<TSource, TProperty>(this TSource source, Expression<Func<TSource, TProperty>> propertyExpr, TProperty value)
            where TSource : NotifyModelBase
        {
            if (source == null) throw new ArgumentNullException("source");
            if (propertyExpr == null) throw new ArgumentNullException("propertyExpr");

            source.SetGenericValue<TProperty>(propertyExpr.ToPath(), value);
        }
    }
}