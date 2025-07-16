function ConsumablesWindowModel(context, startupConsumables) {

    var thisModel = this;

    var items = context.consumables.map(function (consumable) {

        var active = startupConsumables.contains(consumable);

        var obsActive = ko.observable(active);

        obsActive.subscribe(recalcSelectedAll);

        return $.extend({
            active: obsActive,
            baseConsumable: consumable
        }, consumable);
    });

    $.extend(thisModel, new SwitchItemDialogModel(items, null));


    thisModel.getResult = function () {

        return items.filter(function (item) {
            return item.active();
        }).map(function (item) {
            return item.baseConsumable;
        });
    }

    thisModel.selectedAll = ko.observable(getSelectedAllState());
    thisModel.selectedAll.subscribe(function (newValue) {

        if (newValue != null) {

            items.foreach(function (item) {
                item.active(newValue);
            });
        }
    });

    function recalcSelectedAll(_) {

        var newState = getSelectedAllState();

        if (newState !== thisModel.selectedAll()) {
            thisModel.selectedAll(newState);
        }
    }

    function getSelectedAllState() {
        return items.all(function (item) { return item.active() === true; }) ? true
             : items.all(function (item) { return item.active() === false; }) ? false
             : null;
    }
}