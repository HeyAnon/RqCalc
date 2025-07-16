ko.bindingHandlers.indeterminateValue = {
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        element.indeterminate = value;
    }
};