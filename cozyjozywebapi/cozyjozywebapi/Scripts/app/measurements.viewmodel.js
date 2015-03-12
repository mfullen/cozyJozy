﻿function MeasurementClass(data) {
    this.id = ko.observable(data.id);
    this.dateRecorded = ko.observable(data.dateRecorded).extend({ required: true });
    this.height = ko.observable(data.height);
    this.weight = ko.observable(data.weight);
    this.childId = ko.observable(data.childId);
    this.headCircumference = ko.observable(data.headCircumference);
}

function Measurement(app, dataModel, options) {
    var self = this;
    BaseVm.call(self, app, dataModel, options);

    self.canSave = function () {

    }

    self.sortedByDate = ko.computed(function () {
        var s = self.items().slice(0).sort(function (l, r) {
            return moment(r.dateRecorded()).isBefore(moment(l.dateRecorded())) ? -1 : 1;
        });
        return s;
    }, self);

    self.fetchItems();
}

app.addViewModel({
    name: "Measurements",
    bindingMemberName: "measurementMgt",
    factory: function (app, dataModel) {
        var newMeasurement = function () {
            return new MeasurementClass({
                id: 0,
                dateRecorded: '',
                height: null
            });
        }

        var options = {
            modelFunc: MeasurementClass,
            newItem: newMeasurement,
            url: 'api/measurement',
            itemName: 'measurement'
        };
        Measurement.prototype = new BaseVm(app, dataModel, options);
        Measurement.prototype.constructor = Measurement;
        return Measurement(app, dataModel, options);
    }
});