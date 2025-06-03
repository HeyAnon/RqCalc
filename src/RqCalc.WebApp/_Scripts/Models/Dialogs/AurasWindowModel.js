function SharedAurasModel(context, startupSharedAuras) {

    var thisModel = this;
    
    var items = context.getSharedAuras().map(function (aura) {

        var withTalents = startupSharedAuras.getValueByKeyOrDefault(aura);

        var active = withTalents != null;
        
        var item = $.extend({
            active: ko.observable(active),
            baseAura: aura,
            withTalents: ko.observable(withTalents == null ? true : withTalents),
            sharedBonuses: ko.observable()
        }, aura);

        function recalcSharedBonuses() {

            var sharedBonusContainer = {
                bonuses: item.withTalents() ? aura.sharedBonusesWithTalents : aura.sharedBonusesWithoutTalents
            };

            item.sharedBonuses(sharedBonusContainer);
        }

        item.withTalents.subscribe(function(_) {
            recalcSharedBonuses();
        });

        recalcSharedBonuses();

        return item;
    });

    $.extend(thisModel, new SwitchItemDialogModel(items, null));
}

function PrimaryAuraModel(context, $class, currentAura, currentLevel) {

    var thisModel = this;

    var auras = MainHelper.Class.getAuras($class, currentLevel);
    var startAura = currentAura == null ? auras.firstOrDefault() : currentAura;

    $.extend(thisModel, new SwitchItemDialogModel(auras, startAura));
    //---------------------------------------------------------------------
}


function AurasWindowModel(context, $class, currentAura, currentLevel, startupSharedAuras) {
    
    var thisModel = this;

    thisModel.primaryAura = new PrimaryAuraModel(context, $class, currentAura, currentLevel);

    thisModel.sharedAuras = new SharedAurasModel(context, startupSharedAuras);


    thisModel.getResult = function() {

        var primaryAura = thisModel.primaryAura.pureModel.selectedItem();

        var sharedAuras = thisModel.sharedAuras.pureModel.items().filter(function (item) {
            return item.active();
        }).map(function(item) {
            return {
                key: item.baseAura,
                value: item.withTalents()
            };
        });
        

        return {

            primaryAura: primaryAura,

            sharedAuras: sharedAuras
        }
    };
    //---------------------------------------------------------------------
}