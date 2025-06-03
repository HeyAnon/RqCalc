using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

using Framework.Core;


namespace Anon.RQ_Calc.WPF
{
    public abstract class CollectionModifyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var genericType = value.GetType().GetCollectionElementType();

                if (genericType != null)
                {
                    return new Func<IEnumerable<object>, object>(this.Modify)
                        .CreateGenericMethod(genericType)
                        .Invoke(this, new[] { value });
                }
            }

            return value;
        }

        protected abstract object Modify<T>(IEnumerable<T> source);



        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}