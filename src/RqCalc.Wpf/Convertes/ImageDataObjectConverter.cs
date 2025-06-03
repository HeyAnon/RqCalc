using System;
using System.Globalization;
using System.Windows.Data;

using Anon.RQ_Calc.Domain;

using Framework.Core;

namespace Anon.RQ_Calc.WPF
{
    public class ImageDataObjectConverter : IValueConverter
    {
        private readonly bool _isGray;


        public ImageDataObjectConverter()
        {
        }


        public ImageDataObjectConverter(bool isGray)
        {
            this._isGray = isGray;
        }

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.GetImage(value).Maybe(image =>
            {
                var bitmap = image.ToBitmapFrame();

                if (this._isGray)
                {
                    return new BitmapGrayObjectConverter().Convert(bitmap, null, null, null);
                }
                else
                {
                    return bitmap;
                }
            });
        }

        private IImage GetImage(object value)
        {
            if (value is Domain.IImageObject)
            {
                var imageDataObject = value as Domain.IImageObject;

                return imageDataObject.Image;
            }
            else if (value is IImage)
            {
                return value as IImage;
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}