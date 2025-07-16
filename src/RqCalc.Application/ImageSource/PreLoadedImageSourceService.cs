using Framework.Core;
using Framework.DataBase;

using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain.Card;
using RqCalc.Domain.Equipment;
using RqCalc.Domain.GuildTalent;
using RqCalc.Domain.Stamp;
using RqCalc.Domain.Talent;

namespace RqCalc.Application.ImageSource;



public class PreLoadedImageSourceService(
    ITypeResolver<string> typeResolver,
    IDataSource<IPersistentDomainObjectBase> dataSource) : IImageSourceService
{
    private readonly IDictionary<string, IImageSource> cache = CreateCache(typeResolver, dataSource).ToDictionary();

    private static IEnumerable<ValueTuple<string, IImageSource>> CreateCache(ITypeResolver<string> typeResolver, IDataSource<IPersistentDomainObjectBase> dataSource)
    {
        var createImageSourcePairMethod =
            new Func<IDataSource<IPersistentDomainObjectBase>, Func<IPersistentIdentityDomainObjectBase, IImage>, string, ValueTuple<string, IImageSource>>(CreateImageSourcePair)
                .Method.GetGenericMethodDefinition();

        var property = ExpressionHelper.Create((IImageObject source) => source.Image).GetProperty();

        var exceptTypes = new[] { typeof(IImageObject), typeof(IStaticImage), typeof(IImageDirectoryBase) };

        var request =

            from type in typeResolver.GetTypes().Except(exceptTypes)

            where typeof(IImageObject).IsAssignableFrom(type)

            let lambdaExpr = property.ToLambdaExpression()

            let lambda = lambdaExpr.Compile()

            select createImageSourcePairMethod.MakeGenericMethod(type)
                .Invoke<ValueTuple<string, IImageSource>>(null, dataSource, lambda, default(string));

        foreach (var item in request)
        {
            yield return item;
        }

        yield return ("StaticImage", CreateImageSource(dataSource, (IStaticImage v) => v));
        yield return ("GrayTalent", CreateImageSource(dataSource, (ITalent v) => v.GrayImage));
        yield return ("GrayGuildTalent", CreateImageSource(dataSource, (IGuildTalent v) => v.GrayImage));
        yield return ("Costume", CreateImageSource(dataSource, (IEquipment v) => v.CostumeImage));
        yield return ("CardTypeToolTip", CreateImageSource(dataSource, (ICardType v) => v.ToolTipImage));
        yield return ("BigStampColor", CreateImageSource(dataSource, (IStampColor v) => v.BigImage));
    }

    private static ValueTuple<string, IImageSource> CreateImageSourcePair<T>(IDataSource<IPersistentDomainObjectBase> dataSource, Func<T, IImage> lambda, string? name = null)
        where T : class, IPersistentIdentityDomainObjectBase
    {
        var imageSource = CreateImageSource(dataSource, lambda);

        return (name ?? typeof(T).Name.Skip("I", true), imageSource);
    }

    private static IImageSource CreateImageSource<T>(IDataSource<IPersistentDomainObjectBase> dataSource, Func<T, IImage> lambda)
        where T : class, IPersistentIdentityDomainObjectBase
    {
        var source = dataSource.GetFullList<T>().ToDictionary(v => v.Id, lambda);

        return new FuncImageSource(id => source[id]);
    }

    public IImageSource GetImageSource(string typeName) => this.cache[typeName];
}