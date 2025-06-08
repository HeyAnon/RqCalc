using Framework.Core;
using Framework.Core.Serialization;
using RqCalc.Application.Serializer._Internal;
using RqCalc.Domain;
using RqCalc.Model;

namespace RqCalc.Application.Serializer.Talent;

internal class TalentBuildSerializer : ISerializer<byte[], ITalentBuildSource>
{
    private readonly ApplicationContext context;
        

    private readonly IReadOnlyDictionary<int, Lazy<TalentBuildVersionSerializer>> serializers;

    internal readonly TalentBuildVersionSerializer LastVersionSerializer;


    public TalentBuildSerializer(ApplicationContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));


        var serializersRequest = from version in context.DataSource.GetFullList<IVersion>()

            where version.Id <= context.LastVersion.Id
                                     
            group version by version.MaxLevel into levelGroup

            let version = levelGroup.OrderBy(v => v.Id).First()

            select version.Id.ToKeyValuePair(LazyHelper.Create(() => new TalentBuildVersionSerializer(this.context, version)));

        this.serializers = serializersRequest.ToDictionary();

        this.LastVersionSerializer = this.serializers.OrderByDescending(ser => ser.Key).First(ser => ser.Key <= context.LastVersion.Id).Value.Value;
    }


    public ITalentBuildSource Parse(byte[] input)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));

        var reader = new BitReader(input);

        var version = reader.ReadByMax(byte.MaxValue);
            
        var serializer = this.serializers.GetValue(version, () => new Exception($"Invalid Version: {version}"));

        return serializer.Value.Parse(reader);
    }

    public byte[] Format(ITalentBuildSource character)
    {
        if (character == null) throw new ArgumentNullException(nameof(character));

        var writer = new BitWriter();

        this.LastVersionSerializer.FullFormat(writer, character);

        return writer.GetBytes();
    }
}