using System.Collections.ObjectModel;

using Framework.Core;
using Framework.DataBase;
using RqCalc.Application.IndexedDict;
using RqCalc.Application.Serializer._Internal;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain._Extensions;
using RqCalc.Domain.Talent;
using RqCalc.Model;

namespace RqCalc.Application.Serializer.Talent;

internal class TalentBuildVersionSerializer
{
    private readonly IVersion version;

    private readonly IIndexedDict<IClass> classes;

    private readonly IReadOnlyDictionary<IClass, ReadOnlyCollection<Tuple<ITalentBranch, IReadOnlyList<IReadOnlyList<ITalent>>>>> talents;
        

    public TalentBuildVersionSerializer(IDataSource<IPersistentDomainObjectBase> dataSource, IVersion version)
    {
        this.version = version ?? throw new ArgumentNullException(nameof(version));

        var classList = dataSource.GetFullList<IClass>();

        this.classes = IndexedDict.Create(classList, false);

        this.talents = this.classes.OrderById().ToReadOnlyDictionary(@class => @class, @class =>

            @class.GetAllTalentBranches().OrderById().ToReadOnlyCollection(branch =>

                Tuple.Create(branch, branch.Talents.GroupBy(t => t.HIndex).OrderBy(v => v.Key).ToReadOnlyListI(g =>

                    g.OrderBy(v => v.VIndex).ToReadOnlyListI()))));
    }


    public void FullFormat(BitWriter writer, ITalentBuildSource character)
    {
        if (writer == null) throw new ArgumentNullException(nameof(writer));
        if (character == null) throw new ArgumentNullException(nameof(character));

        writer.WriteByMax(this.version.Id, byte.MaxValue);

        this.Format(writer, character);
    }

    public void Format(BitWriter writer, ITalentBuildSource character)
    {
        if (writer == null) throw new ArgumentNullException(nameof(writer));
        if (character == null) throw new ArgumentNullException(nameof(character));
            
        writer.Write(character.Class, this.classes);

        writer.WriteByMax(character.Level, character.Class.Specialization.MaxLevel ?? this.version.MaxLevel);

        var charBranches = character.Talents.GroupBy(tal => tal.Branch).ToDictionary(g => g.Key, g => g.OrderBy(v => v.HIndex).ToReadOnlyCollection());

        foreach (var branchPair in this.talents[character.Class])
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

        var @class = reader.Read(this.classes);

        var level = reader.ReadByMax(@class.Specialization.MaxLevel ?? this.version.MaxLevel);

        var talentList = this.ReadTalents(reader, @class).ToList();

        return new TalentBuildSource
        {
            Class = @class,

            Level = level,

            Talents = talentList
        };
    }


    private IEnumerable<ITalent> ReadTalents(BitReader reader, IClass @class)
    {
        if (reader == null) throw new ArgumentNullException(nameof(reader));
        if (@class == null) throw new ArgumentNullException(nameof(@class));

        return from branchPair in this.talents[@class]

            let talentCount = reader.ReadByMax(branchPair.Item2.Count)

            from talentVertList in branchPair.Item2.Take(talentCount)

            let talentIndex = reader.ReadByMax(talentVertList.Count - 1)

            let talent = talentVertList[talentIndex]

            select talent;
    }
}