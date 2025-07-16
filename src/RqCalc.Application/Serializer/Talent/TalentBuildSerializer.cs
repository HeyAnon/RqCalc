using Framework.Core;
using Framework.Core.Serialization;
using Framework.DataBase;

using RqCalc.Application.Serializer._Internal;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Model;

namespace RqCalc.Application.Serializer.Talent;

public class TalentBuildSerializer : ISerializer<byte[], ITalentBuildSource>
{
    private readonly IReadOnlyDictionary<int, Lazy<TalentBuildVersionSerializer>> serializers;

    private readonly Lazy<TalentBuildVersionSerializer> lazyLastVersionSerializer;


    public TalentBuildSerializer(IDataSource<IPersistentDomainObjectBase> dataSource, IVersion lastVersion)
    {
        var serializersRequest =

            from version in dataSource.GetFullList<IVersion>()

            where version.Id <= lastVersion.Id

            group version by version.MaxLevel
            into levelGroup

            let version = levelGroup.OrderBy(v => v.Id).First()

            select (version.Id, LazyHelper.Create(() => new TalentBuildVersionSerializer(dataSource, version)));

        this.serializers = serializersRequest.ToDictionary();

        this.lazyLastVersionSerializer = LazyHelper.Create(() => this.serializers.OrderByDescending(ser => ser.Key).First(ser => ser.Key <= lastVersion.Id).Value.Value);
    }


    public ITalentBuildSource Parse(byte[] input)
    {
        var reader = new BitReader(input);

        var version = reader.ReadByMax(byte.MaxValue);

        var serializer = this.serializers.GetValue(version, () => new Exception($"Invalid Version: {version}"));

        return serializer.Value.Parse(reader);
    }

    public byte[] Format(ITalentBuildSource character)
    {
        var writer = new BitWriter();

        this.lazyLastVersionSerializer.Value.FullFormat(writer, character);

        return writer.GetBytes();
    }
}