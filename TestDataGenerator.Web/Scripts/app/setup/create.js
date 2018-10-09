'use strict';

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

    self.removeField = function(field) {
        if (field !== null) {
            if (message(getConstants().messages.WARN_CONFIRM_REMOVE_FIELD, true)) {
                self.fields.remove(field);
            }
        }

        return field;
    };

    self.saveSetup = function() {
        var jsonData = ko.toJSON(self);

        $.ajax({
            type: 'POST',
            url: window.baseURL + 'Setup/Create',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: jsonData,
            success: function(response) {
                console.log(response);
            },
            error: function(response) {
                console.log(response);
            }
        });
    };
}

function FieldViewModel(data) {
    var self = this;
    data = data || {};

    // observables
    self.id = ko.observable(data.id || 1);
    self.name = ko.observable(data.name || "");
    self.fieldType = ko.observable(data.fieldType || -1);
    self.nullChance = ko.observable(data.nullChance || 0);

    self.lastNameFieldModel = ko.observable(data.LastNameFieldModel || new LastNameFieldModel());
    self.firstNameFieldModel = ko.observable(data.FirstNameFieldModel || new FirstNameFieldModel());
    self.dateTimeFieldModel = ko.observable(data.DateTimeFieldModel || new DateTimeFieldModel());
    self.emailFieldModel = ko.observable(data.EmailFieldModel || new EmailFieldModel());
    self.textFieldModel = ko.observable(data.TextFieldModel || new TextFieldModel());
    self.byteFieldModel = ko.observable(data.ByteFieldModel || new ByteFieldModel());
    self.int16FieldModel = ko.observable(data.Int16FieldModel || new Int16FieldModel());
    self.int32FieldModel = ko.observable(data.Int32FieldModel || new Int32FieldModel());
    self.int64FieldModel = ko.observable(data.Int64FieldModel || new Int64FieldModel());
    self.singleFieldModel = ko.observable(data.SingleFieldModel || new SingleFieldModel());
    self.doubleFieldModel = ko.observable(data.DoubleFieldModel || new DoubleFieldModel());
    self.decimalFieldModel = ko.observable(data.DecimalFieldModel || new DecimalFieldModel());
    self.hashFieldModel = ko.observable(data.HashFieldModel || new HashFieldModel());
    self.guidFieldModel = ko.observable(data.GuidFieldModel || new GuidFieldModel());
    self.base64FieldModel = ko.observable(data.Base64FieldModel || new Base64FieldModel());
    self.customSetFieldModel = ko.observable(data.CustomSetFieldModel || new CustomSetFieldModel());

    // helpers
    self.templateToUse = function() {
        var enumItemNames = Object.keys(getConstants().fieldType);
        var selectedFieldTypeIndex = self.fieldType();

        return enumItemNames[selectedFieldTypeIndex] + "-template";
    };
}

//
// Subtype field models
//
function LastNameFieldModel(data) {
    // empty model
}

function FirstNameFieldModel(data) {
    var self = this;
    data = data || {};

    // observables
    self.gender = ko.observable(data.gender || getConstants().gender.none);
}

function DateTimeFieldModel(data) {
    var self = this;
    data = data || {};
    
    self.minDate = ko.observable(data.minDate || new CustomDate(1900, 1, 1, 0, 0, 0));
    self.maxDate = ko.observable(data.maxDate || new CustomDate(2000, 12, 31, 23, 59, 59));
    self.minValue = ko.pureComputed(function() {
        return concatDate(self.minDate());
    });
    self.maxValue = ko.pureComputed(function() {
        return concatDate(self.maxDate());
    });

    function concatDate(date) {
        var zeroes = (value) => {
            return value ? (value < 10 ? '0' : '') + value : '00';
        };

        var result = date.year() + '.' + zeroes(date.month()) + '.' + zeroes(date.day()) + ' ' +
            zeroes(date.hours()) + ':' + zeroes(date.minutes()) + ':' + zeroes(date.seconds());

        return result;
    }
}

function CustomDate(year, month, day, hours, minutes, seconds) {
    return new CustomDate({
        year: year,
        month: month,
        day: day,
        hours: hours,
        minutes: minutes,
        seconds: seconds
    });
}

