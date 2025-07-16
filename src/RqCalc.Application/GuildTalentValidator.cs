using System.ComponentModel.DataAnnotations;

using Framework.Core;
using Framework.DataBase;

using RqCalc.Domain._Base;
using RqCalc.Domain._Extensions;
using RqCalc.Domain.GuildTalent;
using RqCalc.Model;

namespace RqCalc.Application;

public class GuildTalentValidator(IDataSource<IPersistentDomainObjectBase> dataSource) : IValidator<IGuildTalentBuildSource>
{
    public void Validate(IGuildTalentBuildSource character)
    {
        var initializedTalents = character.GuildTalents.Where(pair => pair.Value != 0).ToList();

        initializedTalents.Select(t => t.Key).GetDuplicates().ToList().Pipe(guildBonusDuplicates =>
        {
            if (guildBonusDuplicates.Any())
            {
                throw new ValidationException($"Invalid Guild Talents. Duplicate elements: {guildBonusDuplicates.Join(", ", gb => gb.Name)}");
            }
        });

        initializedTalents.GroupBy(pair => pair.Key.Branch, pair => pair.Key)
            .Where(g => g.Count() > 1).Foreach(overflowGroup =>
                throw new ValidationException($"More one guild talent ({overflowGroup.Join(", ")}) in branch \"{overflowGroup.Key}\""));

        var missedBranches = dataSource.GetFullList<IGuildTalentBranch>().OrderById().Take(initializedTalents.Count).Except(initializedTalents.Select(t => t.Key.Branch)).ToList();

        if (missedBranches.Any())
        {
            throw new ValidationException($"Missed Talent in Branches: {missedBranches.Join(", ")}");
        }

        foreach (var pair in initializedTalents.OrderBy(tal => tal.Key.Branch.Id)
                     .Select((pair, index) => new { Talent = pair.Key, Points = pair.Value, IsLast = index == initializedTalents.Count - 1 }))
        {
            if (pair.Points <= 0 || pair.Points > pair.Talent.Branch.MaxPoints)
            {
                throw new ValidationException($"Invalid point count ({pair.Points}) in guild talent: \"{pair.Talent}\"");
            }

            if (!pair.IsLast && pair.Points != pair.Talent.Branch.MaxPoints)
            {
                throw new ValidationException($"Points ({pair.Points}) in guild talent \"{pair.Talent}\" must be maximized ({pair.Talent.Branch.MaxPoints})");
            }
        }
    }
}