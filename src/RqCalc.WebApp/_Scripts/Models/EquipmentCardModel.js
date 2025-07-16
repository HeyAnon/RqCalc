function EquipmentCardModel(context, evaluateStats, index, startCard, $class, obsEquipment) {

    var thisModel = this;

    thisModel.index = index;
    thisModel.card = ko.observable(startCard);

    thisModel.image = ko.observable();
    thisModel.toolTipImage = ko.observable();

    thisModel.allowEdit = ko.observable(true);

    updateCard();

    thisModel.card.subscribe(function (_) {
        updateCard();
    });

    thisModel.switchCard = function () {

        if (!thisModel.allowEdit() || obsEquipment() == null) {
            return;
        }

        context.dialogManager.switchCard(new CardsWindowModel(context, evaluateStats, obsEquipment().type, index, $class, thisModel.card(), obsEquipment().class)).run(function (tryResult) {

            if (tryResult.isSuccess) {
                thisModel.card(tryResult.result);
            }

            return;
        });
    };

    thisModel.description = ko.computed(function () {

        var card = thisModel.card();

        return card == null || evaluateStats == null ? null : new CardDescriptionModel(context, evaluateStats, card);

    }, thisModel);

    function updateCard() {

        thisModel.toolTipImage(getToolTipImage());

        thisModel.image(getImage());
    }

    function getToolTipImage() {

        var card = thisModel.card();

        if (card == null) {
            return context.facade.getStaticImage(StaticImageType.equipmentToolTipCardEmpty);
        } else if (card.type.hasToolTipImage) {
            return card.type.toolTipImage;
        } else {
            return context.facade.getStaticImage(StaticImageType.equipmentToolTipCard);
        }
    }

    function getImage() {

        var card = thisModel.card();

        if (card == null) {
            return context.facade.getStaticImage(StaticImageType.emptyCard);
        } else {
            return card.type.image;
        }
    }
}