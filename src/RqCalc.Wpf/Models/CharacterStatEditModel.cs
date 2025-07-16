using Framework.Reactive;
using Framework.Reactive.ObservableRecurse;

using RqCalc.Domain;
using RqCalc.Model;

namespace RqCalc.Wpf.Models;

public class CharacterStatEditModel : NotifyModelBase
{
    private readonly ApplicationSettings settings;
    private readonly CharacterChangeModel rootModel;

    public CharacterStatEditModel(
        ApplicationSettings settings,
        CharacterChangeModel rootModel)
    {
        this.settings = settings;
        this.rootModel = rootModel;

        this.SubscribeExplicit(rule => rule.Subscribe(model => model.DisplayValue, () =>
        {
            this.MultiplicityValue = this.DisplayValue / this.Multiplicity;
            this.MultiplicityValueVisible = this.MultiplicityValue != 0;
            this.ProgressValue = this.DisplayValue % this.Multiplicity;
        }));
    }

    public void SetMaxStat()
    {
        var limit = this.settings.MaxStatCount - this.EditValue;

        if (limit > 0)
        {
            this.EditValue += Math.Min(this.rootModel.FreeStats, limit);
        }
    }

    public void TryIncreaseStat()
    {
        var limit = this.settings.MaxStatCount - this.EditValue;

        if (limit > 0 && this.rootModel.FreeStats > 0)
        {
            this.EditValue++;
        }
    }

    public void TryDecreaseStat()
    {
        if (this.EditValue > 1)
        {
            this.EditValue--;
        }
    }

    public int EditValue
    {
        get => this.GetValue(v => v.EditValue);
        set => this.SetValue(v => v.EditValue, value);
    }

    public int DisplayValue
    {
        get => this.GetValue(v => v.DisplayValue);
        set => this.SetValue(v => v.DisplayValue, value);
    }

    public int Multiplicity => this.settings.LevelMultiplicity;

    public int MultiplicityValue
    {
        get => this.GetValue(v => v.MultiplicityValue);
        private set => this.SetValue(v => v.MultiplicityValue, value);
    }

    public int ProgressValue
    {
        get => this.GetValue(v => v.ProgressValue);
        private set => this.SetValue(v => v.ProgressValue, value);
    }

    public bool MultiplicityValueVisible
    {
        get => this.GetValue(v => v.MultiplicityValueVisible);
        private set => this.SetValue(v => v.MultiplicityValueVisible, value);
    }


    public IStat Stat
    {
        get => this.GetValue(v => v.Stat);
        set => this.SetValue(v => v.Stat, value);
    }
}