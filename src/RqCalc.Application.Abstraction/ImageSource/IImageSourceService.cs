using RqCalc.Domain;
using RqCalc.Domain._Base;

namespace RqCalc.Application.ImageSource;

public interface IImageSourceService
{
    IImageSource GetImageSource(string typeName);
}

public interface IImageSource
{
    IImage? GetById(int id);
}

public static class ImageSourceServiceExtensions
{
    public static IImage? GetStaticImage(this IImageSourceService service, StaticImageType type)
    {
        if (service == null) throw new ArgumentNullException(nameof(service));

        return service.GetImageSource("StaticImage").GetById((int)type);
    }
}