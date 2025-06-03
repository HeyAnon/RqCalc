function CollectedGroupModel(context, gender, group, startupCollectedItem) {

    var thisModel = this;

    var items = group.items.filter(function (collectedItem) {
        return MainHelper.CollectedItem.isAllowed(collectedItem, gender);
    }).map(function (collectedItem) {

        var active = startupCollectedItem.contains(collectedItem);

        var obsActive = ko.observable(active);

        obsActive.subscribe(recalcSelectedAll);

        return $.extend({
            active: obsActive,
            baseCollectedItem: collectedItem
        },
            collectedItem);
    });

    thisModel.group = group;

    $.extend(thisModel, new SwitchItemDialogModel(items, null));

    thisModel.selectedAll = ko.observable(getSelectedAllState());
    thisModel.selectedAll.subscribe(function (newValue) {

        if (newValue != null) {

            items.foreach(function (item) {
                item.active(newValue);
            });

            return;
        }
    });


    thisModel.selectedCount = ko.observable();
    thisModel.selectedLimit = items.length;

    thisModel.bonuses = ko.observable();

    function recalcSelectedAll(_) {

        var newState = getSelectedAllState();

        if (newState !== thisModel.selectedAll()) {
            thisModel.selectedAll(newState);
        }

        recalcSelectedCount();
        recalcBonuses();
    }

    function getSelectedAllState() {
        return items.all(function (item) { return item.active() === true; }) ? true
            : items.all(function (item) { return item.active() === false; }) ? false
                : null;
    }

    function recalcSelectedCount() {

        var selectedItems = items.filter(function (item) {
            return item.active();
        });

        thisModel.selectedCount(selectedItems.length);
    }

    function recalcBonuses() {

        var newBonuses = items.filter(function(item) {
            return item.active();
        }).selectMany(function (item) {
            return item.bonuses;
        }).groupBy(function (bonus) {
            return bonus.type;
        }).map(function (pair) {

            var newVariables = Array.apply(null, Array(pair.key.variables.length)).map(Number.prototype.valueOf, 0);
            
            pair.value.foreach(function (bonus) {

                $.each(bonus.variables, function (index, bonusVar) {

                    newVariables[index] += bonusVar;
                });
            });

            return {
                type: pair.key,

                variables: newVariables
            }
        });

        thisModel.bonuses(context.getBonusesBody(newBonuses));
    }

    recalcSelectedCount();
    recalcBonuses();
}