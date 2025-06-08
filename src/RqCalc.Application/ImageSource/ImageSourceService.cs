using Framework.DataBase;

using RqCalc.Domain._Base;

namespace RqCalc.Application.ImageSource;

public class ImageSourceService(IDataSource<IPersistentDomainObjectBase> dataSource, ImageSourceFuncCache invokeCache) : IImageSourceService
{
    public IImageSource GetImageSource(string typeName)
    {
        var func = invokeCache.GetFunc(typeName);

        return new FuncImageSource(id => func(dataSource, id));
    }
}

internal class FuncImageSource(Func<int, IImage?> getImage) : IImageSource
{
    public IImage? GetById(int id)
    {
        return getImage(id);
    }
}