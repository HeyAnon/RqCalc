using System.Drawing;

namespace RqCalc.Tests.Extensions;

public static class BitmapExtensions
{
    public static byte[] BytesMakeGrayscale3(this byte[] original)
    {
        using (var inputStream = new MemoryStream(original))
        using (var sourceImage = (Bitmap)Bitmap.FromStream(inputStream))
        using (var resultImage = sourceImage.MakeGrayscale3())
        using (var resultStream = new MemoryStream())
        {
            resultImage.Save(resultStream, ImageFormat.Png);

            return resultStream.ToArray();
        }
    }

    public static Bitmap MakeGrayscale3(this Bitmap original)
    {
        //create a blank bitmap the same size as original
        Bitmap newBitmap = new Bitmap(original.Width, original.Height);

        //get a graphics object from the new image
        Graphics g = Graphics.FromImage(newBitmap);

        //create the grayscale ColorMatrix
        ColorMatrix colorMatrix = new ColorMatrix(
            new float[][]
            {
                new float[] {.3f, .3f, .3f, 0, 0},
                new float[] {.59f, .59f, .59f, 0, 0},
                new float[] {.11f, .11f, .11f, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {0, 0, 0, 0, 1}
            });

        //create some image attributes
        ImageAttributes attributes = new ImageAttributes();

        //set the color matrix attribute
        attributes.SetColorMatrix(colorMatrix);

        //draw the original image on the new image
        //using the grayscale color matrix
        g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
            0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

        //dispose the Graphics object
        g.Dispose();
        return newBitmap;
    }
}