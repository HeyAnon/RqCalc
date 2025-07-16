using Framework.Core;
using Framework.Reactive;

using RqCalc.Domain;
using RqCalc.Domain.Talent;
using RqCalc.Wpf.Models.Window.Dialog;

namespace RqCalc.Wpf.Models;

public class TalentBranchModel : NotifyModelBase
{
    public readonly TalentsWindowModel WindowModel;
    
    public TalentBranchModel(IReadOnlyDictionary<TextTemplateVariableType, decimal> evaluateStats, TalentsWindowModel windowModel, ITalentBranch talentBranch)
    {
        this.WindowModel = windowModel;
        this.TalentBranch = talentBranch;


        var talentRequest = from tal in talentBranch.Talents

            group tal by tal.HIndex into hGroup

            orderby hGroup.Key

            select hGroup;

        this.TalentMatrix = talentRequest.Reverse()
                                         .Select((g, index) => g.OrderBy(tal => tal.VIndex).ToObservableCollection(tal => new TalentModel(evaluateStats, this, index == 0, tal)))
                                         .Reverse()
                                         .ToObservableCollection();

        this.TalentList = this.TalentMatrix.SelectMany().ToObservableCollection();
    }


    public IEnumerable<ITalent> ActiveTalents
    {
        get =>
            from collection in this.TalentMatrix

            from model in collection

            where model.Active

            select model.SelectedObject;
        set
        {
            var cache = value.Pipe(Enumerable.ToHashSet);

            foreach (var collection in this.TalentMatrix)
            {
                foreach (var model in collection)
                {
                    model.Active = cache.Contains(model.SelectedObject);
                }
            }
        }
    }

    public ObservableCollection<TalentModel> TalentList
    {
        get => this.GetValue(v => v.TalentList);
        private set => this.SetValue(v => v.TalentList, value);
    }

    public ObservableCollection<ObservableCollection<TalentModel>> TalentMatrix
    {
        get => this.GetValue(v => v.TalentMatrix);
        private set => this.SetValue(v => v.TalentMatrix, value);
    }

    public ITalentBranch TalentBranch
    {
        get => this.GetValue(v => v.TalentBranch);
        private set => this.SetValue(v => v.TalentBranch, value);
    }
}