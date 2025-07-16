function GuildTalentBuildApplicationContextBase(context, data, facade) {

    $.extend(context, data);

    context.facade = facade;
    //-------------------------------
    context.guildTalentBranches = context.guildTalentBranches.map(function (guildTalentBranch) {

        return $.extend(guildTalentBranch, {
            
            talents: guildTalentBranch.talents.map(function (guildTalent) {
                return $.extend(guildTalent, {
                    branch: guildTalentBranch,
                    image: facade.getImageUrl("GuildTalent", guildTalent.id),
                    grayImage: facade.getImageUrl("GrayGuildTalent", guildTalent.id)
                });
            })
        });
    });
    //-------------------------------
    context.guildTalents = context.guildTalentBranches.selectMany(function (guildTalentBranch) {

        return guildTalentBranch.talents;
    });

    var pointLimit = context.guildTalentBranches.sum(function(b) {
        return b.maxPoints;
    });

    //-------------------------------
    context.getFreeGuildTalents = function (character) {

        var usedPoints = character.getActiveGuildTalents().sum(function(tal) {
            return tal.value;
        });

        return pointLimit - usedPoints;
    };
    //-------------------------------
    context.getLimitedGuildTalents = function (source) {
        var freeGuildTalents = context.getFreeGuildTalents(source);

        if (freeGuildTalents < 0) {
            var removingGuildTalents = source
                .talents.orderBy(function (talent) {
                    return talent.hIndex;
                })
                .orderBy(function (talent) {
                    return talent.branch.id;
                })
                .reverseC()
                .take(-freeGuildTalents);

            return source.talents.except(removingGuildTalents);
        } else {
            return source.talents;
        }
    };
    //-------------------------------
    {
        var guildTalents = context.guildTalentBranches.selectMany(function (branch) {
            return branch.talents;
        });

        context.getGuildTalentById = function (id) {
            return guildTalents.getById(id);
        }
    }
}