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

public class ImageSourceFuncCache(ITypeResolver<string> typeResolver)
{
    private readonly IDictionaryCache<string, Func<IDataSource<IPersistentDomainObjectBase>, int, IImage?>> cache =
        new DictionaryCache<string, Func<IDataSource<IPersistentDomainObjectBase>, int, IImage?>>(typeName =>
        {
            switch (typeName)
            {
                case "StaticImage":
                    return GetImageSourceFunc<IStaticImage>(v => v);

                case "GrayTalent":
                    return GetImageSourceFunc<ITalent>(v => v.GrayImage);

                case "GrayGuildTalent":
                    return GetImageSourceFunc<IGuildTalent>(v => v.GrayImage);

                case "Costume":
                    return GetImageSourceFunc<IEquipment>(v => v.CostumeImage);

                case "CardTypeToolTip":
                    return GetImageSourceFunc<ICardType>(v => v.ToolTipImage);

                case "BigStampColor":
                    return GetImageSourceFunc<IStampColor>(v => v.BigImage);

                default:
                {
                    var type = typeResolver.Resolve("I" + typeName, true);

                    return new Func<Func<IDataSource<IPersistentDomainObjectBase>, int, IImage?>>(GetDefaultImageSourceFunc<IImageDirectoryBase>)
                        .CreateGenericMethod(type)
                        .Invoke<Func<IDataSource<IPersistentDomainObjectBase>, int, IImage?>>(null);
                }
            }
        }).WithLock();

    public Func<IDataSource<IPersistentDomainObjectBase>, int, IImage?> GetFunc(string typeName) => this.cache[typeName];

    private static Func<IDataSource<IPersistentDomainObjectBase>, int, IImage?> GetDefaultImageSourceFunc<T>()
        where T : class, IImageDirectoryBase =>
        GetImageSourceFunc<T>(v => v.Image);

    private static Func<IDataSource<IPersistentDomainObjectBase>, int, IImage?> GetImageSourceFunc<T>(Func<T, IImage> path)
        where T : class, IPersistentDomainObjectBase, IIdentityObject<int> =>
        (dataSource, id) => dataSource.TryGetById<T>(id).Maybe(path);

    //public static readonly ImageSourceInvokeCache Default = new ImageSourceInvokeCache(DomainTypeResolver.Default);
}