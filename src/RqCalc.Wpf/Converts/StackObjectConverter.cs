using System.Globalization;
using System.Windows.Data;

using Framework.Core;

using RqCalc.Domain._Base;
using RqCalc.Domain._Extensions;
using RqCalc.Wpf.Models;

namespace RqCalc.Wpf.Converts;

public class StackObjectConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        switch (value)
        {
            case IStackObjectModel<IStackObject> model:
            {
                var maxStackCount = model.SelectedObject.MaxStackCount;

                switch (value)
                {
                    case IStackObjectModel<IBonusBase> objectModel:
                    {
                        var bonus = objectModel.SelectedObject;
                    
                        return Enumerable.Range(0, maxStackCount + 1)
                            .ToObservableCollection(index => index == 0 ? "Нет" : bonus.Multiply(index).EvaluateTemplate());
                    }

                    case IStackObjectModel<IBonusContainer<IBonusBase>> stackObjectModel:
                    {
                        var bonusContainer = stackObjectModel.SelectedObject;

                        return Enumerable.Range(0, maxStackCount + 1)
                            .ToObservableCollection(index => index == 0 ? "Нет" : bonusContainer.Multiply(index).Bonuses.Join(Environment.NewLine, bonus => bonus.EvaluateTemplate()));
                    }
                }

                break;
            }
        }

        return new ObservableCollection<string>();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}