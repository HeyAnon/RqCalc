using System.Collections;
﻿using System.Collections.ObjectModel;
﻿using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Linq;

using Framework.Core;

namespace Framework.Reactive;

public static class ObservableExtensions
{
    public static IObservable<MergeResultEventArgs<T>> ToMergeObservable<T>(this ObservableCollection<T> source)
    {
        Func<IList, IEnumerable<T>> toCollection = items => items.Maybe(v => v.Cast<T>()).EmptyIfNull();

        return ((INotifyCollectionChanged)source).ToObservable().Select(arg => new MergeResultEventArgs<T>(toCollection(arg.OldItems).GetMergeResult(toCollection(arg.NewItems))));
    }


    public static IObservable<PropertyChangedEventArgs> ToObservable(this INotifyPropertyChanged source) =>
        Observable.FromEvent
            <PropertyChangedEventHandler, PropertyChangedEventArgs>(
            q => (s, e) => q(e),
            q => source.PropertyChanged += q,
            q => source.PropertyChanged -= q);

    public static IObservable<PropertyChangedEventArgs> ToObservableWithout<T>(this T source, IEnumerable<Expression<Func<T, object>>> noObservablePropertyExpressions)
        where T : INotifyPropertyChanged
    {
        var excludedProperties = noObservablePropertyExpressions.Select(z => z.ToPath()).ToList();
        return Observable.FromEvent
                             <PropertyChangedEventHandler, PropertyChangedEventArgs>(
                             q => (s, e) => q(e),
                             q => source.PropertyChanged += q, q => source.PropertyChanged -= q)
                         .Where(z => !excludedProperties.Contains(z.PropertyName));

    }

    public static IObservable<PropertyChangedEventArgs> ToObservableWithout<T>(this T source, Expression<Func<T, object>> noObservablePropertyExpression, params Expression<Func<T, object>>[] noObservablePropertyExpressions)
        where T : INotifyPropertyChanged =>
        source.ToObservableWithout(noObservablePropertyExpressions.Concat([noObservablePropertyExpression]));

    public static IObservable<PropertyChangedEventArgs> ToObservable(this INotifyPropertyChanged source, string propertyPath) =>
        Observable.FromEvent
                      <PropertyChangedEventHandler, PropertyChangedEventArgs>(
                      q => (s, e) => q(e),
                      q => source.PropertyChanged += q, q => source.PropertyChanged -= q)
                  .Where(z => string.Equals(z.PropertyName, propertyPath, StringComparison.InvariantCultureIgnoreCase));

    public static IObservable<PropertyChangedEventArgs> ToObservable<T>(this T source, Expression<Func<T, object>> observablePropertyExpression)
        where T : INotifyPropertyChanged =>
        source.ToObservable(observablePropertyExpression.ToPath());

    public static IObservable<NotifyCollectionChangedEventArgs> ToObservable(this INotifyCollectionChanged source) =>
        Observable.FromEvent
            <NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
            q => (s, e) => q(e),
            q => source.CollectionChanged += q, q => source.CollectionChanged -= q);

    public static IObservable<NotifyCollectionChangedEventArgs>
        ToObservable(this INotifyCollectionChanged source, NotifyCollectionChangedAction action, params NotifyCollectionChangedAction[] actions) =>
        Observable.FromEvent
                      <NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                      q => (s, e) => q(e),
                      q => source.CollectionChanged += q, q => source.CollectionChanged -= q)
                  .Where(z => z.Action == action || actions.Contains(action));

    public static IObservable<NotifyCollectionChangedEventArgs> ToCollectionObservable(this INotifyCollectionChanged source) => source.ToObservable();

    public static IObservable<T> AsObservable<T>(this T obj) => Observable.Empty<T>().StartWith(obj);

    public static IObservable<T> SubscribeOne<T>(this IObservable<T> source, Action<T> action)
    {
        IDisposable s = null;
        s = source.Subscribe(obj =>
                             {
                                 using (s)
                                 {
                                     action(obj);
                                 }
                             });

        return source;
    }


    public static IObservable<PropertyChangedEventArgs> ObservePropertyChanged<TObject, TProperty>(this TObject subject, Expression<Func<TObject, TProperty>> property)
        where TObject : INotifyPropertyChanged
    {
        var propName = property.ToPath();
        return Observable
               .FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                   h => new(h),
                   z => subject.PropertyChanged += z, z => subject.PropertyChanged -= z)
               .Where(arg => arg.EventArgs.PropertyName == propName)
               .Select(e => e.EventArgs);
    }
}
