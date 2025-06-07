using System.Collections.ObjectModel;
using Framework.Core;
using RqCalc.Application.Serializer._Internal;
using RqCalc.Domain;
using RqCalc.Domain._Extensions;
using RqCalc.Domain.Talent;
using RqCalc.Model;

namespace RqCalc.Application.Serializer.Talent;

internal class TalentBuildVersionSerializer
{
    private readonly ApplicationContext _context;

    private readonly IVersion _version;

    private readonly IIndexedDict<IClass> _classes;

    private readonly IReadOnlyDictionary<IClass, ReadOnlyCollection<Tuple<ITalentBranch, IReadOnlyList<IReadOnlyList<ITalent>>>>> _talents;
        

    public TalentBuildVersionSerializer(ApplicationContext context, IVersion version)
    {
        this._context = context ?? throw new ArgumentNullException(nameof(context));
        this._version = version ?? throw new ArgumentNullException(nameof(version));

        var classes = this._context.DataSource.GetFullList<IClass>();

        this._classes = IndexedDict.Create(classes, false);

        this._talents = classes.OrderById().ToReadOnlyDictionary(@class => @class, @class =>

            @class.GetAllTalentBranches().OrderById().ToReadOnlyCollection(branch =>

                Tuple.Create(branch, branch.Talents.GroupBy(t => t.HIndex).OrderBy(v => v.Key).ToReadOnlyListI(g =>

                    g.OrderBy(v => v.VIndex).ToReadOnlyListI()))));
    }


    public void FullFormat(BitWriter writer, ITalentBuildSource character)
    {
        if (writer == null) throw new ArgumentNullException(nameof(writer));
        if (character == null) throw new ArgumentNullException(nameof(character));

        writer.WriteByMax(this._version.Id, byte.MaxValue);

        this.Format(writer, character);
    }

    public void Format(BitWriter writer, ITalentBuildSource character)
    {
        if (writer == null) throw new ArgumentNullException(nameof(writer));
        if (character == null) throw new ArgumentNullException(nameof(character));
            
        writer.Write(character.Class, this._classes);

        writer.WriteByMax(character.Level, character.Class.Specialization.MaxLevel ?? this._version.MaxLevel);

        var charBranches = character.Talents.GroupBy(tal => tal.Branch).ToDictionary(g => g.Key, g => g.OrderBy(v => v.HIndex).ToReadOnlyCollection());

        foreach (var branchPair in this._talents[character.Class])
        {
            var charTalents = charBranches.GetValueOrDefault(branchPair.Item1);

            writer.WriteByMax(charTalents.Maybe(v => v.Count, 0), branchPair.Item2.Count);

            if (charTalents != null)
            {
                foreach (var talentPair in branchPair.Item2.Zip(charTalents, (definition, talent) => new { Definition = definition, Talent = talent }))
                {
                    var index = talentPair.Definition.IndexOf(talentPair.Talent);

                    writer.WriteByMax(index, talentPair.Definition.Count - 1);
                }
            }
        }
    }

    public ITalentBuildSource Parse(BitReader reader)
    {
        if (reader == null) throw new ArgumentNullException(nameof(reader));

        var @class = reader.Read(this._classes);

        var level = reader.ReadByMax(@class.Specialization.MaxLevel ?? this._version.MaxLevel);

        var talents = this.ReadTalents(reader, @class).ToList();

        return new TalentBuildSource
        {
            Class = @class,

            Level = level,

            Talents = talents
        };
    }


    private IEnumerable<ITalent> ReadTalents(BitReader reader, IClass @class)
    {
        if (reader == null) throw new ArgumentNullException(nameof(reader));
        if (@class == null) throw new ArgumentNullException(nameof(@class));

        return from branchPair in this._talents[@class]

            let talentCount = reader.ReadByMax(branchPair.Item2.Count)

            from talentVertList in branchPair.Item2.Take(talentCount)

            let talentIndex = reader.ReadByMax(talentVertList.Count - 1)

            let talent = talentVertList[talentIndex]

            select talent;
    }
}