using System.Reactive;
using System.Reactive.Linq;
using Framework.Core;

namespace Framework.Reactive;

using System;
using System.ComponentModel;
using System.Linq.Expressions;


public static class NotifyPropertyChangeReactiveExtensions
{
    private static readonly LambdaCompileCache LambdaCompileCache = new();

    // Returns the values of property (an Expression) as they change,
    // starting with the current value
    public static IObservable<TValue> GetPropertyValues<TSource, TValue>(
        this TSource source, Expression<Func<TSource, TValue>> property)
        where TSource : INotifyPropertyChanged
    {
        MemberExpression memberExpression = property.Body as MemberExpression;

        if (memberExpression == null)
        {
            throw new ArgumentException(
                "property must directly access a property of the source");
        }

        string propertyName = memberExpression.Member.Name;

        Func<TSource, TValue> accessor = property.Compile(LambdaCompileCache);

        return source.GetPropertyChangedEvents()
                     .Where(x => x.EventArgs.PropertyName == propertyName)
                     .Select(x => accessor(source))
                     .StartWith(accessor(source));
    }

    public static IObservable<EventPattern<PropertyChangedEventArgs>> GetPropertyChange<TSource, TValue>(
        this TSource source, Expression<Func<TSource, TValue>> property)
        where TSource : INotifyPropertyChanged
    {
        MemberExpression memberExpression = property.Body as MemberExpression;

        if (memberExpression == null)
        {
            throw new ArgumentException(
                "property must directly access a property of the source");
        }

        string propertyName = memberExpression.Member.Name;
        return source.GetPropertyChangedEvents()
                     .Where(x => x.EventArgs.PropertyName == propertyName);
    }
    public static IObservable<PropertyChangedEventArgs> GetPropertyChange(
        this INotifyPropertyChanged source, string propertyPath) =>
        source.ToObservable().Where(x => x.PropertyName == propertyPath);

    // This is a wrapper around FromEvent(PropertyChanged)
    public static IObservable<EventPattern<PropertyChangedEventArgs>>
        GetPropertyChangedEvents(this INotifyPropertyChanged source) =>
        Observable.FromEventPattern<PropertyChangedEventHandler,
            PropertyChangedEventArgs>(
            h => source.PropertyChanged += h,
            h => source.PropertyChanged -= h);
}
