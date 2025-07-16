using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Framework.Core;

using RqCalc.Domain._Base;

namespace RqCalc.Wpf._Extensions;

public static class BitmapFrameExtensions
{
    public static BitmapFrame? TryToBitmapFrame(this IImageObject dataObject) => dataObject.Maybe(v => v.Image!.ToBitmapFrame());

    public static BitmapFrame ToBitmapFrame(this IImage image) => BitmapFrame.Create(new MemoryStream(image.Data), BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.OnLoad);

    public static BitmapSource ToGray(this BitmapSource source)
    {
        var graySource = new FormatConvertedBitmap();

        graySource.BeginInit();
        graySource.Source = source;
        graySource.DestinationFormat = PixelFormats.Gray32Float;
        graySource.EndInit();

        return graySource;
    }
}