function DiaperManagement(app, dataModel, options) {
    BaseVm.apply(this, arguments);
    var self = this;

    self.canSave = ko.computed(function () {
        self.clearErrors();

        if(!(self.isEditing() || self.isCreatingNew())) {
            return false;
        }

        //Must have an Occurred on Date
        if (!self.item().occurredOn()) {
            self.addError('Date of Occurrence is required.');
            return false;
        }
        //Must have either Poop or Pee
        if (!(self.item().urine() || self.item().stool())) {
            self.addError('Pee and/or Poop is required.');
            return false;
        }

        return true;
    }, self);

    self.sortedByDate = ko.computed(function () {
        var s = self.items.slice(0).sort(function (l, r) {
            return moment(r.occurredOn()).isBefore(moment(l.occurredOn())) ? -1 : 1;
        });
        return s;
    }, self);
}

app.addViewModel({
    name: "Diaper",
    bindingMemberName: "diapermgt",
    factory: function(app, dataModel) {
        var newDc = function() {
            return new DC({
                id: 0,
                occurredOn: '',
                notes: '',
                urine: false,
                stool: false
            });
        };
        var options = {
            modelFunc: DC,
            newItem: newDc,
            url: 'api/diaperchanges',
            itemName: 'diaper'
        };
        DiaperManagement.prototype = new BaseVm(app, dataModel, options);
        DiaperManagement.prototype.constructor = DiaperManagement;

        return new DiaperManagement(app, dataModel, options);
    }
});