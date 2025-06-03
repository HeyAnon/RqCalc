function GuildTalentModel(context, branchModel, talent) {

    var thisModel = this;

    thisModel.active = ko.observable();

    thisModel.image = ko.observable();

    thisModel.talent = talent;

    thisModel.mainDescription = ko.observable();

    thisModel.points = ko.observable(0);

    thisModel.points.subscribe(pointsChanged);

    thisModel.branch = branchModel;
    
    updateDescription();

    function pointsChanged(points) {
        thisModel.active(points > 0);

        updateDescription();
    }

    function updateDescription() {

        var description = MainHelper.TextTemplate.getGuildTalentDescription(talent, Math.max(1, thisModel.points()));

        var newDescriptionModel = new TalentDescriptionModel(context, [{ key: TextTemplateVariableType.const, value: 1 }], description);

        thisModel.mainDescription(newDescriptionModel);    
    }
    
    thisModel.active.subscribe(function(active) {

        thisModel.image(active ? talent.image : talent.grayImage);
    });

    thisModel.active(false);
    
    thisModel.increase = function () {

        branchModel.windowModel.change(this, true);
    }

    thisModel.decrease = function () {

        branchModel.windowModel.change(this, false);
    }
}