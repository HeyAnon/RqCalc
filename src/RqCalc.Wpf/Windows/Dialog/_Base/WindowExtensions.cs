using System.Windows;

namespace RqCalc.Wpf.Windows.Dialog._Base;

public static class WindowExtensions
{
    public static void SuccessDialog(this Window window, Action successAction)
    {
        if (window.ShowDialog() == true)
        {
            successAction();
        }
    }
}