using Framework.Core;
using Framework.Core.Serialization;
using Framework.DataBase;
using Framework.Reactive;

using RqCalc.Application;
using RqCalc.Domain._Base;
using RqCalc.Domain._Extensions;
using RqCalc.Domain.GuildTalent;
using RqCalc.Model;
using RqCalc.Wpf.Exception;
using RqCalc.Wpf.Models._Base;
using RqCalc.Wpf.Models.Window.Dialog._Base;

namespace RqCalc.Wpf.Models.Window.Dialog;

public class GuildTalentsWindowModel(
    ISerializer<string, IGuildTalentBuildSource> guildTalentSerializer,
    IGuildTalentCalculator guildTalentCalculator) : UpdateModel, IClearModel, IGuildTalentBuildSource
{
    public GuildTalentsWindowModel(
        ISerializer<string, IGuildTalentBuildSource> guildTalentSerializer,
        IGuildTalentCalculator guildTalentCalculator,
        IDataSource<IPersistentDomainObjectBase> dataSource,
        IGuildTalentBuildSource talentBuildSource)
        : this(guildTalentSerializer, guildTalentCalculator)
    {
        this.Branches = dataSource.GetFullList<IGuildTalentBranch>().OrderById().ToObservableCollection(branch => new GuildTalentBranchModel(this, branch));

        this.SetData(talentBuildSource);
    }

    public string Code
    {
        get => this.GetValue(v => v.Code);
        set
        {
            if (this.Code == value)
            {
                return;
            }

            var prevValue = this.Code;

            this.SetValue(v => v.Code, value);

            try
            {
                this.SetData(guildTalentSerializer.Deserialize(this.Code));
            }
            catch (System.Exception ex)
            {
                this.SetValue(v => v.Code, prevValue);

                throw new ClientException("Invalid code", ex);
            }
        }
    }

    public ObservableCollection<GuildTalentBranchModel> Branches
    {
        get => this.GetValue(v => v.Branches);
        private set => this.SetValue(v => v.Branches, value);
    }

    public int FreeGuildTalents
    {
        get => this.GetValue(v => v.FreeGuildTalents);
        private set => this.SetValue(v => v.FreeGuildTalents, value);
    }

    public IReadOnlyDictionary<IGuildTalent, int> ActiveTalents
    {
        get => this.Branches.SelectMany(b => b.ActiveTalents).ToDictionary();
        private set
        {
            var newActiveDict = value.GroupBy(pair => pair.Key.Branch).ToDictionary(pair => pair.Key);

            foreach (var g in newActiveDict)
            {
                var branchModel = this.Branches.Single(b => b.GuildTalentBranch == g.Key);

                branchModel.ActiveTalents = g.Value.ToDictionary();
            }

            foreach (var branchModel in this.Branches.Where(branchModel => !newActiveDict.ContainsKey(branchModel.GuildTalentBranch)))
            {
                branchModel.ActiveTalents = new Dictionary<IGuildTalent, int>();
            }
        }
    }



    public bool CloseDialog => false;


    public void Clear() => this.UpdateOperation(() => this.ActiveTalents = new Dictionary<IGuildTalent, int>());

    public void Change(GuildTalentModel talentModel, bool increase)
    {
        if (talentModel == null) throw new ArgumentNullException(nameof(talentModel));

        this.UpdateOperation(() =>
        {
            var branchModel = talentModel.Branch;

            var prevActiveTalent = branchModel.Talents.Except([talentModel]).SingleOrDefault(tal => tal.Active);

            if (prevActiveTalent != null)
            {
                talentModel.Points = prevActiveTalent.Points;
                prevActiveTalent.Points = 0;
            }
            else
            {
                if (branchModel.Editable)
                {
                    if (increase)
                    {
                        talentModel.Points = talentModel.Points == branchModel.GuildTalentBranch.MaxPoints ? 0 : talentModel.Points + 1;
                    }
                    else
                    {
                        talentModel.Points = talentModel.Points == 0 ? branchModel.GuildTalentBranch.MaxPoints : talentModel.Points - 1;
                    }
                }
            }
        });
    }

    private void SetData(IGuildTalentBuildSource character)
    {
        if (character == null) throw new ArgumentNullException(nameof(character));

        this.UpdateOperation(() => this.ActiveTalents = character.GuildTalents);
    }


    protected override void InternalRecalculate()
    {
        this.FreeGuildTalents = guildTalentCalculator.GetFreeGuildTalents(this);

        var code = guildTalentSerializer.Serialize(this);

        this.SetValue(v => v.Code, code);

        this.FreeGuildTalents = guildTalentCalculator.GetFreeGuildTalents(this);
    }


    IReadOnlyDictionary<IGuildTalent, int> IGuildTalentBuildSource.GuildTalents => this.ActiveTalents;
}