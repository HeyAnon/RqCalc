using Framework.Core;
using Framework.Reactive;
using Framework.Reactive.ObservableRecurse;

using RqCalc.Application.ImageSource;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain.Card;

namespace RqCalc.Wpf.Models;

public class EquipmentCardModel : NotifyModelBase
{
    private readonly IImageSourceService imageSourceService;

    public EquipmentCardModel(IImageSourceService imageSourceService, int index)
    {
        this.imageSourceService = imageSourceService;
        this.Index = index;

        this.SubscribeExplicit(rule => rule.Subscribe(model => model.Card, this.UpdateCard));

        this.UpdateCard();
    }

    public int Index
    {
        get => this.GetValue(v => v.Index);
        set => this.SetValue(v => v.Index, value);
    }

    public ICard? Card
    {
        get => this.GetValue(v => v.Card);
        set => this.SetValue(v => v.Card, value);
    }

    public bool HasCard
    {
        get => this.GetValue(v => v.HasCard);
        private set => this.SetValue(v => v.HasCard, value);
    }

    public IImage? Image
    {
        get => this.GetValue(v => v.Image);
        private set => this.SetValue(v => v.Image, value);
    }

    public IImage? ToolTipImage
    {
        get => this.GetValue(v => v.ToolTipImage);
        private set => this.SetValue(v => v.ToolTipImage, value);
    }


    private void UpdateCard()
    {
        this.HasCard = this.Card != null;

        this.ToolTipImage = this.Card == null
                                ? this.imageSourceService.GetStaticImage(StaticImageType.EquipmentToolTipCardEmpty)
                                : this.Card.Type.ToolTipImage ?? this.imageSourceService.GetStaticImage(StaticImageType.EquipmentToolTipCard);

        this.Image = this.Card.Maybe(card => card.Type.Image) ?? this.imageSourceService.GetStaticImage(StaticImageType.EmptyCard);
    }
}