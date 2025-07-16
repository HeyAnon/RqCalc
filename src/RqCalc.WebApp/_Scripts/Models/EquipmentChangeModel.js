function EquipmentChangeModel(context, character, identity) {

    var thisModel = this;
    
    thisModel.data = ko.observable(null);

    thisModel.image = ko.observable();
    
    
    thisModel.hasData = ko.observable(false);

    thisModel.isReverse = ko.observable(false);

    thisModel.isAllowed = ko.observable();

    thisModel.toolTipData = ko.observable(null);

    thisModel.hasToolTipData = ko.observable(false);

    thisModel.elementImage = ko.observable();
    

    thisModel.identity = identity;

    //--------------------------------------------
    
    thisModel.data.subscribe(function (_) {

        thisModel.updateSlot();
            
        if (MainHelper.Slot.isPrimary (thisModel.identity.slot))
        {
            thisModel.reverseModel.updateSlot();
        }

        thisModel.hasData(thisModel.data() != null);
    });

    //--------------------------------------------

    thisModel.isDoubleHand = function() {

        var data = thisModel.data();

        return data != null && MainHelper.Equipment.isDoubleHand(data.equipment);
    };

    var isExtra = MainHelper.Slot.isExtra(thisModel.identity.slot);

    thisModel.updateSlot = function() {

        thisModel.isReverse(isExtra && thisModel.reverseModel != null && thisModel.reverseModel.isDoubleHand());

        updateToolTip();
    };
    
    thisModel.setPureData = function(newData) {

        if (newData === null) {

            thisModel.data(null);

        } else {

            thisModel.data(new EquipmentDataChangeModel(context, newData));
        }
    };

    thisModel.switchEquipments = function() {
        
        character.getEquipmentEditModel(thisModel).internalSwitchEquipments();
    };

    thisModel.internalSwitchEquipments = function () {
        
        var reverseEquipment = thisModel.reverseModel != null && thisModel.reverseModel.data() != null ? thisModel.reverseModel.data().equipment : null;

        var windowModel = new EquipmentsWindowModel(context, character.getTemplateEvaluateStats(), character.unwrap(), thisModel.identity.slot, thisModel.data(), reverseEquipment);

        context.dialogManager.switchEquipments(windowModel).run(function (tryResult) {

            if (tryResult.isSuccess) {

                thisModel.setPureData(tryResult.result);
            }

            return;
        });
    };

    function updateImageObject() {

        thisModel.image(getImageObject().image);
        thisModel.elementImage(getElementImage());
    }

    function updateToolTip() {

        thisModel.toolTipData((thisModel.isReverse() ? thisModel.reverseModel : thisModel).data());
        thisModel.hasToolTipData(thisModel.toolTipData() !== null);

        updateImageObject();
    }

    function getImageObject() {

        var actualDataModel = getActualDataModel();

        if (actualDataModel != null && actualDataModel.equipment != null) {
            return actualDataModel.equipment;
        } else {
            return thisModel.identity.slot;
        }
    }


    function getElementImage()
    {
        var actualDataModel = getActualDataModel();

        if (actualDataModel != null) {

            var cardModel = actualDataModel.cards.firstOrDefault();

            if (cardModel != null) {

                var card = cardModel.card();

                if (card != null && card.type.element != null) {

                    return cardModel.toolTipImage();
                }
            }
        }

        return null;
    }

    
    function getActualDataModel() {

        return thisModel.isReverse() ? thisModel.reverseModel.data() : thisModel.data();
    }


    //---------------------------------------------------------------------

    updateImageObject();
}