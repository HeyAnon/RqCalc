using Framework.DataBase;
using RqCalc.Domain._Base;

namespace RqCalc.Application.ImageSource;

public class ImageSourceService : IImageSourceService
{
    private readonly IDataSource<IPersistentDomainObjectBase> _dataSource;

    private readonly ImageSourceInvokeCache _invokeCache;



    public ImageSourceService(IDataSource<IPersistentDomainObjectBase> dataSource, ImageSourceInvokeCache invokeCache)
    {
        if (dataSource == null) throw new ArgumentNullException(nameof(dataSource));
        if (invokeCache == null) throw new ArgumentNullException(nameof(invokeCache));

        this._dataSource = dataSource;
        this._invokeCache = invokeCache;
    }



    public IImageSource GetImageSource(string typeName)
    {
        var del = this._invokeCache.FuncCache[typeName];

        return new ImageSourceFunc(id => del(this._dataSource, id));
    }
}

internal class ImageSourceFunc(Func<int, IImage> getImage) : IImageSource
{
    public IImage GetById(int id)
    {
        return getImage(id);
    }
}