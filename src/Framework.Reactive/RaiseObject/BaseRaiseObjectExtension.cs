using System.ComponentModel;
using System.Linq.Expressions;

using Framework.Core;

namespace Framework.Reactive;

public static class BaseRaiseObjectExtensions
{
    public static void RaisePropertyChanged<TSource, TProperty> (this TSource source, Expression<Func<TSource, TProperty>> propertyExpression)
        where TSource : class, IBaseRaiseObject
    {
        if (source == null)
        {
            throw new ArgumentNullException ("source");
        }

        if (propertyExpression == null)
        {
            throw new ArgumentNullException ("propertyExpression");
        }


        source.PropertyChanged.Maybe (
            propertyChanged => propertyChanged (source, new(propertyExpression.ToPath ())));
    }

    public static void RaisePropertyChanged<TSource>(this TSource source, string propertyName)
        where TSource : class, IBaseRaiseObject
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        if (string.IsNullOrWhiteSpace(propertyName))
        {
            throw new ArgumentNullException("propertyName");
        }


        source.PropertyChanged.Maybe(
            propertyChanged => propertyChanged(source, new(propertyName)));
    }



    public static IDisposable DelegateRaiseSubscribe<TSender, TSenderProperty, TReceiver, TReceiverProperty> (

        this TSender   sender,   Expression<Func<TSender,  TSenderProperty>>    senderPropertyExpression,
        TReceiver receiver, Expression<Func<TReceiver, TReceiverProperty>> receiverPropertyExpression)

        where TSender : class, INotifyPropertyChanged
        where TReceiver : class, IBaseRaiseObject
    {
        if (sender == null)
        {
            throw new ArgumentNullException ("sender");
        }

        if (senderPropertyExpression == null)
        {
            throw new ArgumentNullException ("senderPropertyExpression");
        }

        if (receiver == null)
        {
            throw new ArgumentNullException ("receiver");
        }

        if (receiverPropertyExpression == null)
        {
            throw new ArgumentNullException ("receiverPropertyExpression");
        }


        return sender.GetPropertyChange (senderPropertyExpression).Subscribe (_ =>
                                                                                  receiver.RaisePropertyChanged (receiverPropertyExpression));
    }
}
