function getFacade(serviceAddress) {

    function postInvokeFacade(method, data) {

        return Async.ajax({
            type: "POST",
            url: serviceAddress + "/" + method,
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        });
    }

    function getInvokeFacade(method, data) {

        return Async.ajax({
            type: "GET",
            url: serviceAddress + "/" + method,
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        });
    }


    var internalFacade =
    {
        getCharacterStartupComplexData: postInvokeFacade("GetCharacterStartupComplexData", {}),

        getDefaultCharacter: postInvokeFacade("GetDefaultCharacter", {}),

        decryptCharacter: function (code) {
            return postInvokeFacade("DecryptCharacter", code);
        },

        calcCharacter: function (source) {
            return postInvokeFacade("CalcCharacter", source);
        },

        //---------------------------------------------

        getTalentBuildStartupComplexData: postInvokeFacade("GetTalentBuildStartupComplexData", {}),

        getDefaultTalentBuild: postInvokeFacade("GetDefaultTalentBuild", {}),

        decryptTalentBuild: function (code) {
            return postInvokeFacade("DecryptTalentBuild", code);
        },

        calcTalentBuild: function (source) {
            return postInvokeFacade("CalcTalentBuild", source);
        },

        //---------------------------------------------

        getGuildTalentBuildStartupComplexData: postInvokeFacade("GetGuildTalentBuildStartupComplexData", {}),

        getDefaultGuildTalentBuild: postInvokeFacade("GetDefaultGuildTalentBuild", {}),

        decryptGuildTalentBuild: function (code) {
            return postInvokeFacade("DecryptGuildTalentBuild", code);
        },

        calcGuildTalentBuild: function (source) {
            return postInvokeFacade("CalcGuildTalentBuild", source);
        }
    }

    return new function () {

        var thisModel = this;

        thisModel.getImageUrl = function (type, id) {
            return serviceAddress + "/GetImage?type=" + type + "&id=" + id;
        };

        thisModel.getStaticImage = function (id) {
            return thisModel.getImageUrl("StaticImage", id);
        };

        //---------------------------------------------

        thisModel.decryptCharacter = function (source) {
            return Async.fmap(internalFacade.decryptCharacter(source), ObjHelper.toLowerData);
        }

        thisModel.calcCharacter = function (source) {
            return Async.fmap(internalFacade.calcCharacter(ObjHelper.toUpperData(source)), ObjHelper.toLowerData);
        }

        thisModel.getCharacterContextData = Async.fmap(Async.zipD([internalFacade.getCharacterStartupComplexData, internalFacade.getDefaultCharacter]), function (result) {

            return ObjHelper.toLowerData($.extend(result[0], { defaultCharacter: result[1] }));
        });

        //---------------------------------------------

        thisModel.decryptTalentBuild = function (source) {
            return Async.fmap(internalFacade.decryptTalentBuild(source), ObjHelper.toLowerData);
        }

        thisModel.calcTalentBuild = function (source) {
            return Async.fmap(internalFacade.calcTalentBuild(ObjHelper.toUpperData(source)), ObjHelper.toLowerData);
        }

        thisModel.getTalentBuildContextData = Async.fmap(Async.zipD([internalFacade.getTalentBuildStartupComplexData, internalFacade.getDefaultTalentBuild]), function (result) {

            return ObjHelper.toLowerData($.extend(result[0], { defaultTalentBuild: result[1] }));
        });

        //---------------------------------------------

        thisModel.decryptGuildTalentBuild = function (source) {
            return Async.fmap(internalFacade.decryptGuildTalentBuild(source), ObjHelper.toLowerData);
        }

        thisModel.calcGuildTalentBuild = function (source) {
            return Async.fmap(internalFacade.calcGuildTalentBuild(ObjHelper.toUpperData(source)), ObjHelper.toLowerData);
        }

        thisModel.getGuildTalentBuildContextData = Async.fmap(Async.zipD([internalFacade.getGuildTalentBuildStartupComplexData, internalFacade.getDefaultGuildTalentBuild]), function (result) {

            return ObjHelper.toLowerData($.extend(result[0], { defaultGuildTalentBuild: result[1] }));
        });
    }
}