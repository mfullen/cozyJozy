﻿function Feeding(app, dataModel, options) {

    BaseVm.apply(this, arguments);

    var self = this;
    self.deliveryTypes = [
        { value: 0, name: "Left Breast", code: "LB" },
        { value: 1, name: "Right Breast", code: "RB" },
        { value: 2, name: "Bottle", code: "B" }
    ];

    self.getDeliveryType = function (f) {
        for (var i = 0; i < self.deliveryTypes.length; i++) {
            var dt = self.deliveryTypes[i];
            if (f === dt.value) {
                return dt.name;
            }
        }
        return null;
    };

    self.deliveryTypeClassTheme = function (dt) {
        var css = "c-white bgm-indigo";
        switch (dt) {
            case 0:
                css = "c-white bgm-teal";
                break;
            case 1:
                css = "c-white bgm-purple";
                break;
        }
        return css;
    }


    self.canSave = ko.computed(function() {
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

        //Must have delivery Type
        if (!self.item().deliveryType()) {
            self.addError('Delivery Type is required.');
            return false;
        }

        if (self.item().deliveryType() == 2) {

            if (!self.item().amount() || self.item().amount() <= 0) {
                self.addError('Amount must be greater than zero.');
                return false;
            }
        }

        //Start Date must be before End Date
        var startBeforeEnd = moment(self.item().startTime()).isBefore(moment(self.item().endTime()));
        if (!startBeforeEnd) {
            self.addError('Start date/time must come before the End date/time.');
            return false;
        }
        return true;

    }, self);

    self.sortedFeedingsByDate = ko.computed(function () {
        var s = self.items().slice(0).sort(function (l, r) {
            return moment(r.startTime()).isBefore(moment(l.startTime())) ? -1 : 1;
        });
        return s;
    }, self);

    self.mlToOz = function (ml) {
        return +(Math.round((ml * 0.033814) + "e+2") + "e-2");
    }

    self.amountOunces = ko.computed(function () {
        if (!self.item() || !self.item().amount() || self.item().amount() < 0)
            return 0;
        return +(Math.round((self.item().amount() * 0.033814) + "e+2") + "e-2");
    }, self);

    self.getText = function (d) {
        //60 mL (2 oz) @ 2/22/2015 7:45 pm  - 2/22/2015 8:00 pm 
        var amountText = '';
        if (d.amount()) {
            var amountOz = +(Math.round((d.amount() * 0.033814) + "e+2") + "e-2");
            amountText = amountText + d.amount() + ' mL (' + amountOz + ' oz) from ';
        }

        var rowText = amountText + self.getDeliveryType(d.deliveryType());
        rowText = rowText + ' for ' + self.getTimeDiff(d) + ' minutes';

        return rowText;
    }

    self.getTimeDiff = function (d) {
        var sd = moment(d.startTime());
        var ed = moment(d.endTime());
        return ed.diff(sd, 'minutes');
    }

    self.amountText = function (s) {
        if (!s.amount()) {
            return null;
        }
        return self.mlToOz(s.amount()) + ' oz (' + s.amount() + ' ml)';
    }
}

app.addViewModel({
    name: "Feeding",
    bindingMemberName: "feedingMgt",
    factory: function (app, dataModel) {
        var dateFormater = 'MM/DD/YYYY hh:mm a';
        var newFeed = function () {
            return new FEED({
                id: 0,
                startTime: moment().format(dateFormater),
                endTime: moment().add(15, 'minutes').format(dateFormater),
                dateReported: '',
                spitUp: '',
                deliveryType: 2
            });
        }

        var options = {
            modelFunc: FEED,
            newItem: newFeed,
            url: 'api/feeding',
            itemName: 'feeding',
            canCrud: app.canWriteFeedings()
        };
        Feeding.prototype = new BaseVm();
        Feeding.prototype.constructor = Feeding;
        return new Feeding(app, dataModel, options);
    }
});