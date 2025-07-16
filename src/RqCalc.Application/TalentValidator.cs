using System.ComponentModel.DataAnnotations;

using Framework.Core;

using RqCalc.Domain;
using RqCalc.Domain._Extensions;
using RqCalc.Model;

namespace RqCalc.Application;

public class TalentValidator(IVersion lastVersion, IFreeTalentCalculator freeTalentCalculator) : IValidator<ITalentBuildSource>
{
    public void Validate(ITalentBuildSource character)
    {
        if (character == null) throw new ArgumentNullException(nameof(character));

        if (character.Level < 1 || character.Level > (character.Class.Specialization.MaxLevel ?? lastVersion.MaxLevel))
        {
            throw new ValidationException($"Invalid Level. Out of range: {character.Level}");
        }

        character.Talents.GroupBy(tal => tal.Branch).ToList().Pipe(talentBranchGroups =>
        {
            foreach (var talentBranchGroup in talentBranchGroups)
            {
                var branch = talentBranchGroup.Key;

                if (!character.Class.IsSubsetOf(branch.Class))
                {
                    throw new ValidationException($"Invalid TalentBrunch \"{branch.Name}\" Class: {branch.Class}");
                }

                foreach (var pair in branch.Talents.GroupBy(tal => tal.HIndex).OrderBy(g => g.Key)
                             .Zip(
                                 talentBranchGroup.GroupBy(tal => tal.HIndex)
                                     .OrderBy(g => g.Key)
                                     .Select(instance => instance.SingleOrDefault(dup => new Exception(
                                         $"To many talents ({dup.Join(", ", t => t.Name)}) in one vertical set {instance.Key}"))),

                                 (definition, talent) => new { Definition = definition, Talent = talent }))
                {
                    var definition = pair.Definition;

                    var talent = pair.Talent;

                    if (!definition.Contains(talent))
                    {
                        throw new ValidationException($"Invalid Talent \"{talent.Name}\" Position");
                    }
                }
            }
        });


        freeTalentCalculator.GetFreeTalents(character).Pipe(freeTalents =>
        {
            if (freeTalents < 0)
            {
                throw new ValidationException($"Invalid talents. Overflow usage talents: {-freeTalents}");
            }
        });
    }
}