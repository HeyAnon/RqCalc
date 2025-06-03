using System;
using System.Windows.Threading;

namespace Anon.RQ_Calc.WPF
{
    public static class DispatcherObjectExtensions
    {
        public static void Invoke<T>(this T dispatcherObject, Action<T> action)
            where T : DispatcherObject
        {
            dispatcherObject.Dispatcher.Invoke(action, dispatcherObject);
        }

        public static void Invoke<T>(this T dispatcherObject, Action action)
            where T : DispatcherObject
        {
            dispatcherObject.Dispatcher.Invoke(action);
        }
    }
}