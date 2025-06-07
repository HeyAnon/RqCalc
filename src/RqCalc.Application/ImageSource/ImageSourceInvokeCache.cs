using Framework.Core;
using Framework.DataBase;
using Framework.Persistent;

using RqCalc.Application._Extensions;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain.Card;
using RqCalc.Domain.Equipment;
using RqCalc.Domain.GuildTalent;
using RqCalc.Domain.Stamp;
using RqCalc.Domain.Talent;

namespace RqCalc.Application.ImageSource;

public class ImageSourceInvokeCache
{
    internal readonly IDictionaryCache<string, Func<IDataSource<IPersistentDomainObjectBase>, int, IImage>> FuncCache;


    public ImageSourceInvokeCache(ITypeResolver<string> typeResolver)
    {
        if (typeResolver == null) throw new ArgumentNullException(nameof(typeResolver));

        var typeResolver1 = typeResolver;

        this.FuncCache = new DictionaryCache<string, Func<IDataSource<IPersistentDomainObjectBase>, int, IImage?>>(typeName =>
        {
            switch (typeName)
            {
                case "StaticImage":
                    return this.GetImageSourceFunc<IStaticImage>(v => v);

                case "GrayTalent":
                    return this.GetImageSourceFunc<ITalent>(v => v.GrayImage);

                case "GrayGuildTalent":
                    return this.GetImageSourceFunc<IGuildTalent>(v => v.GrayImage); 

                case "Costume":
                    return this.GetImageSourceFunc<IEquipment>(v => v.CostumeImage);

                case "CardTypeToolTip":
                    return this.GetImageSourceFunc<ICardType>(v => v.ToolTipImage); 

                case "BigStampColor":
                    return this.GetImageSourceFunc<IStampColor>(v => v.BigImage);

                default:
                {
                    var type = typeResolver1.Resolve("I" + typeName, true);

                    return new Func<Func<IDataSource<IPersistentDomainObjectBase>, int, IImage>>(this.GetDefaultImageSourceFunc<IImageDirectoryBase>)
                        .CreateGenericMethod(type)
                        .Invoke<Func<IDataSource<IPersistentDomainObjectBase>, int, IImage>>(this);
                }
            }
        }).WithLock();
    }
    private Func<IDataSource<IPersistentDomainObjectBase>, int, IImage?> GetDefaultImageSourceFunc<T>()
        where T : class, IImageDirectoryBase
    {
        return this.GetImageSourceFunc<T>(v => v.Image);
    }

    private Func<IDataSource<IPersistentDomainObjectBase>, int, IImage?> GetImageSourceFunc<T>(Func<T, IImage> path)
        where T : class, IPersistentDomainObjectBase, IIdentityObject<int>
    {
        return (dataSource, id) => dataSource.TryGetById<T>(id).Maybe(path);
    }



    //public static readonly ImageSourceInvokeCache Default = new ImageSourceInvokeCache(DomainTypeResolver.Default);
}