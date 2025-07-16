function CharacterWindowModel(context, setDefault) {

    var thisModel = this;

    thisModel.dialogManager = context.dialogManager;

    thisModel.genders = ko.observableArray(context.genders);
    thisModel.classes = ko.observableArray(context.classes);
    thisModel.events = ko.observableArray(context.events);
    thisModel.states = ko.observableArray(context.states);

    thisModel.character = ko.observable();
    
    thisModel.setCharacter = function (characterData) {

        thisModel.character(new CharacterChangeModel(context, characterData));
    }

    thisModel.setDefaultCharacter = function () {

        thisModel.setCharacter(context.getDefaultCharacter());
    }

    thisModel.trySetDefaultCharacter = function () {

        context.dialogManager.askNewChar().run(function (tryResult) {
            
            if (tryResult.isSuccess) {
                thisModel.setDefaultCharacter();
            }
        });
    }

    thisModel.showAbout = function () {

        context.dialogManager.showAbout().run(function (_) {

        });
    }

    if (setDefault) {
        thisModel.setDefaultCharacter();
    }
}