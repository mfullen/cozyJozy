function Measurement(app, dataModel, options) {
    BaseVm.apply(this, arguments);
    var self = this;

    self.canSave = ko.computed(function() {
        self.clearErrors();
        if (!(self.isEditing() || self.isCreatingNew())) {
            return false;
        }

        //Must have Date
        if (!self.item().dateRecorded()) {
            self.addError('Date is required.');
            return false;
        }

        //Must have Height
        if (!self.item().height()) {
            self.addError('Height is required.');
            return false;
        }

        //Must have Weight
        if (!self.item().weight()) {
            self.addError('Weight is required.');
            return false;
        }

        //Must have Head Circumference
        if (!self.item().headCircumference()) {
            self.addError('Head Circumference is required.');
            return false;
        }

        return true;
    });

    self.sortedByDate = ko.computed(function () {
        var s = self.items().slice(0).sort(function (l, r) {
            return moment(r.dateRecorded()).isBefore(moment(l.dateRecorded())) ? -1 : 1;
        });
        return s;
    }, self);

    self.lbsAndOzString = function(lb) {
        var dec = (lb % 1).toFixed(4);
        var oz = Math.ceil(dec * 16);
        return Math.floor(lb) + " lb " + oz + " oz";
    }

}

app.addViewModel({
    name: "Measurements",
    bindingMemberName: "measurementMgt",
    factory: function (app, dataModel) {
        var newMeasurement = function () {
            return new MeasurementClass({
                id: 0,
                dateRecorded: '',
                height: 5,
                weight: 1,
                headCircumference: 1
            });
        }

        var options = {
            modelFunc: MeasurementClass,
            newItem: newMeasurement,
            url: 'api/measurement',
            itemName: 'measurement',
            canCrud: app.canWriteMeasurements()
        };
        Measurement.prototype = new BaseVm(app, dataModel, options);
        Measurement.prototype.constructor = Measurement;
        return new Measurement(app, dataModel, options);
    }
});