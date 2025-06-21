using System.Globalization;
using System.Windows.Data;

using RqCalc.Domain.Card;
using RqCalc.Wpf.Models;
using RqCalc.Wpf.Models._Base;
using RqCalc.Wpf.Models.Window.Dialog;

namespace RqCalc.Wpf.Converts;

public class CardDescriptionConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length >= 2 && (values[0] is ICard || values[0] is ICardGroup) && values[1] is IEvaluateClientContext)
        {
            var card = (values[0] as ICardGroup)?.ActiveCard ?? (values[0] as ICard);
            var cardWindowModel = values[1] as IEvaluateClientContext;

            return new CardDescriptionModel(cardWindowModel.Context, cardWindowModel.EvaluateStats, card);
        }

        return null;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}