function ElixirsWindowModel(context, $class, startupElixir) {
    
    var thisModel = this;

    $.extend(thisModel, new SwitchItemDialogModel(context.elixirs, startupElixir));


    thisModel.pureModel.selectedItem(startupElixir);
    thisModel.showLegacy = ko.observable(startupElixir != null && startupElixir.isLegacy);

    thisModel.showLegacy.subscribe(function(_) {
        refresh();
    });


    thisModel.clear = function() {
        thisModel.pureModel.selectedItem(null);
    };

    thisModel.getResult = function() {

        return thisModel.pureModel.selectedItem();
    };


    refresh();

    //---------------------------------------------------------------------

    function refresh() {

        var elixirs = context.elixirs.filter(function (elixir) {
            return !elixir.isLegacy || thisModel.showLegacy();
        }).orderBy(function(elixir) {
            return -MainHelper.Bonus.getOrderIndex(elixir, $class);
        });

        thisModel.pureModel.items(elixirs);

        if (thisModel.pureModel.selectedItem() == null) {

            thisModel.pureModel.selectedItem(elixirs.firstOrDefault());
        }
    }
}