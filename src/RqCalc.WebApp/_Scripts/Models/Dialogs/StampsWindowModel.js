function StampsWindowModel(context, equipment, $class, startupStampVariant) {

    var thisModel = this;

    $.extend(thisModel, new SwitchItemDialogModel([], null));

    thisModel.colors = context.stampColors.orderBy(function (stamp) {
        return stamp.id;
    });

    var bestColor = thisModel.colors.last();

    thisModel.color = ko.observable(startupStampVariant == null ? bestColor : startupStampVariant.color);

    thisModel.clear = function () {
        thisModel.pureModel.selectedItem(null);
    };

    thisModel.getResult = function () {

        var stampVariant = thisModel.pureModel.selectedItem();

        return stampVariant == null ? null : MainHelper.Stamp.getByColor(stampVariant.stamp, thisModel.color());
    };

    var baseVariants = context.stamps
        .map(function (stamp) {
            return { stamp: stamp, allowed: context.isAllowedStamp(stamp, equipment, $class) };
        }).filter(function (pair) {
            return pair.allowed !== false;
        }).map(function (pair) {
            return { stampVariant: MainHelper.Stamp.getByColor(pair.stamp, bestColor), allowed: pair.allowed === true };
        });

    thisModel.showLegacy = ko.observable(startupStampVariant != null && startupStampVariant.stamp.isLegacy);

    thisModel.showShared = ko.observable(startupStampVariant != null && baseVariants.any(function (pair) {
        return pair.stampVariant.stamp === startupStampVariant.stamp && !pair.allowed;
    }));

    thisModel.showLegacy.subscribe(function (_) {
        refresh();
    });

    thisModel.showShared.subscribe(function (_) {
        refresh();
    });


    thisModel.hasLegacy = baseVariants.any(function (pair) {
        return pair.stampVariant.stamp.isLegacy;
    });

    thisModel.hasShared = baseVariants.any(function (v) {
        return v.allowed === false;
    });


    refresh(true);

    //---------------------------------------------------------------------

    function refresh(isInit) {

        var stampVariants = baseVariants
            .filter(function (pair) {
                return !pair.stampVariant.stamp.isLegacy || thisModel.showLegacy();
            })
            .filter(function (pair) {
                return pair.allowed || thisModel.showShared();
            })
            .map(function (pair) {
                return pair.stampVariant;
            })
            .orderBy(function (stampVariant) {
                return -MainHelper.Stamp.getOrderIndex(stampVariant, $class);
            });

        thisModel.pureModel.items(stampVariants);

        if (isInit) {

            var startupStamp = startupStampVariant == null ? null : startupStampVariant.stamp;

            if (startupStamp != null) {
                thisModel.pureModel.selectedItem(MainHelper.Stamp.getByColor(startupStamp, bestColor));
            }
        }

        if (thisModel.pureModel.selectedItem() == null) {

            thisModel.pureModel.selectedItem(stampVariants.firstOrDefault());
        }
    }
}