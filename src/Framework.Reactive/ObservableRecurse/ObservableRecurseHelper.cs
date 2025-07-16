using System.ComponentModel;

namespace Framework.Reactive.ObservableRecurse;

public static class ObservableRecurseHelper
{
    public static IDisposable SubscribeExplicit<TSource> (this TSource source, params Func<ObservableRule<TSource>, ILinkNode>[] subRules)
        where TSource : class, INotifyPropertyChanged =>
        new SubscribeState<TSource> (source, subRules, false);

    public static IDisposable SubscribeExplicitLock<TSource> (this TSource source, params Func<ObservableRule<TSource>, ILinkNode>[] subRules)
        where TSource : class, INotifyPropertyChanged =>
        new SubscribeState<TSource> (source, subRules, true);

    public static IDisposable SubscribeTotal (this INotifyPropertyChanged source, PropertyChangedEventHandler handler) => new SubscribeTotalState (source, handler);
}
