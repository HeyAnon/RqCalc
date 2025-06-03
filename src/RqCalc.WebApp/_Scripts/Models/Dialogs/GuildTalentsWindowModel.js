function GuildTalentsWindowModel(context, startupSource) {

    var thisModel = this;

    $.extend(thisModel, new UpdateCodeModel(thisModel, context.facade.calcGuildTalentBuild, context.facade.decryptGuildTalentBuild));

    thisModel.freeGuildTalents = ko.observable();

    thisModel.branches =

        context.guildTalentBranches.map(function (branch) {
            return new GuildTalentBranchModel(context, thisModel, branch);
        });

    //-----------------------------------------------------------------------------------------------------------------
    function setActiveGuildTalents(talents) {

        var newActiveDict = talents.groupBy(function (pair) {
            return pair.key.branch;
        });

        newActiveDict.foreach(function (talentGroup) {

            var branchModel = thisModel.branches.single(function (branchModel) {
                return branchModel.branch === talentGroup.key;
            });

            branchModel.setActiveGuildTalents(talentGroup.value);
        });

        thisModel.branches.filter(function (branchModel) {

            return !newActiveDict.containsKey(branchModel.branch);
        }).foreach(function (branchModel) {

            branchModel.setActiveGuildTalents([]);
        });
    };
    //-----------------------------------------------------------------------------------------------------------------
    thisModel.getActiveGuildTalents = function () {

        return thisModel.branches.selectMany(function (branchModel) {
            return branchModel.getActiveGuildTalents();
        });
    };
    //-----------------------------------------------------------------------------------------------------------------
    thisModel.clear = function () {

        thisModel.updateOperation(function () {

            setActiveGuildTalents([]);
        });
    };

    //-----------------------------------------------------------------------------------------------------------------
    thisModel.recalculate = function () {

        thisModel.freeGuildTalents(context.getFreeGuildTalents(thisModel));
    };

    //-----------------------------------------------------------------------------------------------------------------
    thisModel.setData = function (talentBuild) {

        thisModel.updateOperation(function () {

            var talents = talentBuild.guildTalents.changeKey(function(guidTalentIdentity) {

                return context.getGuildTalentById(guidTalentIdentity.id);
            });

            setActiveGuildTalents(talents);

        }, false);
    };

    //-----------------------------------------------------------------------------------------------------------------
    thisModel.getCalcData = function () {

        return {

            guildTalents: thisModel.getActiveGuildTalents()
                .map(function (pair) {
                    return { key: { id: pair.key.id }, value: pair.value };
                })
        }
    };
    //-----------------------------------------------------------------------------------------------------------------
    thisModel.getResultCode = function (code) {
        return code;
    };
    //-----------------------------------------------------------------------------------------------------------------
    thisModel.getResult = function () {

        return thisModel.getActiveGuildTalents();
    };
    //-----------------------------------------------------------------------------------------------------------------
    thisModel.setData(startupSource);

    thisModel.change = function (talentModel, increase) {

        thisModel.updateOperation(function () {

            var branchModel = talentModel.branch;

            var prevActiveTalent = branchModel.talents.except([talentModel]).singleOrDefault(function (tal) {
                return tal.active();
            });

            if (prevActiveTalent != null) {
                talentModel.points(prevActiveTalent.points());
                prevActiveTalent.points(0);
            } else {
                if (branchModel.isEditable()) {
                    if (increase) {
                        talentModel.points(talentModel.points() === branchModel.branch.maxPoints ? 0 : talentModel.points() + 1);
                    }
                    else {
                        talentModel.points(talentModel.points() === 0 ? branchModel.branch.maxPoints : talentModel.points() - 1);
                    }
                }
            }
        });
    }
}