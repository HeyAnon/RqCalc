function TalentBranchModel(context, evaluateStats, windowModel, branch) {

    var thisModel = this;

    thisModel.branch = branch;
    thisModel.window = windowModel;
    //-----------------------------------------------------------------------------------------------------------------
    thisModel.talentMatrix = branch.talents.groupBy(function(tal) {
            return tal.hIndex;
        })
        .orderBy(function(g) {
            return g.key;
        })
        .reverseC()
        .map(function(g, index) {

            return g.value.orderBy(function(tal) {
                    return tal.vIndex;
                })
                .map(function(tal) {
                    return new TalentModel(context, evaluateStats, thisModel, tal, index === 0);
                });
        })
        .reverseC();

    //-----------------------------------------------------------------------------------------------------------------
    thisModel.getTalentModels = function () {

        return thisModel.talentMatrix.selectMany(function(talentColumn) {
            return talentColumn;
        });
    }

    //-----------------------------------------------------------------------------------------------------------------
    thisModel.setActiveTalents = function(talents) {

        thisModel.getTalentModels()
            .foreach(function(talentModel) {
                talentModel.active(talents.contains(talentModel.talent));
            });
    };
    //-----------------------------------------------------------------------------------------------------------------
    thisModel.getActiveTalents = function() {

        return thisModel.getTalentModels()
            .filter(function(talentModel) {
                return talentModel.active();
            })
            .map(function(talentModel) {
                return talentModel.talent;
            });
    };
}