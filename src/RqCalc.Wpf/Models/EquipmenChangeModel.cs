using Framework.Core;
using Framework.Reactive;
using Framework.Reactive.ObservableRecurse;

using RqCalc.Domain._Base;
using RqCalc.Domain._Extensions;
using RqCalc.Model;

namespace RqCalc.Wpf.Models;

public class EquipmentChangeModel : NotifyModelBase, IImageObject
{
    private readonly IModelFactory modelFactory;

    public EquipmentChangeModel(
        IModelFactory modelFactory,
        CharacterEquipmentIdentity identity)
    {
        this.modelFactory = modelFactory;
        this.Identity = identity;

        this.SubscribeExplicit(rule => rule.Subscribe(model => model.DataModel, this.DataModelChanged));

        this.UpdateImageObject();
    }

    public CharacterEquipmentIdentity Identity
    {
        get => this.GetValue(v => v.Identity);
        private set => this.SetValue(v => v.Identity, value);
    }

    public EquipmentChangeModel? ReverseModel
    {
        get => this.GetValue(v => v.ReverseModel);
        set => this.SetValue(v => v.ReverseModel, value);
    }

    public ICharacterEquipmentData? Data
    {
        get => this.DataModel;
        set => this.DataModel = value.Maybe(v => this.modelFactory.CreateEquipmentDataModel(v));
    }

    public EquipmentDataChangeModel? DataModel
    {
        get => this.GetValue(v => v.DataModel);
        private set => this.SetValue(v => v.DataModel, value);
    }

    public EquipmentDataChangeModel ToolTipDataModel
    {
        get => this.GetValue(v => v.ToolTipDataModel);
        private set => this.SetValue(v => v.ToolTipDataModel, value);
    }

    public IImage? Image
    {
        get => this.GetValue(v => v.Image);
        private set => this.SetValue(v => v.Image, value);
    }

    public IImage? ElementImage
    {
        get => this.GetValue(v => v.ElementImage);
        private set => this.SetValue(v => v.ElementImage, value);
    }

    public bool HasData
    {
        get => this.GetValue(v => v.HasData);
        private set => this.SetValue(v => v.HasData, value);
    }

    public bool HasToolTipData
    {
        get => this.GetValue(v => v.HasToolTipData);
        private set => this.SetValue(v => v.HasToolTipData, value);
    }



    public bool IsAllowed
    {
        get => this.GetValue(v => v.IsAllowed);
        set => this.SetValue(v => v.IsAllowed, value);
    }

    public bool IsReverse
    {
        get => this.GetValue(v => v.IsReverse);
        private set => this.SetValue(v => v.IsReverse, value);
    }

    public bool IsDoubleHand => this.DataModel.Maybe(data => data.Equipment.IsDoubleHand());

    private void UpdateImageObject()
    {
        this.Image = this.GetImageObject().Maybe(obj => obj.Image);

        this.ElementImage = this.GetElementImage();
    }

    private IImageObject? GetImageObject() => (IImageObject?)this.GetActualDataModel().Maybe(model => model.Equipment) ?? this.Identity.Slot;

    private IImage? GetElementImage() => this.GetActualDataModel().Maybe(e => e.CardList.FirstOrDefault()).Maybe(cardModel => cardModel.Card).Maybe(card => card.Type.ToolTipImage);

    private EquipmentDataChangeModel GetActualDataModel() => this.IsReverse ? this.ReverseModel.DataModel : this.DataModel;

    private void DataModelChanged()
    {
        this.UpdateSlot();

        if (this.Identity.Slot.IsPrimarySlot())
        {
            this.ReverseModel.UpdateSlot();
        }

        this.HasData = this.DataModel != null;
    }

    private void UpdateSlot()
    {
        this.IsReverse = this.Identity.Slot.IsExtraSlot() && this.ReverseModel.Maybe(rm => rm.IsDoubleHand);

        this.UpdateToolTip();
    }

    private void UpdateToolTip()
    {
        this.ToolTipDataModel = this.GetActualDataModel();
        this.HasToolTipData = this.ToolTipDataModel != null;

        this.UpdateImageObject();
    }
}