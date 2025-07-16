function EquipmentDataChangeModel(context, data) {

    var thisModel = this;

    thisModel.equipment = data.equipment;
    thisModel.equipmentBonuses = ko.observable(thisModel.equipment);

    thisModel.upgrade = data.upgrade;

    thisModel.elixirClockImage = context.facade.getStaticImage(StaticImageType.equipmentElixirClock);

    thisModel.cards = $.map(data.cards, function (card, index) {
        return new EquipmentCardModel(context, null, index, card);
    });

    thisModel.stampVariant = data.stampVariant;

    thisModel.stampColor = data.stampVariant == null ? "#FFFFFF" : data.stampVariant.color.argb;

    thisModel.hasStamp = thisModel.stampVariant != null;

    thisModel.elixir = data.elixir;
    thisModel.hasElixir = thisModel.elixir != null;


    thisModel.displayName = thisModel.upgrade === 0 ? thisModel.equipment.name : ("+" + thisModel.upgrade + " " + thisModel.equipment.name);
    
    thisModel.activate = data.equipment.isActivate;
    thisModel.active = ko.observable(!thisModel.activate || data.active);

    thisModel.resultInfo = ko.observable(null);
    thisModel.displayInfo = ko.observable();
    
    
    {
        var classes = MainHelper.Equipment.getClassConditions(thisModel.equipment);
        thisModel.classCondition = classes.any() ? classes.map(function ($class) {
            return $class.name;
        }).join(", ") : "Любой";
    }

    {
        thisModel.genderCondition = thisModel.equipment.gender == null ? "Любой" : thisModel.equipment.gender.name;
    }


    updateDisplayInfo();

    thisModel.resultInfo.subscribe(function (_) {
        updateDisplayInfo();
    });


    function updateDisplayInfo() {
        thisModel.displayInfo(getDisplayInfo());
    }


    function getDisplayInfo() {

        var info = thisModel.equipment.info;
        var resultInfo = thisModel.resultInfo();

        if (resultInfo == null || resultInfo.dynamicBonuses == null)
        {
            thisModel.equipmentBonuses(thisModel.equipment);
        }
        else {

            thisModel.equipmentBonuses({

                bonuses: thisModel.equipment.bonuses.map(function(bonus) {

                    var dynamicBonus = resultInfo.dynamicBonuses.bonuses.singleOrDefault(function(b) {
                        return b.type === bonus.type;
                    });

                    return dynamicBonus == null ? bonus : dynamicBonus;
                })
            });
        }

        var upgradeInfo = resultInfo == null ? null : resultInfo.upgrade;

        var mainDefense = info == null ? 0 : info.defense;
        var upgradeDefense = upgradeInfo == null ? 0 : upgradeInfo.defense;
        
        var displayDefense = [mainDefense, upgradeDefense].filter(function (def) {
            return def !== 0;
        }).join("+");

        //----------------------------------------------

        var mainAttack = info == null ? 0 : info.attack;
        var upgradeAttack = upgradeInfo == null ? 0 : upgradeInfo.attack;

        var displayAttack = [mainAttack, upgradeAttack].filter(function (attack) {
            return attack !== 0;
        }).join("+");

        var attackSpeed = info == null ? 0 : info.attackSpeed;

        var dps = ((mainAttack + upgradeAttack) * attackSpeed).round(2);
        
        //----------------------------------------------

        var hpBonus = upgradeInfo == null ? 0 : upgradeInfo.hp;

        //----------------------------------------------

        var allStatBonus = upgradeInfo == null ? 0 : upgradeInfo.allStatBonus;

        var hasDefense = displayDefense !== "";
        var hasAttack = displayAttack !== "";
        var hasHpBonus = hpBonus !== 0;
        var hasAllStatBonus = allStatBonus !== 0;
        var stampVariant = resultInfo == null ? null : resultInfo.stampVariant;
        
        return {

            displayDefense: displayDefense,

            displayAttack: displayAttack,

            attackSpeed: attackSpeed,

            dps: dps,

            hpBonus: hpBonus,

            allStatBonus: allStatBonus,

            hasDefense: hasDefense,

            hasAttack: hasAttack,

            hasHpBonus: hasHpBonus,

            hasAllStatBonus: hasAllStatBonus,

            stampVariant: stampVariant
        }
    }
}