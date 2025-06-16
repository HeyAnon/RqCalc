using Framework.Core;
using Framework.Core.Serialization;

using RqCalc.Application.Serializer._Internal;
using RqCalc.Domain;
using RqCalc.Model;

namespace RqCalc.Application.Serializer.GuildTalent;

internal class GuildTalentBuildSerializer : ISerializer<byte[], IGuildTalentBuildSource>
{
    private readonly ApplicationContext context;
        

    private readonly IReadOnlyDictionary<int, Lazy<GuildTalentBuildVersionSerializer>> serializers;

    internal readonly GuildTalentBuildVersionSerializer LastVersionSerializer;


    public GuildTalentBuildSerializer(ApplicationContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));


        var serializersRequest = from version in dataSource.GetFullList<IVersion>()

            where version.Id <= lastVersion.Id && version.GuildTalents
                                     
            orderby version.Id

            select version.Id.ToKeyValuePair(LazyHelper.Create(() => new GuildTalentBuildVersionSerializer(this.context, version)));

        this.serializers = serializersRequest.Take(1).ToDictionary();

        this.LastVersionSerializer = this.serializers.OrderByDescending(ser => ser.Key).First(ser => ser.Key <= lastVersion.Id).Value.Value;
    }


    public IGuildTalentBuildSource Parse(byte[] input)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));

        var reader = new BitReader(input);

        var version = reader.ReadByMax(byte.MaxValue);
            
        var serializer = this.serializers.GetValue(version, () => new Exception($"Invalid Version: {version}"));

        return serializer.Value.Parse(reader);
    }

    public byte[] Format(IGuildTalentBuildSource guildTalentBuildSource)
    {
        if (guildTalentBuildSource == null) throw new ArgumentNullException(nameof(guildTalentBuildSource));

        var writer = new BitWriter();

        this.LastVersionSerializer.FullFormat(writer, guildTalentBuildSource);

        return writer.GetBytes();
    }
}