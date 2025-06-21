using Framework.Reactive;
using Framework.Reactive.ObservableRecurse;
using RqCalc.Domain;

namespace RqCalc.Wpf.Models;

public class CharacterStatEditModel : NotifyModelBase
{
    public CharacterStatEditModel(CharacterChangeModel rootModel)

    {
        if (rootModel == null) throw new ArgumentNullException(nameof(rootModel));

        this.RootModel = rootModel;
            
        this.SubscribeExplicit(
            rule => rule.Subscribe(model => model.DisplayValue, () =>
            {
                this.MultiplicityValue = this.DisplayValue / this.Multiplicity;
                this.MultiplicityValueVisible = this.MultiplicityValue != 0;
                this.ProgressValue = this.DisplayValue % this.Multiplicity;
            }));
    }

    public CharacterChangeModel RootModel
    {
        get { return this.GetValue(v => v.RootModel); }
        private set { this.SetValue(v => v.RootModel, value); }
    }

    public int EditValue
    {
        get { return this.GetValue(v => v.EditValue); }
        set { this.SetValue(v => v.EditValue, value); }
    }

    public int DisplayValue
    {
        get { return this.GetValue(v => v.DisplayValue); }
        set { this.SetValue(v => v.DisplayValue, value); }
    }

    public int Multiplicity => this.Context.Settings.LevelMultiplicity;

    public int MultiplicityValue
    {
        get { return this.GetValue(v => v.MultiplicityValue); }
        private set { this.SetValue(v => v.MultiplicityValue, value); }
    }

    public int ProgressValue
    {
        get { return this.GetValue(v => v.ProgressValue); }
        private set { this.SetValue(v => v.ProgressValue, value); }
    }

    public bool MultiplicityValueVisible
    {
        get { return this.GetValue(v => v.MultiplicityValueVisible); }
        private set { this.SetValue(v => v.MultiplicityValueVisible, value); }
    }


    public IStat Stat
    {
        get { return this.GetValue(v => v.Stat); }
        set { this.SetValue(v => v.Stat, value); }
    }
}