using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using Framework.Core;
using RqCalc.Domain._Base;
using RqCalc.Domain._Extensions;
using RqCalc.Wpf.Models;

namespace RqCalc.Wpf.Converts;

public class StackObjectConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IStackObjectModel<IStackObject>)
        {
            var maxStackCount = (value as IStackObjectModel<IStackObject>).SelectedObject.MaxStackCount;

            if (value is IStackObjectModel<IBonusBase>)
            {
                var bonus = (value as IStackObjectModel<IBonusBase>).SelectedObject;
                    
                return Enumerable.Range(0, maxStackCount + 1)
                    .ToObservableCollection(index => index == 0 ? "Нет" : bonus.Multiply(index).EvaluateTemplate());
            }
            else if (value is IStackObjectModel<IBonusContainer<IBonusBase>>)
            {
                var bonusContainer = (value as IStackObjectModel<IBonusContainer<IBonusBase>>).SelectedObject;

                return Enumerable.Range(0, maxStackCount + 1)
                    .ToObservableCollection(index => index == 0 ? "Нет" : bonusContainer.Multiply(index).Bonuses.Join(Environment.NewLine, bonus => bonus.EvaluateTemplate()));
            }
        }

        return new ObservableCollection<string>();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}