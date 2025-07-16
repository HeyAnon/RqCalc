function ActiveImageModel(context, type, hint, startupValue, switchFunc) {
    
    var thisModel = this;

    var defaultImage = context.facade.getStaticImage(type);


    thisModel.active = ko.observable(false);

    thisModel.value = ko.observable(startupValue);
    
    thisModel.image = ko.observable();

    thisModel.value.subscribe(function(_) {

        updateImageObject();
    });

    thisModel.switchValue = switchFunc;
    thisModel.hint = hint;

    //---------------------------------------------------------------------

    function updateImageObject() {

        thisModel.image(getImage());
    }

    function getImage() {

        var value = thisModel.value();
        
        if (value != null && value.image != undefined) {
            return value.image;
        } else {
            return defaultImage;
        }
    }

    //---------------------------------------------------------------------

    updateImageObject();
}