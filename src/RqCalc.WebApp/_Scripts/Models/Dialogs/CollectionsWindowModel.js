function CollectionsWindowModel(context, gender, startupCollectedItems) {

    var thisModel = this;

    var groupModels = context.collectedGroups.map(function (group) {

        var groupModel = new CollectedGroupModel(context, gender, group, startupCollectedItems);

        groupModel.selectedAll.subscribe(recalcSelectedAll);

        return groupModel;
    });

    var groupTabs = groupModels.map(function (groupModel) {

        return new CollectedTabModel(context, groupModel, null);
    });

    var statisticTabs = context.collectedStatistics.map(function (statistic) {

        var statisticGroupModels = groupModels.filter(function(groupModel) {

            return groupModel.group.statistic === statistic;
        });

        var statisticModel = new CollectedStatisticModel(context, statistic, statisticGroupModels);

        return new CollectedTabModel(context, null, statisticModel);
    });

    thisModel.tabs = statisticTabs.concat(groupTabs);

    thisModel.selectedAll = ko.observable(getSelectedAllState());
    
    thisModel.selectedAll.subscribe(function (newValue) {

        if (newValue != null) {
            
            groupModels.foreach(function (g) {
                g.selectedAll(newValue);
            });
            
            return;
        }
    });

    thisModel.getResult = function () {

        return groupModels.selectMany(function (groupModel) {
            return groupModel.pureModel.items();
        }).filter(function (item) {
            return item.active();
        }).map(function (item) {
            return item.baseCollectedItem;
        });
    }

    function recalcSelectedAll(_) {

        var newState = getSelectedAllState();

        if (newState !== thisModel.selectedAll()) {
            thisModel.selectedAll(newState);
        }
    }

    function getSelectedAllState() {
        return groupModels.all(function (g) { return g.selectedAll() === true; })  ? true
             : groupModels.all(function (g) { return g.selectedAll() === false; }) ? false
             : null;
    }
}