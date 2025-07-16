function TalentModel(context, evaluateStats, branchModel, talent, isUltimate) {

    var thisModel = this;

    thisModel.isUltimate = isUltimate;

    thisModel.active = ko.observable();

    thisModel.image = ko.observable();

    thisModel.hIndex = talent.hIndex;

    thisModel.talent = talent;


    thisModel.mainDescription = new TalentDescriptionModel(context, evaluateStats, talent.mainDescription);
    
    thisModel.passiveDescription = talent.passiveDescription == null ? null : new TalentDescriptionModel(context, evaluateStats, talent.passiveDescription);



    var freeTalents = branchModel.window.freeTalents;


    thisModel.active.subscribe(function(active) {

        thisModel.image(active ? talent.image : talent.grayImage);
    });

    thisModel.active(false);
    

    thisModel.switchActive = function () {

        var branchTalents = branchModel.talentMatrix.selectMany();

        if (thisModel.active()) {

            var canDeactivate = branchTalents.filter(function (model) {
                return model.hIndex > thisModel.hIndex;
            }).all(function (model) {
                return !model.active();
            });

            if (canDeactivate) {
                thisModel.active(false);
            }
        } else {

            var vertActiveModel = branchTalents.singleOrDefault(function (model) {
                return model.active() && model.hIndex === thisModel.hIndex;
            });

            if (vertActiveModel) {

                vertActiveModel.active(false);
                thisModel.active(true);
            } else {

                var canActivate = freeTalents() > 0 && (thisModel.hIndex === 0 || branchTalents.any(function (model) {
                    return model.active() && model.hIndex === thisModel.hIndex - 1;
                }));

                if (canActivate) {

                    thisModel.active(true);
                }
            }
        }
    }
}