function CustomDate(data) {
    var self = this;
    data = data || {};

    self.year = ko.observable(data.year || 1900);
    self.month = ko.observable(data.month || 1);
    self.day = ko.observable(data.day || 1);
    self.hours = ko.observable(data.hours || 0);
    self.minutes = ko.observable(data.minutes || 0);
    self.seconds = ko.observable(data.seconds || 0);

    self.dayList = ko.computed(function() {
        var year = self.year();
        var month = self.month();
        var daysInMonth = getDaysInMonth(year, month);
        var result = fillArray(1, daysInMonth);

        return result;
    });
}

function EmailFieldModel(data) {
    // empty model
}

function TextFieldModel(data) {
    var self = this;
    data = data || {};

    // observables
    self.minLength = ko.observable(data.minValue || 0);
    self.maxLength = ko.observable(data.maxValue || getConstants().int32.maxValue);
    self.letterCase = ko.observable(data.letterCase || getConstants().letterCase.ignore);
    self.mustContainLetters = ko.observable(data.mustContainLetters || true);
    self.mustContainDigit = ko.observable(data.mustContainDigit || false);
    self.mustContainCustom = ko.observableArray(data.mustContainCustom || []);
}

function ByteFieldModel(data) {
    var self = this;
    data = data || {};

    // observables
    self.isSigned = ko.observable(data.isSigned || false);
    self.minValue = ko.observable(data.minValue || (self.isSigned() ? getConstants().sbyte.minValue : getConstants().byte.minValue));
    self.maxValue = ko.observable(data.maxValue || (self.isSigned() ? getConstants().sbyte.maxValue : getConstants().byte.maxValue));
}

function Int16FieldModel(data) {
    var self = this;
    data = data || {};

    // observables
    self.isSigned = ko.observable(data.isSigned || true);
    self.minValue = ko.observable(data.minValue || (self.isSigned() ? getConstants().int16.minValue : getConstants().uint16.minValue));
    self.maxValue = ko.observable(data.maxValue || (self.isSigned() ? getConstants().int16.maxValue : getConstants().uint16.maxValue));
}

function Int32FieldModel(data) {
    var self = this;
    data = data || {};

    // observables
    self.isSigned = ko.observable(data.isSigned || true);
    self.minValue = ko.observable(data.minValue || (self.isSigned() ? getConstants().int32.minValue : getConstants().uint32.minValue));
    self.maxValue = ko.observable(data.maxValue || (self.isSigned() ? getConstants().int32.maxValue : getConstants().uint32.maxValue));
}

function Int64FieldModel(data) {
    var self = this;
    data = data || {};

    // observables
    self.isSigned = ko.observable(data.isSigned || true);
    self.minValue = ko.observable(data.minValue || (self.isSigned() ? getConstants().int64.minValue : getConstants().uint64.minValue));
    self.maxValue = ko.observable(data.maxValue || (self.isSigned() ? getConstants().int64.minValue : getConstants().uint64.minValue));
}

function SingleFieldModel(data) {
    var self = this;
    data = data || {};

    // observables
    self.minValue = ko.observable(data.minValue || getConstants().single.minValue);
    self.maxValue = ko.observable(data.maxValue || getConstants().single.maxValue);
}

function DoubleFieldModel(data) {
    var self = this;
    data = data || {};

    // observables
    self.minValue = ko.observable(data.minValue || getConstants().double.minValue);
    self.maxValue = ko.observable(data.maxValue || getConstants().double.maxValue);
}

function DecimalFieldModel(data) {
    var self = this;
    data = data || {};

    // observables
    self.minValue = ko.observable(data.minValue || getConstants().decimal.minValue);
    self.maxValue = ko.observable(data.maxValue || getConstants().decimal.maxValue);
}

function HashFieldModel(data) {
    var self = this;
    data = data || {};

    // observables
    self.desiredLength = ko.observable(data.desiredLength || 32);
}

function GuidFieldModel(data) {
    var self = this;
    data = data || {};

    // observables
    self.separateWithHyphens = ko.observable(data.separateWithHyphens || false);
}

function Base64FieldModel(data) {
    var self = this;
    data = data || {};

    // observables
    self.desiredLength = ko.observable(data.desiredLength || 32);
}

function CustomSetFieldModel(data) {
    var self = this;
    data = data || {};

    // observables
    self.items = ko.observableArray(data.items || []);
}