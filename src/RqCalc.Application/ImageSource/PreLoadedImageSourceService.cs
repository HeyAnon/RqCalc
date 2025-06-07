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

public class PreLoadedImageSourceService : IImageSourceService
{
    private readonly IDictionary<string, IImageSource> _сache;


    public PreLoadedImageSourceService(IDataSource<IPersistentDomainObjectBase> dbSession)
    {
        if (dbSession == null) throw new ArgumentNullException(nameof(dbSession));

        this._сache = this.CreateCache(dbSession).ToDictionary();
    }

    private IEnumerable<KeyValuePair<string, IImageSource>> CreateCache(IDataSource<IPersistentDomainObjectBase> dbSession)
    {
        var createImageSourcePairMethod = new Func<IDataSource<IPersistentDomainObjectBase>, Func<IPersistentIdentityDomainObjectBase, IImage>, string, KeyValuePair<string, IImageSource>>(this.CreateImageSourcePair).Method.GetGenericMethodDefinition();

        var property = ExpressionHelper.Create((IImageObject source) => source.Image).GetProperty();

        var exceptTypes = new[] { typeof(IImageObject), typeof(IStaticImage), typeof(IImageDirectoryBase) };

        var request = from type in TypeSource.FromSample<IPersistentIdentityDomainObjectBase>().GetSourceTypes().Except(exceptTypes)

            where typeof(IImageObject).IsAssignableFrom(type)

            let lambdaExpr = property.ToLambdaExpression()

            let lambda = lambdaExpr.Compile()

            select createImageSourcePairMethod.MakeGenericMethod(type)
                .Invoke<KeyValuePair<string, IImageSource>>(this, dbSession, lambda, default(string));

        foreach (var item in request)
        {
            yield return item;
        }

        yield return "StaticImage".ToKeyValuePair(this.CreateImageSource(dbSession, (IStaticImage v) => v));
        yield return "GrayTalent".ToKeyValuePair(this.CreateImageSource(dbSession, (ITalent v) => v.GrayImage));
        yield return "GrayGuildTalent".ToKeyValuePair(this.CreateImageSource(dbSession, (IGuildTalent v) => v.GrayImage));
        yield return "Costume".ToKeyValuePair(this.CreateImageSource(dbSession, (IEquipment v) => v.CostumeImage));
        yield return "CardTypeToolTip".ToKeyValuePair(this.CreateImageSource(dbSession, (ICardType v) => v.ToolTipImage));
        yield return "BigStampColor".ToKeyValuePair(this.CreateImageSource(dbSession, (IStampColor v) => v.BigImage));
    }


    private KeyValuePair<string, IImageSource> CreateImageSourcePair<T>(IDataSource<IPersistentDomainObjectBase> dbSession, Func<T, IImage> lambda, string name = null)
        where T : class, IPersistentIdentityDomainObjectBase
    {
        var imageSource = this.CreateImageSource(dbSession, lambda);

        return (name ?? typeof(T).Name.Skip("I", true)).ToKeyValuePair(imageSource);
    }

    private IImageSource CreateImageSource<T>(IDataSource<IPersistentDomainObjectBase> dbSession, Func<T, IImage> lambda)
        where T : class, IPersistentIdentityDomainObjectBase
    {
        var source = dbSession.GetFullList<T>().ToDictionary(v => v.Id, lambda);

        return new ImageSourceFunc(id => source[id]);
    }



    public IImageSource GetImageSource(string typeName)
    {
        return this._сache[typeName];
    }
}