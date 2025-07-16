using Framework.Core;
using Framework.Core.Serialization;
using Framework.HierarchicalExpand;
using Framework.Reactive;

using RqCalc.Domain;
using RqCalc.Domain._Extensions;
using RqCalc.Domain.Talent;
using RqCalc.Model;
using RqCalc.Wpf.Exception;
using RqCalc.Wpf.Models._Base;
using RqCalc.Wpf.Models.Window.Dialog._Base;
using RqCalc.Application;

namespace RqCalc.Wpf.Models.Window.Dialog;

public class TalentsWindowModel : UpdateModel, IClearModel, ITalentBuildSource
{
    private readonly ISerializer<string, ITalentBuildSource> talentSerializer;

    private readonly IFreeTalentCalculator freeTalentCalculator;

    private readonly ITalentCalculator talentCalculator;

    private readonly ITalentBuildSource talentBuildSource;


    public TalentsWindowModel(
        ISerializer<string, ITalentBuildSource> talentSerializer,
        IFreeTalentCalculator freeTalentCalculator,
        ITalentCalculator talentCalculator,
        ITalentBuildSource talentBuildSource,
        IReadOnlyDictionary<TextTemplateVariableType, decimal> evaluateStats)
    {
        this.talentSerializer = talentSerializer;
        this.freeTalentCalculator = freeTalentCalculator;
        this.talentCalculator = talentCalculator;
        this.talentBuildSource = talentBuildSource;

        this.Branches = talentBuildSource.Class.GetAllTalentBranches()
                                         .ToObservableCollection(branch => new TalentBranchModel(evaluateStats, this, branch));

        this.SetData(talentBuildSource);
    }


    public IClass Class => this.talentBuildSource.Class;

    public int Level => this.talentBuildSource.Level;

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
                this.SetData(this.talentSerializer.Deserialize(this.Code));
            }
            catch (System.Exception ex)
            {
                this.SetValue(v => v.Code, prevValue);

                throw new ClientException("Invalid code", ex);
            }
        }
    }

    public ObservableCollection<TalentBranchModel> Branches
    {
        get => this.GetValue(v => v.Branches);
        private set => this.SetValue(v => v.Branches, value);
    }

    public int FreeTalents { get => this.GetValue(v => v.FreeTalents); private set => this.SetValue(v => v.FreeTalents, value); }

    public IEnumerable<ITalent> ActiveTalents
    {
        get => this.Branches.SelectMany(b => b.ActiveTalents);
        private set
        {
            var newActiveDict = value.GroupBy(tal => tal.Branch).ToDictionary(pair => pair.Key);

            foreach (var g in newActiveDict)
            {
                var branchModel = this.Branches.Single(b => b.TalentBranch == g.Key);

                branchModel.ActiveTalents = g.Value.ToList();
            }

            foreach (var branchModel in this.Branches.Where(branchModel => !newActiveDict.ContainsKey(branchModel.TalentBranch)))
            {
                branchModel.ActiveTalents = [];
            }
        }
    }



    public bool CloseDialog => false;


    public void Clear() => this.UpdateOperation(() => this.ActiveTalents = []);

    public void SwitchActive(TalentModel talentModel)
    {
        if (talentModel == null) throw new ArgumentNullException(nameof(talentModel));

        this.UpdateOperation(() =>
                             {
                                 var branchTalents = talentModel.Branch.TalentList;

                                 if (talentModel.Active)
                                 {
                                     if (branchTalents.Where(model => model.HIndex > talentModel.HIndex).All(model => !model.Active))
                                     {
                                         talentModel.Active = false;
                                     }
                                 }
                                 else
                                 {
                                     var vertActiveModel = talentModel.ParallelTalents.SingleOrDefault(model => model.Active);

                                     if (vertActiveModel != null)
                                     {
                                         vertActiveModel.Active = false;

                                         talentModel.Active = true;
                                     }
                                     else
                                     {
                                         if (talentModel.HIndex == 0
                                             || branchTalents.Any(model => model.Active && model.HIndex == talentModel.HIndex - 1))
                                         {
                                             if (this.FreeTalents > 0)
                                             {
                                                 talentModel.Active = true;
                                             }
                                         }
                                     }
                                 }
                             });
    }


    private void SetData(ITalentBuildSource character)
    {
        if (character == null) throw new ArgumentNullException(nameof(character));

        this.UpdateOperation(() =>
                             {
                                 if (character.Class.GetAllParents().IsIntersected(this.Class.GetAllParents()))
                                 {
                                     this.ActiveTalents = this
                                                          .talentCalculator.GetLimitedTalents(
                                                              this.talentBuildSource.OverrideTalents(character.Talents))
                                                          .Where(tal => this.Class.GetAllTalentBranches().Contains(tal.Branch));
                                 }
                             });
    }


    protected override void InternalRecalculate()
    {
        this.FreeTalents = this.freeTalentCalculator.GetFreeTalents(this);

        var code = this.talentSerializer.Serialize(this);

        this.SetValue(v => v.Code, code);

        this.FreeTalents = this.freeTalentCalculator.GetFreeTalents(this);
    }


    IReadOnlyList<ITalent> ITalentBuildSource.Talents => this.ActiveTalents.ToList();
}