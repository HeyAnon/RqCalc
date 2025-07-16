using Framework.Core;
using Framework.Reactive;
using RqCalc.Domain;
using RqCalc.Domain.Talent;
using RqCalc.Model._Extensions;
using RqCalc.Wpf.Models._Base;

namespace RqCalc.Wpf.Models;

public class TalentModel : ActiveImageChangeModel<ITalent>
{
    public readonly TalentBranchModel Branch;


    public TalentModel(IReadOnlyDictionary<TextTemplateVariableType, decimal> evaluateStats, TalentBranchModel branch, bool isUltimate, ITalent talent)

    {
        if (evaluateStats == null) throw new ArgumentNullException(nameof(evaluateStats));

        this.Branch = branch ?? throw new ArgumentNullException(nameof(branch));
        this.IsUltimate = isUltimate;
        this.SelectedObject = talent ?? throw new ArgumentNullException(nameof(talent));

        this.MainDescription = new TalentDescriptionModel(evaluateStats, talent.GetMainDescription());
        this.PassiveDescription = talent.GetPassiveDescription().Maybe(desc => new TalentDescriptionModel(evaluateStats, desc));
    }


    public TalentDescriptionModel MainDescription
    {
        get => this.GetValue(v => v.MainDescription);
        private set => this.SetValue(v => v.MainDescription, value);
    }


    public TalentDescriptionModel PassiveDescription
    {
        get => this.GetValue(v => v.PassiveDescription);
        private set => this.SetValue(v => v.PassiveDescription, value);
    }

    public bool IsUltimate
    {
        get => this.GetValue(v => v.IsUltimate);
        private set => this.SetValue(v => v.IsUltimate, value);
    }

    public bool HasEquipmentCondition => !string.IsNullOrWhiteSpace(this.SelectedObject.EquipmentCondition);

    public bool HasPassive => this.PassiveDescription != null;

    public int HIndex => this.SelectedObject.HIndex;

    public IEnumerable<TalentModel> ParallelTalents => this.Branch.TalentList.Where(tal => tal.HIndex == this.HIndex && tal != this);


    public void SwitchActive() => this.Branch.WindowModel.SwitchActive(this);
}