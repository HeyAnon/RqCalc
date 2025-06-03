$(function () {
    
    var facade = getFacade(facadeAddress);

    $.blockUI({
        message: $("<img src=\"_Images/loading.gif\" /> ")
    });

    getRootModelFunc().run(function (tryResult) {

        $.unblockUI();

        if (tryResult.result !== undefined) {

            ko.applyBindings(tryResult.result);

            initPopups(true);

        } else {
            $.blockUI({
                message: "Server has encountered error",
                css: { backgroundColor: "#f00", color: "#fff" }
            });
        }
    });

    //--------------------------------------------------------------

    function getRootModelFunc() {

        var sep = "?code=";

        function getStartCode() {

            var index = window.location.href.toLowerCase().indexOf(sep);

            return index === -1 ? null : window.location.href.substr(index + sep.length);
        }

        //--------------------------------------------------------------


        var loadTemplatesFunc = loadTemplates($("body"),

            "Templates", ["CardDescriptionControl",
                          "EquipmentControl",
                          "EditStatControl",
                          "StatControl",
                          "ActiveImageControl",
                          "EquipmentsControl",
                          "StatsControl",
                          "EquipmentToolTipControl",
                          "BonusCollectionControl",
                          "BuffCollectionControl",
                          "GuildTalentBranchBodyControl",
                          "GuildTalentControl",
                          "TalentBranchBodyControl",
                          "TalentControl",
                          "CardControl",
                          "EquipmentStampControl",
                          "TalentDescriptionControl",
                          "TextTemplateControl",
                          "DisplayStatBodyControl",
                          "EditStatBodyControl",
                          "GuildTalentBuildControl",
                          "TalentBuildControl",
                          "CollectedTabControl",
                          "CollectedStatisticControl",
                          "CollectedGroupControl"],

            "Templates/Dialogs", ["ElixirsDialog",
                                  "ConsumablesDialog",
                                  "GuildTalentsDialog",
                                  "TalentsDialog",
                                  "AurasDialog",
                                  "BuffsDialog",
                                  "EquipmentsDialog",
                                  "CardsDialog",
                                  "StampsDialog",
                                  "NewCharDialog",
                                  "AboutDialog",
                                  "CollectionsDialog"]);

        var startCode = getStartCode();

        var decryptFunc = startCode == null ? Async._return(null) : Async.withCatch(facade.decryptCharacter(startCode));

        var multiLoadFunc = Async.zip3(loadTemplatesFunc, facade.getCharacterContextData, decryptFunc);

        return Async.fmap(multiLoadFunc, function (result) {

            var dialogManager = new DialogManager(initPopups);

            var context = new CharacterApplicationContext(result[1], facade, dialogManager);
            var startCharacter = result[2];

            var rootModel = new CharacterWindowModel(context, false);

            if (startCharacter != null) {
                rootModel.setCharacter(startCharacter);
            } else {
                rootModel.setDefaultCharacter();
            }



            function subscribeCharacter(character, init) {

                character.code.subscribe(updateUrlCode);

                updateUrlCode(init ? startCode : character.code());

                function updateUrlCode(newCode) {

                    var index = window.location.href.toLowerCase().indexOf(sep);

                    var baseUrl = index === -1 ? window.location.href : window.location.href.substr(0, index);

                    if (newCode === null || newCode === context.defaultCharacterCode) {
                        updateUrl(baseUrl);
                    } else {
                        updateUrl(baseUrl + sep + newCode);
                    }
                }

                function updateUrl(newUrl) {

                    window.history.pushState({}, $(this).attr('title'), newUrl);
                }
            }

            rootModel.character.subscribe(subscribeCharacter);
            subscribeCharacter(rootModel.character(), true);

            return rootModel;
        });

        //---------------------------------------------------
    }

    var initializedPopups = [];
    function initPopups(cache) {

        $('.pop-container').each(function () {

            if (cache) {
                if (initializedPopups.contains(this)) {
                    return;
                }

                initializedPopups.push(this);
            }

            var $elem = $(this);

            $elem.popover({
                placement: 'auto',
                trigger: 'hover',
                html: true,
                container: 'body',
                animation: true,
                content: function () {

                    var contentElem = $($elem.children('.popover')[0]);

                    return withHidePopover(contentElem.html());
                }
            });
        });
    }
});

function withHidePopover(html) {

    return "<div onmousemove='hidePopover(this)'>" + html + "</div>";
}

function hidePopover(me) {

    var dynamicPopup = me.parentNode;

    var container = dynamicPopup.parentNode;

    container.removeNode(dynamicPopup);

    return;
}