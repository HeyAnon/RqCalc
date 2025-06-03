using Framework.Core;
using Framework.Core.Serialization;
using RqCalc.Domain.Model;
using RqCalc.Domain.Persistent;
using RqCalc.Logic.Serializer._Internal;

namespace RqCalc.Logic.Serializer.GuildTalent;

internal class GuildTalentBuildSerializer : ISerializer<byte[], IGuildTalentBuildSource>
{
    private readonly ApplicationContext _context;
        

    private readonly IReadOnlyDictionary<int, Lazy<GuildTalentBuildVersionSerializer>> _serializers;

    internal readonly GuildTalentBuildVersionSerializer LastVersionSerializer;


    public GuildTalentBuildSerializer(ApplicationContext context)
    {
        this._context = context ?? throw new ArgumentNullException(nameof(context));


        var serializersRequest = from version in context.DataSource.GetFullList<IVersion>()

            where version.Id <= context.LastVersion.Id && version.GuildTalents
                                     
            orderby version.Id

            select version.Id.ToKeyValuePair(LazyHelper.Create(() => new GuildTalentBuildVersionSerializer(this._context, version)));

        this._serializers = serializersRequest.Take(1).ToDictionary();

        this.LastVersionSerializer = this._serializers.OrderByDescending(ser => ser.Key).First(ser => ser.Key <= context.LastVersion.Id).Value.Value;
    }


    public IGuildTalentBuildSource Parse(byte[] input)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));

        var reader = new BitReader(input);

        var version = reader.ReadByMax(byte.MaxValue);
            
        var serializer = this._serializers.GetValue(version, () => new Exception($"Invalid Version: {version}"));

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