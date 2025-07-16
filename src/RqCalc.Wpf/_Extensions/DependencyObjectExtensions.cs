using System.Windows;
using System.Windows.Media;
using Framework.Core;

namespace RqCalc.Wpf._Extensions;

public static class DependencyObjectExtensions
{
    public static IEnumerable<DependencyObject> GetChildren(this DependencyObject dependencyObject)
    {
        if (dependencyObject == null) throw new ArgumentNullException(nameof(dependencyObject));

        return dependencyObject.GetAllElements(d => d.GetInternalChildren());
    }

    public static IEnumerable<T> GetChildren<T>(this DependencyObject dependencyObject)
        where T : DependencyObject
    {
        if (dependencyObject == null) throw new ArgumentNullException(nameof(dependencyObject));

        return dependencyObject.GetChildren().OfType<T>();
    }


    private static IEnumerable<DependencyObject> GetInternalChildren(this DependencyObject dependencyObject)
    {
        if (dependencyObject == null) throw new ArgumentNullException(nameof(dependencyObject));

        return Enumerable.Range(0, VisualTreeHelper.GetChildrenCount(dependencyObject))
            .Select(i => VisualTreeHelper.GetChild(dependencyObject, i));
    }
}

//public static class ItemsControlExtensions
//{
//    public static IEnumerable<T> GetControls<T>(this ItemsControl itemControl)
//        where T : UIElement
//    {
//        return itemControl.GetControls().OfType<T>();
//    }

//    public static IEnumerable<UIElement> GetControls(this ItemsControl itemControl)
//    {
//        if (itemControl == null) throw new ArgumentNullException("itemControl");

//        foreach (var item in itemControl.Items)
//        {
//            yield return (UIElement)itemControl.ItemContainerGenerator.ContainerFromItem(item);
//        }
//    }
//}