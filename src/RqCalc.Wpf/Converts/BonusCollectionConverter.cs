using System.Globalization;
using System.Windows.Data;
using Framework.Core;
using RqCalc.Domain._Base;
using RqCalc.Domain._Extensions;
using RqCalc.Domain.Card;

namespace RqCalc.Wpf.Converts;

public class BonusCollectionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ICard)
        {
            var card = value as ICard;

            return card.GetOrderedBonuses().ToObservableCollection(bonus => bonus.ToBonusBase().EvaluateTemplate());
        } 
        else if (value is IBonusContainer<IBonus>)
        {
            var bonusContainer = value as IBonusContainer<IBonus>;

            return bonusContainer.GetOrderedBonuses().ToObservableCollection(v => v.EvaluateTemplate());
        }
        else if (value is IBonusContainer<IBonusBase>)
        {
            var bonusContainer = value as IBonusContainer<IBonusBase>;

            return bonusContainer.Bonuses.ToObservableCollection(v => v.EvaluateTemplate());
        }
        else if (value == null)
        {
            return new ObservableCollection<string>();
        }
        else
        {
            return new ObservableCollection<string>();
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}