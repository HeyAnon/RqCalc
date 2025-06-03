function UpdateModel(thisModel) {
    
    thisModel.updating = false;

    thisModel.updateOperation = function (operation, handleUpdate) {

        if (handleUpdate === undefined) {
            handleUpdate = true;
        }

        if (thisModel.updating) {
            if (handleUpdate) {
                return;
            } else {
                throw new Error("Already updating");
            }
        }

        var prevState = thisModel.updating;
        thisModel.updating = true;

        try {
            operation();
            thisModel.recalculateH(false);
        } finally {
            thisModel.updating = prevState;
        }
    };

    thisModel.handleUpdate = function (_) {
        thisModel.recalculateH();
    };

    thisModel.recalculateH = function (handleUpdate) {
        if (thisModel.updating && (handleUpdate === undefined || handleUpdate)) {
            return;
        }

        if (thisModel.recalculate != null) {
            thisModel.recalculate();
        }

        if (thisModel.recalculateAsync != null) {
            thisModel.recalculateAsync();
        }
    };
}

function UpdateCodeModel(thisModel, baseCalcFunc, baseDecryptFunc) {
    
    $.extend(thisModel, new UpdateModel(thisModel));

    thisModel.prevCode = null;
    thisModel.code = ko.observable(null);


    thisModel.calcFunc = Async.throttle(Async.toLastCall(Async.fromFunc(function () {
        return baseCalcFunc(thisModel.getCalcData());
    })), 200);


    thisModel.decryptFunc = Async.throttle(Async.toLastCall(Async.fromFunc(function () {
        return baseDecryptFunc(thisModel.code());
    })), 200);


    //-----------------------------------------------------------------------------------------------------------------

    thisModel.code.subscribe(function (newCode) {

        if (thisModel.prevCode !== newCode) {
            thisModel.recalculateByCodeAsync();
            return;
        }
    });

    //-----------------------------------------------------------------------------------------------------------------

    thisModel.recalculateByCodeAsync = function () {

        thisModel.decryptFunc.run(function (tryResult) {

            if (tryResult.result !== undefined) {

                thisModel.setData(tryResult.result);

            } else {

                thisModel.code(thisModel.prevCode);

                alert(tryResult.error);
                throw tryResult.error;
            }
        });
    }

    //-----------------------------------------------------------------------------------------------------------------

    thisModel.recalculateAsync = function () {

        thisModel.calcFunc.run(function (tryResult) {
            
            if (tryResult.result !== undefined) {

                thisModel.prevCode = thisModel.getResultCode(tryResult.result);
                thisModel.code(thisModel.prevCode);

                if (thisModel.setDataResult != null) {
                    thisModel.setDataResult(tryResult.result);
                }
                
                return;
            } else if (!tryResult.isBreak) {

                alert(tryResult.error);
                throw tryResult.error;
            }
        });
    }

    //-----------------------------------------------------------------------------------------------------------------
}