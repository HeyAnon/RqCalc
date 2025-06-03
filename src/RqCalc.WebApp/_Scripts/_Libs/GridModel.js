function GridModel(model) {

    var thisModel = this;

    thisModel.items = ko.observable();

    var selectedWrapItem;

    thisModel.selectItem = function (newValue) {

        if (newValue === selectedWrapItem) {
            return;
        }

        if (selectedWrapItem != null) {
            selectedWrapItem.isSelectedRow(false);
            selectedWrapItem = null;
        }

        selectedWrapItem = newValue;

        if (selectedWrapItem != null) {
            selectedWrapItem.isSelectedRow(true);
            model.selectedItem(newValue.baseItem);
        } else {
            model.selectedItem(null);
        }
    };


    function updateSelectedItem() {

        var newSelectedWrapItem = thisModel.items().singleOrDefault(function (wrapItem) {
            return wrapItem.baseItem === model.selectedItem();
        });

        thisModel.selectItem(newSelectedWrapItem);
    }

    function updatItems() {

        var newWrapItems = model.items().map(function (item) {
            return $.extend({
                isSelectedRow: ko.observable(item === model.selectedItem()),
                baseItem: item,
                selectRow: function () {
                    thisModel.selectItem(this);
                }
            }, item);
        });

        thisModel.items(newWrapItems);

        updateSelectedItem();
    }

    model.selectedItem.subscribe(function (_) {
        updateSelectedItem();
    });

    model.items.subscribe(function (_) {
        updatItems();
    });

    updatItems();
}