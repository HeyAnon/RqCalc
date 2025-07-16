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
