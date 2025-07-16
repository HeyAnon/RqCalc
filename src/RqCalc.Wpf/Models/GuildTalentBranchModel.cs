using Framework.Core;
using Framework.Reactive;

using RqCalc.Domain.GuildTalent;
using RqCalc.Wpf.Models.Window.Dialog;

namespace RqCalc.Wpf.Models;

public class GuildTalentBranchModel : NotifyModelBase
{
    public readonly GuildTalentsWindowModel WindowModel;


    public GuildTalentBranchModel(GuildTalentsWindowModel windowModel, IGuildTalentBranch talentBranch)

    {
        this.WindowModel = windowModel ?? throw new ArgumentNullException(nameof(windowModel));
        this.GuildTalentBranch = talentBranch ?? throw new ArgumentNullException(nameof(talentBranch));

        this.Talents = talentBranch.Talents.ToObservableCollection(tal => new GuildTalentModel(this, tal));
    }

    public GuildTalentBranchModel PrevBranch => this.WindowModel.Branches.TakeWhile(b => b != this).LastOrDefault();

    public GuildTalentBranchModel NextBranch => this.WindowModel.Branches.SkipWhile(b => b != this).Skip(1).FirstOrDefault();

    public bool Maximized => this.Talents.Sum(t => t.Points) == this.GuildTalentBranch.MaxPoints;

    public bool Editable => (this.PrevBranch?.Maximized ?? true) && !(this.NextBranch?.HasActiveTalents ?? false);

    public bool HasActiveTalents => this.Talents.Any(t => t.Active);

    public IReadOnlyDictionary<IGuildTalent, int> ActiveTalents
    {
        get
        {
            var request = from model in this.Talents

                where model.Points > 0

                select model.SelectedObject.ToKeyValuePair(model.Points);

            return request.ToDictionary();
        }
        set
        {
            foreach (var model in this.Talents)
            {
                model.Points = value.GetValueOrDefault(model.SelectedObject);
            }
        }
    }
        
    public ObservableCollection<GuildTalentModel> Talents
    {
        get => this.GetValue(v => v.Talents);
        private set => this.SetValue(v => v.Talents, value);
    }

    public IGuildTalentBranch GuildTalentBranch
    {
        get => this.GetValue(v => v.GuildTalentBranch);
        private set => this.SetValue(v => v.GuildTalentBranch, value);
    }
}