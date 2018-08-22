var viewModel;

$(document).ready(function() {
    viewModel = new SetupViewModel();

    ko.applyBindings(viewModel, document.getElementsByTagName("body")[0]);
});

function SetupViewModel(data) {
    var self = this;
    data = data || {};

    // Prints the whole model with all of its subarrays into a well-indented string.
    // It can be really useful for debugging.
    self.displayModelAsJSON = function() {
        return ko.toJSON(self, null, 2);
    };

    // static
    self.nextFieldId = 1;

    // observables
    self.name = ko.observable(data.name || "");
    self.fields = ko.observableArray(data.fields || []);

    // events
    self.addField = function() {
        var field = new FieldViewModel({ id: self.nextFieldId });
        self.fields.push(field);
        self.nextFieldId++;

        return field;
    };

    self.removeField = function() {
        console.log("remove field");
    };
}

function FieldViewModel(data) {
    var self = this;
    data = data || {};

    // observables
    self.id = ko.observable(data.id || 1);
    self.name = ko.observable(data.name || "");
    self.fieldType = ko.observable(data.fieldType || -1);
    self.minValue = ko.observable(data.minValue || null);
    self.maxValue = ko.observable(data.maxValue || null);

    // events
    //self.typeChanged = function() {
    //    console.log("type changed");
    //};
}