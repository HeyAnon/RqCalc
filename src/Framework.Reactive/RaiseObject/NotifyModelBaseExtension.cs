using System.Linq.Expressions;

using Framework.Core;

namespace Framework.Reactive;

public static class NotifyModelBaseExtensions
{
    public static TProperty GetValue<TSource, TProperty>(this TSource source, Expression<Func<TSource, TProperty>> propertyExpr)
        where TSource : NotifyModelBase =>
        source.GetGenericValue<TProperty>(propertyExpr.ToPath());

    public static void SetValue<TSource, TProperty>(this TSource source, Expression<Func<TSource, TProperty>> propertyExpr, TProperty value)
        where TSource : NotifyModelBase =>
        source.SetGenericValue<TProperty>(propertyExpr.ToPath(), value);
}
