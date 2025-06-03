using System;
using System.Windows;



namespace Anon.RQ_Calc.WPF
{
    public interface IModelContainer<T>
    {
        T Model { get; set; }
    }

    public static class WindowExtensions
    {
        public static void SucessDialog(this Window window, Action succesAction)
        {
            if (window == null) throw new ArgumentNullException(nameof(window));
            if (succesAction == null) throw new ArgumentNullException(nameof(succesAction));

            if (window.ShowDialog() == true)
            {
                succesAction();
            }
        }
    }
}