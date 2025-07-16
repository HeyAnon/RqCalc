using Framework.Core;
using Framework.DataBase;
using Framework.Reactive;
using Framework.Reactive.ObservableRecurse;

using RqCalc.Application;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain._Extensions;
using RqCalc.Domain.Equipment;
using RqCalc.Domain.Stamp;
using RqCalc.Wpf.Models.Window.Dialog._Base;

namespace RqCalc.Wpf.Models.Window.Dialog;

public class StampWindowModel : NotifyModelBase, IClearModel, ILegacyModel
{
    private readonly IClass currentClass;

    private readonly IStampColor bestColor;

    private readonly IReadOnlyDictionary<IStampVariant, bool> baseVariants;


    public StampWindowModel(
        IDataSource<IPersistentDomainObjectBase> dataSource,
        IStampService stampService,
        IEquipment equipment,
        IClass currentClass,
        IStampVariant? startupStampVariant)
    {
        this.currentClass = currentClass ?? throw new ArgumentNullException(nameof(currentClass));

        this.StampColors = dataSource.GetFullList<IStampColor>().OrderById().ToObservableCollection();

        this.bestColor = this.StampColors.Last();

        this.StampVariant = startupStampVariant;

        {
            var baseVariantsRequest = from stamp in dataSource.GetFullList<IStamp>()

                                      let allowed = stampService.IsAllowedStamp(stamp, equipment, currentClass)

                                      where allowed != false

                                      let variant = stamp.GetByColor(this.bestColor)

                                      select variant.ToKeyValuePair(allowed == true);

            this.baseVariants = baseVariantsRequest.ToDictionary();
        }

        this.StampColor ??= this.bestColor;

        this.ShowLegacy = this.Stamp.Maybe(stamp => stamp.IsLegacy);
        this.ShowShared = this.Stamp.Maybe(stamp => this.baseVariants.Any(pair => pair.Key.Stamp == stamp && !pair.Value));

        this.Refresh();

        this.SubscribeExplicit(rule => rule.Subscribe(v => v.ShowLegacy, this.Refresh));
        this.SubscribeExplicit(rule => rule.Subscribe(v => v.ShowShared, this.Refresh));
    }


    public bool HasLegacy => this.baseVariants.Keys.Any(v => v.Stamp.IsLegacy);

    public bool HasShared => this.baseVariants.Values.Any(v => !v);

    public bool ShowShared { get => this.GetValue(v => v.ShowShared); set => this.SetValue(v => v.ShowShared, value); }

    public ObservableCollection<IStampVariant> DesignStampVariants
    {
        get => this.GetValue(v => v.DesignStampVariants);
        private set => this.SetValue(v => v.DesignStampVariants, value);
    }

    public ObservableCollection<IStampColor> StampColors
    {
        get => this.GetValue(v => v.StampColors);
        private set => this.SetValue(v => v.StampColors, value);
    }


    public IStampVariant? DesignStampVariant
    {
        get => this.GetValue(v => v.DesignStampVariant);
        set => this.SetValue(v => v.DesignStampVariant, value);
    }

    public IStampColor? StampColor { get => this.GetValue(v => v.StampColor); set => this.SetValue(v => v.StampColor, value); }


    public IStamp? Stamp
    {
        get => this.DesignStampVariant.Maybe(sv => sv.Stamp);
        set => this.DesignStampVariant = value.Maybe(stamp => stamp.GetByColor(this.bestColor));
    }

    public IStampVariant? StampVariant
    {
        get => this.Stamp.Maybe(s => s.GetByColor(this.StampColor!));
        set
        {
            this.Stamp = value.Maybe(v => v.Stamp);
            this.StampColor = value.Maybe(v => v.Color, this.bestColor);
        }
    }

    public bool ShowLegacy { get => this.GetValue(v => v.ShowLegacy); set => this.SetValue(v => v.ShowLegacy, value); }

    public bool CloseDialog { get; } = true;

    public void Clear() => this.Stamp = null;

    private void Refresh()
    {
        var request = from pair in this.baseVariants

                      let variant = pair.Key

                      where pair.Value || this.ShowShared

                      where !variant.Stamp.IsLegacy || this.ShowLegacy

                      orderby variant.GetOrderIndex(this.currentClass) descending

                      select variant;

        this.DesignStampVariants = request.ToObservableCollection();

        if (this.DesignStampVariant == null)
        {
            this.DesignStampVariant = this.DesignStampVariants.FirstOrDefault();
        }
    }
}