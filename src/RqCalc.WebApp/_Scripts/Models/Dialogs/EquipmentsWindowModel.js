function EquipmentsWindowModel(context, evaluateStats, character, slot, startupData, reverseEquipment) {

    var thisModel = this;

    var reverseWeaponType = reverseEquipment != null && reverseEquipment.type.slot.isWeapon ? reverseEquipment.type : null;
    var hasReverseSlot = MainHelper.Slot.hasReverse(slot);
    var isPrimarySlot = MainHelper.Slot.isPrimary(slot);

    $.extend(thisModel, new SwitchItemDialogModel(context.equipments, startupData == null ? null : startupData.equipment));

    var obsEquipment = thisModel.pureModel.selectedItem;
    var obsEquipments = thisModel.pureModel.items;

    var allowWeapon = slot.isWeapon || (MainHelper.Slot.isExtra(slot) && character.class.allowExtraWeapon);
    var allowEquip = !slot.isWeapon && MainHelper.Class.isAllowedSlot(character.class, slot);

    thisModel.allowChangeType = allowWeapon === allowEquip;

    thisModel.isWeapon = ko.observable(!allowEquip || (startupData != null ? startupData.equipment.type.slot : slot).isWeapon);

    thisModel.cards = ko.observable([]);


    updateCards(startupData == null ? null : startupData.cards.map(function (cardModel) {
        return cardModel.card();
    }));

    thisModel.stampVariant = ko.observable(startupData == null ? null : startupData.stampVariant);
    thisModel.upgrade = ko.observable(startupData == null ? 0 : startupData.upgrade);

    thisModel.stampImage = ko.observable();
    thisModel.equipmentImage = ko.observable();



    var hasUpgrade = slot.isWeapon != null;

    thisModel.upgrades = ko.observable();

    thisModel.singleElixir = ko.observable(null);
    thisModel.elixir = ko.observable(startupData == null ? null : startupData.elixir);
    thisModel.selectedSingleEquipmentElixir = ko.observable();

    //---------------------------------------------------------------------

    thisModel.selectedSingleEquipmentElixir.subscribe(function (value) {

        thisModel.elixir(value ? thisModel.singleElixir() : null);
    });

    //---------------------------------------------------------------------

    thisModel.isWeapon.subscribe(function (_) {
        refreshEquipmentSource();
    });

    obsEquipment.subscribe(function (_) {
        updateEquipment();
    });

    thisModel.stampVariant.subscribe(function (_) {
        updateStampVariant();
    });
    //---------------------------------------------------------------------
    refreshEquipmentSource();
    updateStampVariant();
    updateUpgrades();
    //---------------------------------------------------------------------
    thisModel.switchStamp = function () {

        if (!hasUpgrade || obsEquipment() == null) {
            return;
        }

        context.dialogManager.switchStamp(new StampsWindowModel(context, obsEquipment(), character.class, thisModel.stampVariant())).run(function (tryResult) {

            if (tryResult.isSuccess) {
                thisModel.stampVariant(tryResult.result);
            }

            return;
        });


        return;
    };

    //---------------------------------------------------------------------

    function updateCards(initCards) {

        var actualSlot = getActualSlot();

        var maxCardCount = MainHelper.Settings.getMaxCardCount(context.settings, actualSlot);

        var equipment = obsEquipment();

        if (thisModel.cards().length !== maxCardCount) {

            var cardModels = thisModel.cards().slice(0);

            if (cardModels.length > maxCardCount) {
                cardModels.splice(maxCardCount - cardModels.length);
            } else {

                while (cardModels.length < maxCardCount) {
                    cardModels.push(new EquipmentCardModel(context, evaluateStats, cardModels.length, initCards == null ? null : initCards[cardModels.length], character.class, obsEquipment));
                }
            }

            thisModel.cards(cardModels);
        }

        thisModel.cards().foreach(function (cardModel) {

            var isAllowed = cardModel.card() != null && equipment != null && MainHelper.Card.isAllowedEE(cardModel.card(), equipment.type, equipment.class);

            if (!isAllowed) {

                cardModel.card(null);
            }
        });

        {
            var primaryCard = equipment == null ? null : equipment.primaryCard;

            if (primaryCard != null) {
                thisModel.cards()[0].card(primaryCard);
            }
        }
    }

    function refreshEquipmentSource() {

        var actualSlot = getActualSlot();

        var equipments = context.equipments.filter(function (equipment) {

            return MainHelper.Equipment.isAllowed(equipment, character) && equipment.type.slot === actualSlot;
        }).filter(function (equipment) {

            return !thisModel.isWeapon() || MainHelper.Equipment.isDoubleHand(equipment) || reverseWeaponType == null || equipment.type === reverseWeaponType;
        }).filter(function (equipment) {

            return !hasReverseSlot || !MainHelper.Equipment.isDoubleHand(equipment) || isPrimarySlot;
        })
            .orderBy(function (equipment) {
                return equipment.name;
            })
            .orderBy(function (equipment) {
                return -equipment.level;
            });


        obsEquipments(equipments);

        if (obsEquipment() == null) {
            obsEquipment(equipments.firstOrDefault());
        }


        {
            var elixirs = context.equipmentElixirs.filter(function (el) {
                return el.slots.contains(actualSlot);
            });

            var isSingle = elixirs.length === 1;
            
            thisModel.singleElixir(isSingle ? elixirs.single() : null);

            if (thisModel.elixir() != null && !elixirs.contains(thisModel.elixir())) {
                thisModel.elixir(null);
            }

            thisModel.selectedSingleEquipmentElixir(thisModel.elixir() != null && thisModel.elixir() === thisModel.singleElixir());
        }
    }

    function updateStampVariant() {

        var bigImage = thisModel.stampVariant() == null ? context.facade.getStaticImage(StaticImageType.emptyStamp) : thisModel.stampVariant().color.bigImage;

        thisModel.stampImage(bigImage);
    }

    function updateEquipment() {

        var equipment = obsEquipment();
        var stampVariant = thisModel.stampVariant();

        updateCards();

        if (stampVariant != null && equipment != null && context.isAllowedStamp(stampVariant.stamp, equipment, character.class) === false) {

            thisModel.stampVariant(null);
        }

        thisModel.equipmentImage(equipment == null ? null : equipment.image);

        updateUpgrades();

        if (thisModel.cards().length > 0) {
            
            var allowEditPrimaryCard = equipment == null || equipment.primaryCard == null;

            var cardModels = thisModel.cards();

            cardModels[0].allowEdit(allowEditPrimaryCard);
        }
    }

    function updateUpgrades() {

        var maxUpgradeLevel = getMaxUpgradeLevel();

        thisModel.upgrade(Math.min(thisModel.upgrade(), maxUpgradeLevel));
        thisModel.upgrades(Enumerable.range(0, maxUpgradeLevel + 1));
    }

    function getMaxUpgradeLevel() {

        var equipment = obsEquipment();

        if (!hasUpgrade || equipment == null) {
            return 0;
        }
        else if (equipment.class.maxUpgradeLevel == null) {
            return context.settings.maxUpgradeLevel;
        } else {
            return equipment.class.maxUpgradeLevel;
        }
    }

    //---------------------------------------------------------------------
    function isExtraWeapon() {

        return thisModel.isWeapon() && MainHelper.Slot.isExtra(slot);
    }

    function getActualSlot() {
        return isExtraWeapon() ? slot.primarySlot : slot;
    }

    //---------------------------------------------------------------------

    thisModel.clear = function () {

        obsEquipment(null);

        return;
    };

    thisModel.getResult = function () {

        var equipment = obsEquipment();

        if (equipment == null) {
            return null;
        } else {
            return {
                equipment: equipment,

                active: startupData != null && startupData.equipment.isActivate && startupData.active(),

                cards: thisModel.cards().map(function (cardModel) {
                    return cardModel.card();
                }),

                stampVariant: thisModel.stampVariant(),

                upgrade: thisModel.upgrade(),

                elixir: thisModel.elixir()
            };
        }
    };
    //---------------------------------------------------------------------
}