function TalentsWindowModel(context, evaluateStats, startupSource, isReadonly) {

    var thisModel = this;

    $.extend(thisModel, new UpdateCodeModel(thisModel, context.facade.calcTalentBuild, context.facade.decryptTalentBuild));
    
    thisModel.class = ko.observable();
    thisModel.level = ko.observable();

    thisModel.freeTalents = ko.observable();

    thisModel.branches = ko.observable([]);
    
    thisModel.minLevel = ko.observable();
    thisModel.maxLevel = ko.observable();
    thisModel.classes = ko.observableArray(context.classes);

    thisModel.isReadonly = isReadonly;

    if (!isReadonly) {

        thisModel.class.subscribe(thisModel.handleUpdate);
        
        thisModel.level.subscribe(thisModel.handleUpdate);
    }

    //-----------------------------------------------------------------------------------------------------------------
    thisModel.class.subscribe(function (_) {

        thisModel.updateOperation(function () {

            updateCharacterClass();
        });
    });

    thisModel.class.subscribe(function (_) {

        thisModel.updateOperation(function () {

            thisModel.recalculate();
        });
    });

    thisModel.level.subscribe(function (_) {

        thisModel.updateOperation(function () {

            thisModel.recalculate();
        });

        return;
    });

    //-----------------------------------------------------------------------------------------------------------------
    function getActiveTalents() {

        return thisModel.branches().selectMany(function (branchModel) {
            return branchModel.getActiveTalents();
        });
    };
    //-----------------------------------------------------------------------------------------------------------------
    function setActiveTalents(talents) {

        var newActiveDict = talents.groupBy(function (talent) {
            return talent.branch;
        });

        var newActiveBranches = newActiveDict.getKeys();

        newActiveDict.foreach(function (talentGroup) {

            var branchModel = thisModel.branches()
                .single((function (m) {
                    return m.branch === talentGroup.key;
                }));

            branchModel.setActiveTalents(talentGroup.value);
        });

        thisModel.branches()
            .filter(function (branchModel) {
                return !newActiveBranches.contains(branchModel.branch);
            })
            .foreach(function (branchModel) {
                branchModel.setActiveTalents([]);
            });
    };

    //-----------------------------------------------------------------------------------------------------------------
    function updateCharacterClass() {

        thisModel.minLevel(thisModel.class().minLevel);
        thisModel.maxLevel(thisModel.class().maxLevel);

        thisModel.level(thisModel.level().minMax(thisModel.minLevel(), thisModel.maxLevel()));
    }

    //-----------------------------------------------------------------------------------------------------------------
    function updateBranches() {

        var newBranches = thisModel.class().talentBranches.map(function (branch) {

            var prevBranch = thisModel.branches().singleOrDefault(function (branchModel) {
                return branchModel.branch === branch;
            });

            if (prevBranch != null) {
                return prevBranch;
            } else {

                var newBranch = new TalentBranchModel(context, evaluateStats, thisModel, branch);

                newBranch.getTalentModels().foreach(function (talModel) {

                    talModel.active.subscribe(thisModel.handleUpdate);
                });

                return newBranch;
            }
        });

        thisModel.branches(newBranches);
    }

    //-----------------------------------------------------------------------------------------------------------------
    function updateTalents() {

        var classTalents = getActiveTalents().filter(function (tal) {

            return MainHelper.Class.isSubsetOf(thisModel.class(), tal.branch.class);
        });

        var limitedTalents = context.getLimitedTalents($.extend ({
            talents: classTalents
        }, thisModel.unwrap()));

        setActiveTalents(limitedTalents);
    }

    //-----------------------------------------------------------------------------------------------------------------
    thisModel.maximizeLevel = function () {

        thisModel.level(thisModel.maxLevel());
    };

    //-----------------------------------------------------------------------------------------------------------------
    thisModel.clear = function () {

        thisModel.updateOperation(function () {

            setActiveTalents([]);
        });
    };

    //-----------------------------------------------------------------------------------------------------------------
    thisModel.unwrap = function () {
        return {
            "class": thisModel.class(),
            level: thisModel.level(),
            talents: getActiveTalents()
        };
    };

    //-----------------------------------------------------------------------------------------------------------------
    thisModel.recalculate = function () {

        updateCharacterClass();
        updateBranches();
        updateTalents();

        thisModel.freeTalents(context.getFreeTalents(thisModel.unwrap()));
    };

    //-----------------------------------------------------------------------------------------------------------------
    thisModel.setData = function (talentBuild) {

        thisModel.updateOperation(function () {
            
            var $class = context.getClass(talentBuild.class.id);

            var talents = $class.talentBranches.selectMany(function (tb) {
                return tb.talents;
            });

            var activeTalents = talentBuild.talents.map(function (talent) {
                return talents.getById(talent.id);
            });

            if (isReadonly && thisModel.class() != null) {
                
                var limitedTalents = context.getLimitedTalents({
                    "class": $class,

                    level: thisModel.level(),

                    talents: activeTalents
                });

                var actualTalents = limitedTalents.filter(function (tal) {
                    return thisModel.class().talentBranches.contains(tal.branch);
                });

                setActiveTalents(actualTalents);

            } else {
                
                thisModel.class($class);
                thisModel.level(talentBuild.level);

                updateBranches();

                setActiveTalents(activeTalents);
            }

        }, false);
    };

    //-----------------------------------------------------------------------------------------------------------------
    thisModel.getCalcData = function () {

        return {
            "class": { id: thisModel.class().id },

            level: thisModel.level(),

            talents: getActiveTalents()
                    .map(function (talent) {
                        return { id: talent.id };
                    })
        }
    };
    //-----------------------------------------------------------------------------------------------------------------
    thisModel.getResultCode = function(code) {
        return code;
    };
    //-----------------------------------------------------------------------------------------------------------------
    thisModel.getResult = function () {

        return getActiveTalents();
    };
    //-----------------------------------------------------------------------------------------------------------------
    thisModel.setData(startupSource);
}