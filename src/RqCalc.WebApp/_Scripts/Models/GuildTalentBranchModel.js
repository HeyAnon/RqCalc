function GuildTalentBranchModel(context, windowModel, branch) {

    var thisModel = this;

    thisModel.branch = branch;
    thisModel.windowModel = windowModel;
    //-----------------------------------------------------------------------------------------------------------------

    thisModel.talents = branch.talents.map(function (tal) {
        return new GuildTalentModel(context, thisModel, tal);
    });


    function getPrevBranch() {

        return windowModel.branches.takeWhile(function (b) {
            return b !== thisModel;
        }).lastOrDefault();
    }

    function getNextBranch() {

        return windowModel.branches.skipWhile(function (b) {
            return b !== thisModel;
        }).skip(1).firstOrDefault();
    }

    thisModel.isMaximized = function () {
        return thisModel.talents.sum(function (t) { return t.points(); }) === branch.maxPoints;
    }

    thisModel.hasActiveTalents = function () {
        return thisModel.talents.any(function (t) {
            return t.active();
        });
    }

    thisModel.isEditable = function () {

        var prevBranch = getPrevBranch();

        if (prevBranch == null || prevBranch.isMaximized()) {

            var nextBranch = getNextBranch();

            if (nextBranch == null || !nextBranch.hasActiveTalents()) {
                return true;
            }
        }

        return false;
    }

    //-----------------------------------------------------------------------------------------------------------------
    thisModel.setActiveGuildTalents = function (talents) {

        thisModel.talents.foreach(function (talentModel) {

            var points = talents.getValueByKeyOrDefault(talentModel.talent);

            talentModel.points(points == null ? 0 : points);
        });
    };

    //-----------------------------------------------------------------------------------------------------------------
    thisModel.getActiveGuildTalents = function () {

        return thisModel.talents.filter(function (talentModel) {
            return talentModel.active();
        }).map(function (talentModel) {
            return { key: talentModel.talent, value: talentModel.points() };
        });
    };
}