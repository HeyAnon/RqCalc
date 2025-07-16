using System.Windows;
using System.Windows.Data;
using Framework.Core;

namespace RqCalc.Wpf._Extensions;

public static class FrameworkElementExtensions
{
    public static void SetBinding(this FrameworkElement source, DependencyProperty dependencyProperty, System.Linq.Expressions.Expression bindingExpression, IValueConverter converter = null)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (dependencyProperty == null) throw new ArgumentNullException(nameof(dependencyProperty));
        if (bindingExpression == null) throw new ArgumentNullException(nameof(bindingExpression));

        source.SetBinding(dependencyProperty, new Binding(bindingExpression.ToPath()) { Converter = converter });
    }
}