function FEED(data) {
    this.id = ko.observable(data.id);
    this.startTime = ko.observable(data.startTime).extend({ required: true });
    this.endTime = ko.observable(data.endTime).extend({ required: true });
    this.deliveryType = ko.observable(data.deliveryType);
    this.amount = ko.observable(data.amount).extend({ numeric: 2 });
    this.dateReported = ko.observable(data.dateReported);
    this.childId = ko.observable(data.childId);
    this.spitUp = ko.observable(data.spitUp);
}

function Feeding(app, dataModel, options) {
    var self = this;
    BaseVm.call(self, app, dataModel, options);

    self.deliveryTypes = [
        { value: 0, name: "Left Breast", code: "LB" },
        { value: 1, name: "Right Breast", code: "RB" },
        { value: 2, name: "Bottle", code: "B" }
    ];

    self.getDeliveryType = function(f) {
        for (var i = 0; i < self.deliveryTypes.length; i++) {
            var dt = self.deliveryTypes[i];
            if (f === dt.value) {
                return dt.name;
            }
        }
        return null;
    };

   


    self.canSave = function () {

    }

    self.sortedFeedingsByDate = ko.computed(function() {
        var s = self.items().slice(0).sort(function(l, r) {
            return moment(r.startTime()).isBefore(moment(l.startTime())) ? -1 : 1;
        });
        return s;
    }, self);

   

    self.amountOunces = ko.computed(function () {
        if (!self.item() || !self.item().amount() || self.item().amount() < 0)
            return 0;
        return +(Math.round((self.item().amount() * 0.033814) + "e+2") + "e-2");
    }, self);

    self.getText = function(d) {
        //60 mL (2 oz) @ 2/22/2015 7:45 pm  - 2/22/2015 8:00 pm 
        var amountText = '';
        if (d.amount()) {
            var amountOz = +(Math.round((d.amount() * 0.033814) + "e+2") + "e-2");
            amountText = amountText + d.amount() + ' mL (' + amountOz + ' oz) from ';
        }

        var rowText = amountText  + self.getDeliveryType(d.deliveryType()) + ' @ ';
        rowText = rowText + moment(d.startTime()).format('hh:mm a') + ' for ' + self.getTimeDiff(d) + ' minutes';
        return rowText;
    }

    self.getTimeDiff = function(d) {
        var sd = moment(d.startTime());
        var ed = moment(d.endTime());
        return ed.diff(sd, 'minutes');
    }

    self.fetchItems();
}

//ko.bindingHandlers.numeric = {
//    init: function (element, valueAccessor) {
//        $(element).on("keydown", function (event) {
//            // Allow: backspace, delete, tab, escape, and enter
//            if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
//                // Allow: Ctrl+A
//                (event.keyCode == 65 && event.ctrlKey === true) ||
//                // Allow: . ,
//                (event.keyCode == 188 || event.keyCode == 190 || event.keyCode == 110) ||
//                // Allow: home, end, left, right
//                (event.keyCode >= 35 && event.keyCode <= 39)) {
//                // let it happen, don't do anything
//                return;
//            }
//            else {
//                // Ensure that it is a number and stop the keypress
//                if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
//                    event.preventDefault();
//                }
//            }
//        });
//    }
//};

app.addViewModel({
    name: "Feeding",
    bindingMemberName: "feedingMgt",
    factory: function (app, dataModel) {
        var newFeed = function () {
            return new FEED({
                id: 0,
                startTime: '',
                endTime: '',
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
        Feeding.prototype = new BaseVm(app, dataModel, options);
        Feeding.prototype.constructor = Feeding;
        return Feeding(app, dataModel, options);
    }
});