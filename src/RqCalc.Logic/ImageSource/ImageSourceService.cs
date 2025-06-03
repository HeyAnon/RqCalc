using Framework.Core;
using Framework.DataBase;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain.Card;
using RqCalc.Domain.Equipment;
using RqCalc.Domain.GuildTalent;
using RqCalc.Domain.Stamp;
using RqCalc.Domain.Talent;

namespace RqCalc.Logic.ImageSource;

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

public class ImageSourceInvokeCache
{
    private readonly ITypeResolver<string> _typeResolver;


    internal readonly IDictionaryCache<string, Func<IDataSource<IPersistentDomainObjectBase>, int, IImage>> FuncCache;


    public ImageSourceInvokeCache(ITypeResolver<string> typeResolver)
    {
        if (typeResolver == null) throw new ArgumentNullException(nameof(typeResolver));

        this._typeResolver = typeResolver;

        this.FuncCache = new DictionaryCache<string, Func<IDataSource<IPersistentDomainObjectBase>, int, IImage>>(typeName =>
        {
            switch (typeName)
            {
                case "StaticImage":
                    return (dataSource, id) => dataSource.GetById<IStaticImage>(id);

                case "GrayTalent":
                    return (dataSource, id) => dataSource.GetById<ITalent>(id).Maybe(v => v.GrayImage);

                case "GrayGuildTalent":
                    return (dataSource, id) => dataSource.GetById<IGuildTalent>(id).Maybe(v => v.GrayImage);

                case "Costume":
                    return (dataSource, id) => dataSource.GetById<IEquipment>(id).Maybe(v => v.CostumeImage);

                case "CardTypeToolTip":
                    return (dataSource, id) => dataSource.GetById<ICardType>(id).Maybe(v => v.ToolTipImage);

                case "BigStampColor":
                    return (dataSource, id) => dataSource.GetById<IStampColor>(id).Maybe(v => v.BigImage);

                default:
                {
                    var type = this._typeResolver.Resolve("I" + typeName, true);

                    return new Func<Func<IDataSource<IPersistentDomainObjectBase>, int, IImage>>(this.GetDefaultImageSourceFunc<IImageDirectoryBase>)
                        .CreateGenericMethod(type)
                        .Invoke<Func<IDataSource<IPersistentDomainObjectBase>, int, IImage>>(this);
                }
            }
        }).WithLock();
    }


    private Func<IDataSource<IPersistentDomainObjectBase>, int, IImage> GetDefaultImageSourceFunc<T>()
        where T : class, IImageDirectoryBase
    {
        return (dataSource, id) => dataSource.GetById<T>(id).Maybe(v => v.Image);
    }


    public static readonly ImageSourceInvokeCache Default = new ImageSourceInvokeCache(DomainTypeResolver.Default);
}

internal class ImageSourceFunc : IImageSource
{
    private readonly Func<int, IImage> _getImage;

    public ImageSourceFunc(Func<int, IImage> getImage)
    {
        if (getImage == null) throw new ArgumentNullException(nameof(getImage));

        this._getImage = getImage;
    }

    public IImage GetById(int id)
    {
        return this._getImage(id);
    }
}