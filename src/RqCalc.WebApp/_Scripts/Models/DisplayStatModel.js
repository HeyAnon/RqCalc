function DisplayStatModel(startStat) {

    var thisModel = this;

    thisModel.stat = ko.observable(startStat);
    thisModel.value = ko.observable();
    thisModel.descriptionValue = ko.observable();

    thisModel.displayValue = ko.computed(function () {

        var stat = thisModel.stat();
        var statValue = thisModel.value();

        if (statValue === undefined) {
            return undefined;
        } else {

            var value = statValue.round(stat.roundDigits == null ? (stat.isPercent ? 2 : 0) : stat.roundDigits);

            return value + (stat.isPercent ? "%" : "");
        }
    }, thisModel);

    thisModel.description = ko.computed(function () {
        
        var descriptionValue = thisModel.descriptionValue();
        var stat = thisModel.stat();

        if (stat.descriptionTemplate != null) {

            var descriptionDisplayValue = descriptionValue == null ? thisModel.value() : descriptionValue;
            
            return descriptionDisplayValue == null ? null : TextTemplate.evaluate(stat.descriptionTemplate, [descriptionDisplayValue.round(2)]);

        } else {
            return null;
        }
    }, thisModel);
}