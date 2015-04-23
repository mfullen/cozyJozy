function Feeding(app, dataModel, options) {

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

    //The selected date we are looking at for feedings
    self.sDate = ko.observable();

    self.canSave = function () {

    }

    self.sortedFeedingsByDate = ko.computed(function () {
        var s = self.items().slice(0).sort(function (l, r) {
            return moment(r.startTime()).isBefore(moment(l.startTime())) ? -1 : 1;
        });
        return s;
    }, self);

    self.mlToOz = function(ml) {
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

    self.previousDateButtonEnabled = ko.observable(true);
    self.nextDateButtonEnabled = ko.observable(true);

    self.toggleDateButtons = function () {
        self.previousDateButtonEnabled(!self.previousDateButtonEnabled());
        self.nextDateButtonEnabled(!self.nextDateButtonEnabled());
    }

    self.sDate.subscribe(function (newValue) {
        self.items.removeAll();
        self.toggleDateButtons();
        self.fetchItems({
            childId: app.selectedChild().child().id(),
            startDate: self.sDate(),
            endDate: self.sDate()
        }, self.toggleDateButtons, self.toggleDateButtons);
    });

    var dateFormater = 'MM/DD/YYYY';
    var dateTimeFormater = 'hh:mm a z';

    self.formattedDateTime = function (d, t) {
        if (t === 'time') {
            return moment(d).format(dateTimeFormater);
        } else {
            return moment(d).format(dateFormater);
        }
    }

    self.amountText = function (s) {
        if (!s.amount()) {
            return null;
        }
        return self.mlToOz(s.amount()) + ' oz (' + s.amount() + ' ml)';
    }

    self.previousDate = function () {
        var m = moment(self.sDate());
        self.sDate(m.subtract('days', 1).format(dateFormater));
    }

    self.nextDate = function () {
        var m = moment(self.sDate());
        self.sDate(m.add('days', 1).format(dateFormater));
    }

    self.sDate(moment().format(dateFormater));
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
                endTime: moment().add('minutes', 15).format(dateFormater),
                dateReported: '',
                spitUp: '',
                deliveryType: 2,
            });
        }

        var options = {
            modelFunc: FEED,
            newItem: newFeed,
            url: 'api/feeding',
            itemName: 'feeding'
        };
        Feeding.prototype = new BaseVm();
        Feeding.prototype.constructor = Feeding;
        return new Feeding(app, dataModel, options);
    }
});