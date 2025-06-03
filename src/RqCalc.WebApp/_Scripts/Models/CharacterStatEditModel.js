function CharacterStatEditModel(context, rootModel, displayModel) {
    
    var thisModel = this;

    thisModel.displayModel = displayModel;
    thisModel.editValue = ko.observable();
    thisModel.displayValue = ko.observable();

    thisModel.multiplicityValue = ko.observable();
    thisModel.progressPercentValue = ko.observable();

    thisModel.multiplicity = context.settings.levelMultiplicity;
    thisModel.multiplicityValueVisible = ko.observable();
    thisModel.stat = ko.observable(displayModel.stat());
    
    thisModel.displayValue.subscribe(function (_) {

        thisModel.multiplicityValue(Math.floor(thisModel.displayValue() / thisModel.multiplicity));
        thisModel.multiplicityValueVisible(thisModel.multiplicityValue() !== 0);

        thisModel.progressPercentValue((((thisModel.displayValue() % thisModel.multiplicity) / thisModel.multiplicity) * 100).round(0));
    });

    displayModel.value.subscribe(function (newValue) {
        thisModel.displayValue(newValue);
    });

    function getLimit() {
        return context.settings.maxStatCount - thisModel.editValue();
    };

    thisModel.maximize = function() {

        var limit = getLimit();

        if (limit > 0 && rootModel.freeStats() > 0) {
            thisModel.editValue(thisModel.editValue() + Math.min(rootModel.freeStats(), limit));
        }
    };

    thisModel.increment = function () {

        var limit = getLimit();
        
         if (limit > 0 && rootModel.freeStats() > 0) {
             thisModel.editValue(thisModel.editValue() + 1);
         }
    };

    thisModel.decrement = function () {

        if (thisModel.editValue() > 1) {
            thisModel.editValue(thisModel.editValue() - 1);
        }
    };

    thisModel.description = ko.computed (function () {

        var editValue = thisModel.editValue();

        var delta = thisModel.displayValue() - editValue;

        if (delta === 0) {
            return editValue;
        } else {
            return editValue + " + " + delta;
        }

    }, thisModel);
}