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

    self.minValue = ko.observable(data.minValue || null); // + minDate, minLength
    self.maxValue = ko.observable(data.maxValue || null); // + maxDate, maxLength

    self.isMale = ko.observable(data.isMale || false);

    self.letterCase = ko.observable(data.letterCase || 0);
    self.mustContainSpace = ko.observable(data.mustContainSpace || false);
    self.mustContainDigit = ko.observable(data.mustContainDigit || false);
    self.mustContainAccutes = ko.observable(data.mustContainAccutes || false);
    self.mustContainCustom = ko.observableArray(data.mustContainCustom || []);

    self.desiredLength = ko.observable(data.desiredLength || 0);

    self.separateWithHyphens = ko.observable(data.separateWithHyphens || false);

    self.fixItems = ko.observableArray(data.fixItems || []);

    // legyen egy függvény, ami POST-olás előtt a kiválasztott fieldType alapján
    // delete -tel kiszedi az egyes mezőknél a felesleges értékpárokat
    // VAGY
    // legyen egy fieldSpecific mező, ami a fieldType állításakor mindig más és más alobjektumot kap értékül

    // events
    self.fieldTypeChanged = function(e) {
        console.log(e);

        switch (self.fieldType) {
            case 1: // kigeneráltatni egy View-ba az enum elemeit, mint ahogy a Signal projektben van
                break;
            case 2:
                break;
            // :
        }
    };
}