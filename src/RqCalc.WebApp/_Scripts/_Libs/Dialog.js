function SwitchItemDialogModel(source, startItem) {

    var model = {
        items: ko.observable(source),
        selectedItem: ko.observable(startItem)
    };

    this.pureModel = model;

    this.mainGrid = new GridModel(model);
}