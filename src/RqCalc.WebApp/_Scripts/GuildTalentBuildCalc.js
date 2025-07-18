﻿$(function () {

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


        var loadTemplatesFunc = loadExternalKnockoutTemplates($("body"),

            "Templates", ["GuildTalentBranchBodyControl",
                          "GuildTalentControl",
                          "TalentDescriptionControl",
                          "TextTemplateControl",
                          "GuildTalentBuildControl"]);

        var startCode = getStartCode();

        var decryptFunc = startCode == null ? Async._return(null) : Async.withCatch(facade.decryptGuildTalentBuild(startCode));

        var multiLoadFunc = Async.zip3(loadTemplatesFunc, facade.getGuildTalentBuildContextData, decryptFunc);

        return Async.fmap(multiLoadFunc, function (result) {

            var context = new GuildTalentBuildApplicationContext(result[1], facade);
            var startCharacter = result[2];

            var rootModel = new GuildTalentsWindowModel (context, startCharacter == null ? context.defaultGuildTalentBuild : startCharacter);
            
            function subscribeCode(init) {

                rootModel.code.subscribe(updateUrlCode);

                updateUrlCode(init ? startCode : rootModel.code());

                function updateUrlCode(newCode) {

                    var index = window.location.href.toLowerCase().indexOf(sep);

                    var baseUrl = index === -1 ? window.location.href : window.location.href.substr(0, index);

                    if (newCode === null || newCode === context.defaultGuildTalentBuildCode) {
                        updateUrl(baseUrl);
                    } else {
                        updateUrl(baseUrl + sep + newCode);
                    }
                }

                function updateUrl(newUrl) {

                    window.history.pushState({}, $(this).attr('title'), newUrl);
                }
            }

            subscribeCode(true);

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