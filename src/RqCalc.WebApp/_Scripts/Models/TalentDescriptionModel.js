function TalentDescriptionModel(context, evaluateStats, talentDescription) {

    var thisModel = this;

    thisModel.body = new TextTemplateModel(context, evaluateStats, talentDescription.body);

    thisModel.buffs = talentDescription.buffs.map(function (buffTemplate) {
        return new TextTemplateModel(context, evaluateStats, buffTemplate);
    });
}