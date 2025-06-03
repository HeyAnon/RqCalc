function DialogManager(initPopups) {

    this.elixirsModel = ko.observable();
    this.consumablesModel = ko.observable();
    this.guildTalentsModel = ko.observable();
    this.talentsModel = ko.observable();
    this.aurasModel = ko.observable();
    this.buffsModel = ko.observable();
    this.equipmentsModel = ko.observable();
    this.cardModel = ko.observable();
    this.stampModel = ko.observable();
    this.collectionsModel = ko.observable();
    
    this.switchElixir = getDialogFunc(this.elixirsModel, "#elixirsModal", "#elixirsBtnSuccess", "elixirsBtnCancel", "#elixirsBtnClear", null);
    this.switchConsumables = getDialogFunc(this.consumablesModel, "#consumablesModal", "#consumablesBtnSuccess", "consumablesBtnCancel");
    this.switchGuildTalents = getDialogFunc(this.guildTalentsModel, "#guildTalentsModal", "#guildTalentsBtnSuccess", "guildTalentsBtnCancel", "#guildTalentsBtnClear", []);
    this.switchTalents = getDialogFunc(this.talentsModel, "#talentsModal", "#talentsBtnSuccess", "talentsBtnCancel", "#talentsBtnClear", []);
    this.switchAura = getDialogFunc(this.aurasModel, "#aurasModal", "#aurasBtnSuccess", "aurasBtnCancel");
    this.switchBuffs = getDialogFunc(this.buffsModel, "#buffsModal", "#buffsBtnSuccess", "buffsBtnCancel", "#buffsBtnClear", []);
    this.switchEquipments = getDialogFunc(this.equipmentsModel, "#equipmentsModal", "#equipmentsBtnSuccess", "equipmentsBtnCancel", "#equipmentsBtnClear", null);
    this.switchCard = getDialogFunc(this.cardModel, "#cardsModal", "#cardsBtnSuccess", "cardsBtnCancel", "#cardsBtnClear", null);
    this.switchStamp = getDialogFunc(this.stampModel, "#stampsModal", "#stampsBtnSuccess", "stampsBtnCancel", "#stampsBtnClear", null);
    this.switchCollections = getDialogFunc(this.collectionsModel, "#collectionsModal", "#collectionsBtnSuccess", "collectionsBtnCancel");

    this.askNewChar = getDialogFunc(undefined, "#newCharModal", "#newCharBtnSuccess", "newCharBtnCancel");
    this.showAbout = getDialogFunc(undefined, "#aboutModal", undefined, "aboutBtnCancel");


    function getDialogFunc(dialogSetter, modalName, successName, cancelName, clearName, clearValue) {

        var isInvoke = false;

        var dialogModel;
        var result;
        var currentCallback;

        var modal = $(modalName);

        if (successName !== undefined)
        {
            var modal_Success = $(successName);

            modal_Success.click(function () {
                result = TryResult._return(dialogModel == null ? null : dialogModel.getResult());
            });
        }

        if (clearName !== undefined)
        {
            var modal_Cancel = $(cancelName);

            modal_Cancel.click(function () {
                result = TryResult._break;
                return;
            });
        }

        if (clearName !== undefined && clearValue !== undefined) {

            var modal_Clear = $(clearName);

            modal_Clear.click(function () {
                
                result = TryResult._return(clearValue);

                if (dialogModel.clear != undefined) {
                    dialogModel.clear();
                }
            });
        }

        modal.on('hidden.bs.modal', function () {

            if (!isInvoke) {
                throw "Multiply close";
            } else {
                isInvoke = false;
                currentCallback(result);
            }
        });


        return function (model) {

            return {

                run: function (callback) {

                    if (isInvoke) {
                        callback(TryResult._break);
                        return;
                    } else {
                        isInvoke = true;
                    }

                    currentCallback = callback;

                    dialogModel = model;

                    if (dialogSetter != undefined) {
                        dialogSetter(dialogModel);
                    }

                    result = TryResult._break;
                    modal.modal();

                    initPopups();
                }
            }
        };
    }
}