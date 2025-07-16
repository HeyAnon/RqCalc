function loadExternalKnockoutTemplates(target, templateFolder, templates) {

    var loads = $.map(templates, function (templateName) {

        var getFunc = Async.get(templateFolder + "/" + templateName + ".html");

        return Async.fmap(getFunc, function (templateData) {
            return { name: templateName, data: templateData }
        });
    });

    var multiLoad = Async.zipD(loads);

    return Async.fmap(multiLoad, function (res) {

        return res.map(function (pair) {

            var template = "<script id=\"" + pair.name + "\" type=\"text/html\">" + pair.data + "</script>";

            var data = $(template);

            target.append(data);

            return data;
        });
    });
}

function loadDialogsTemplates(target, dialogFolder, dialogs) {

    var loads = $.map(dialogs, function (templateName) {

        var getFunc = Async.get(dialogFolder + "/" + templateName + ".html");

        return Async.fmap(getFunc, function (templateData) {
            return { name: templateName, data: templateData }
        });
    });

    var multiLoad = Async.zipD(loads);

    return Async.fmap(multiLoad, function (res) {

        return res.map(function (pair) {

            var data = $(pair.data);

            target.append(data);

            return data;
        });
    });
}



function loadTemplates(target, templateFolder, templates, dialogFolder, dialogs) {

    return Async.zip2(loadExternalKnockoutTemplates(target, templateFolder, templates), loadDialogsTemplates(target, dialogFolder, dialogs));
}