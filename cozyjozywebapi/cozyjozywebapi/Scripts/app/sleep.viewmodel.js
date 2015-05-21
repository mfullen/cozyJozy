function SleepViewModel(app, dataModel, options) {

    BaseVm.apply(this, arguments);

    var self = this;

    self.canSave = ko.computed(function () {
        self.clearErrors();

        if (!(self.isEditing() || self.isCreatingNew())) {
            return false;
        }

        //Must have Start Date
        if (!self.item().startTime()) {
            self.addError('Start date/time is required.');
            return false;
        }

        //Must have End Date
        if (!self.item().endTime()) {
            self.addError('Ending date/time is required.');
            return false;
        }


        //Start Date must be before End Date
        var startBeforeEnd = moment(self.item().startTime()).isBefore(moment(self.item().endTime()));
        if (!startBeforeEnd) {
            self.addError('Start date/time must come before the End date/time.');
            return false;
        }

        return true;

    }, self);

    self.sortedByDate = ko.computed(function () {
        var s = self.items.slice(0).sort(function (l, r) {
            return moment(r.startTime()).isBefore(moment(l.startTime())) ? -1 : 1;
        });
        return s;
    }, self);

    self.getTimeDiff = function (d) {
        var sd = moment(d.startTime());
        var ed = moment(d.endTime());
        return ed.diff(sd, 'minutes');
    }

}

app.addViewModel({
    name: "Sleep",
    bindingMemberName: "sleepMgt",
    factory: function (app, dataModel) {
        var dateFormater = 'MM/DD/YYYY hh:mm a';
        var newSleepSession = function () {
            return new SleepSession({
                id: 0,
                startTime: moment().format(dateFormater),
                endTime: moment().add(40, 'minutes').format(dateFormater),
            });
        }

        var options = {
            modelFunc: SleepSession,
            newItem: newSleepSession,
            url: 'api/sleep',
            itemName: 'sleep session'
        };
        SleepViewModel.prototype = new BaseVm();
        SleepViewModel.prototype.constructor = Feeding;
        return new SleepViewModel(app, dataModel, options);
    }
});