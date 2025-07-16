using System.Globalization;
using System.Windows.Data;

using Framework.Core;

using RqCalc.Domain._Base;
using RqCalc.Wpf._Extensions;

namespace RqCalc.Wpf.Converts;

public class ImageDataObjectConverter : IValueConverter
{
    private readonly bool isGray;


    public ImageDataObjectConverter()
    {
    }


    public ImageDataObjectConverter(bool isGray) => this.isGray = isGray;

    public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        this.GetImage(value).Maybe(image =>
                                   {
                                       var bitmap = image.ToBitmapFrame();

                                       if (this.isGray)
                                       {
                                           return new BitmapGrayObjectConverter().Convert(bitmap, null, null, null);
                                       }
                                       else
                                       {
                                           return bitmap;
                                       }
                                   });

    private IImage GetImage(object value)
    {
        if (value is IImageObject)
        {
            var imageDataObject = value as IImageObject;

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

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}