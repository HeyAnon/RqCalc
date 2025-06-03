function CardDescriptionModel(context, evaluateStats, card) {

    var thisModel = this;

    thisModel.card = card;
    thisModel.buffs = card.buffDescriptions.map(function (buffTemplate) {
        return new TextTemplateModel(context, evaluateStats, buffTemplate);
    });
}