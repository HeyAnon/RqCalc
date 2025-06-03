function TextTemplateModel(context, evaluateStats, textTemplate) {

    var thisModel = this;

    thisModel.header = textTemplate.header;

    thisModel.message = MainHelper.TextTemplate.evaluateMessage(textTemplate, evaluateStats);
}