function CollectedTabModel(context, groupModel, statisticModel) {
    
    var thisModel = this;

    thisModel.groupModel = groupModel;
    thisModel.statisticModel = statisticModel;

    thisModel.name = groupModel != null ? groupModel.group.name : statisticModel.statistic.name;


    thisModel.bindId = groupModel != null ? "CollectedGroup_" + groupModel.group.id : "CollectedStatistic_" + statisticModel.statistic.id;

    thisModel.isFirst = statisticModel != null && statisticModel.statistic.id === 1;
}