function TalentBuildApplicationContextBase(context, data, facade) {

    $.extend(context, data);

    context.facade = facade;
    //-------------------------------
    context.getClass = function (classId) {
        return context.classes.getById(classId);
    };
    //-------------------------------
    context.getFreeTalents = function (character) {

        var baseAvailableTalents = Math.floor(Math.min(character.level, context.settings.maxTalentLevel) / context.settings.talentLevelMultiplicity);

        var allAvailableTalents = baseAvailableTalents + character.class.talentCount;

        var usedTalents = character.talents.length;

        return allAvailableTalents - usedTalents;
    };
    //-------------------------------
    context.getLimitedTalents = function (source) {
        var freeTalents = context.getFreeTalents(source);

        if (freeTalents < 0) {
            var removingTalents = source
                .talents.orderBy(function (talent) {
                    return talent.hIndex;
                })
                .orderBy(function (talent) {
                    return talent.branch.id;
                })
                .reverseC()
                .take(-freeTalents);

            return source.talents.except(removingTalents);
        } else {
            return source.talents;
        }
    };
    //-------------------------------
    context.classes = context.classes.map(function ($class) {

        var talBranches = $class.talentBranches.map(function (talentBranch) {
            return $.extend(talentBranch, {
                "class": $class,
                talents: talentBranch.talents.map(function (talent) {
                    return $.extend(talent, {
                        image: facade.getImageUrl("Talent", talent.id),
                        grayImage: facade.getImageUrl("GrayTalent", talent.id),
                        branch: talentBranch
                    });
                })
            });
        });

        return $.extend($class, {

            image: facade.getImageUrl("Class", $class.id),

            parent: context.classes.getByIdOrDefault($class.parent.id),

            talentBranches: talBranches
        });
    }).map(function ($class) {

        var offset = MainHelper.Class.getLevelOffset($class);

        return $.extend($class, {

            offset: offset,

            offsetName: new Array(offset * 2).join("\u00A0") + $class.name,

            children: context.classes.filter(function (c) {
                return c.parent === $class;
            })
        });
    }).map(function ($class) {

        var parents = ObjHelper.getAllParents($class).reverseC();

        return $.extend($class, {

            talentBranches: parents.selectMany(function (c) {
                return c.talentBranches;
            })
        });
    });
}