using Framework.Reactive;
using Framework.Reactive.ObservableRecurse;
using RqCalc.Domain;
using RqCalc.Domain.GuildTalent;
using RqCalc.Model._Extensions;
using RqCalc.Wpf.Models._Base;

namespace RqCalc.Wpf.Models;

public class GuildTalentModel : ActiveImageChangeModel<IGuildTalent>
{
    public readonly GuildTalentBranchModel Branch;


    public GuildTalentModel(GuildTalentBranchModel branch, IGuildTalent talent)

    {
        this.Branch = branch ?? throw new ArgumentNullException(nameof(branch));
        this.SelectedObject = talent ?? throw new ArgumentNullException(nameof(talent));
            
        this.SubscribeExplicit(rootRule => rootRule.Subscribe(model => model.Points, this.PointsChanged));
        this.UpdateDescription();
    }


    public TalentDescriptionModel MainDescription
    {
        get => this.GetValue(v => v.MainDescription);
        private set => this.SetValue(v => v.MainDescription, value);
    }

    public bool HasPassive { get; } = false;

    public bool HasEquipmentCondition { get; } = false;

    public int Points
    {
        get => this.GetValue(v => v.Points);
        set => this.SetValue(v => v.Points, value);
    }

    private void PointsChanged()
    {
        this.Active = this.Points > 0;

        this.UpdateDescription();
    }

    private void UpdateDescription() => this.MainDescription = new TalentDescriptionModel(new Dictionary<TextTemplateVariableType, decimal> { { TextTemplateVariableType.Const, 1 } }, this.SelectedObject.GetDescription(Math.Max(1, this.Points)));

    public void Change(bool increase) => this.Branch.WindowModel.Change(this, increase);
}