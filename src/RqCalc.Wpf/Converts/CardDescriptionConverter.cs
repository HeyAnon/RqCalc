using System.Globalization;
using System.Windows.Data;

using RqCalc.Domain.Card;
using RqCalc.Wpf.Models;
using RqCalc.Wpf.Models._Base;
using RqCalc.Wpf.Models.Window.Dialog;

namespace RqCalc.Wpf.Converts;

public class CardDescriptionConverter : IMultiValueConverter
{
    public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        switch (values.Length)
        {
            case >= 2 when (values[0] is ICard || values[0] is ICardGroup) && values[1] is IEvaluateClientContext:
            {
                var card = (values[0] as ICardGroup)?.ActiveCard ?? (ICard)values[0];
                var cardWindowModel = (IEvaluateClientContext)values[1];

                return new CardDescriptionModel(cardWindowModel.EvaluateStats, card);
            }

            default:
                return null;
        }
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}