using Framework.Core;
using Framework.Core.Serialization;
using Framework.DataBase;

using RqCalc.Application.Serializer._Internal;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Model;

namespace RqCalc.Application.Serializer.GuildTalent;

public class GuildTalentBuildSerializer : ISerializer<byte[], IGuildTalentBuildSource>
{
    private readonly IReadOnlyDictionary<int, Lazy<GuildTalentBuildVersionSerializer>> serializers;

    private readonly Lazy<GuildTalentBuildVersionSerializer> lazyLastVersionSerializer;


    public GuildTalentBuildSerializer(
        IDataSource<IPersistentDomainObjectBase> dataSource,
        IVersion lastVersion)
    {
        var serializersRequest =

            from version in dataSource.GetFullList<IVersion>()

            where version.Id <= lastVersion.Id && version.GuildTalents

            orderby version.Id

            select (version.Id, LazyHelper.Create(() => new GuildTalentBuildVersionSerializer(dataSource, version)));

        this.serializers = serializersRequest.Take(1).ToDictionary();

        this.lazyLastVersionSerializer = LazyHelper.Create(() => this.serializers.OrderByDescending(ser => ser.Key).First(ser => ser.Key <= lastVersion.Id).Value.Value);
    }


    public IGuildTalentBuildSource Parse(byte[] input)
    {
        var reader = new BitReader(input);

        var version = reader.ReadByMax(byte.MaxValue);

        var serializer = this.serializers.GetValue(version, () => new Exception($"Invalid Version: {version}"));

        return serializer.Value.Parse(reader);
    }

    public byte[] Format(IGuildTalentBuildSource guildTalentBuildSource)
    {
        var writer = new BitWriter();

        this.lazyLastVersionSerializer.Value.FullFormat(writer, guildTalentBuildSource);

        return writer.GetBytes();
    }
}