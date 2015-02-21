function Feeding(app, dataModel) {
    var self = this;

    self.id = ko.observable("");
    self.startTime = ko.observable("").extend({ required: true });
    self.endTime = ko.observable("").extend({ required: true });
    self.deliveryType = ko.observable("");
    self.amount = ko.observable().extend({ numeric: 2 });
    self.dateReported = ko.observable("");
    self.childId = ko.observable("");
    self.spitUp = ko.observable();


   

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

    self.feeding = ko.observable();
    self.feedings = ko.observableArray();

    self.canSave = function () {

    }

    self.create = function () {
        var feed2Send = {
            startTime: self.startTime,
            endTime: self.endTime,
            deliveryType: self.deliveryType,
            amount: self.amount,
            spitUp: self.spitUp,
            childId: app.selectedChild().id
        };

        self.feeding(feed2Send);
        var d2Send = ko.toJSON(self.feeding);
        console.log(d2Send);
        $.ajax({
            url: 'api/feeding',
            cache: 'false',
            type: 'POST',
            contentType: 'application/json',
            headers: dataModel.getSecurityHeaders(),
            data: ko.toJSON(self.feeding),
            success: function (data) {
                self.feedings.push(data);
                self.reset();
            }
        });
    }

    self.cancel = function () {
        self.reset();
    }

    self.reset = function() {
        self.id("");
        self.startTime("");
        self.endTime("");
        self.deliveryType("");
        self.amount(null);
        self.dateReported("");
        self.childId(null);
        self.spitUp(false);
    }

    self.edit = function(f) {
        self.feeding(f);
    }
    
    self.update = function() {
        
    }

    self.delete = function() {
        
    }

    self.amountOunces = ko.computed(function () {
        if (!self.amount() || self.amount() < 0)
            return 0;
        return +(Math.round((self.amount() * 0.033814) + "e+2") + "e-2");
    }, self);

    self.amountOunces2 = ko.computed(function () {
        if (!self.feeding() || !self.feeding().amount || self.feeding().amount < 0)
            return 0;
        return +(Math.round((self.feeding().amount * 0.033814) + "e+2") + "e-2");
    }, self);

    $.ajax({
        url: 'api/feeding',
        cache: false,
        headers: dataModel.getSecurityHeaders(),
        data: { childId: app.selectedChild().id },
        contentType: 'json',
        success: function (data) {
            self.feedings(data);
        }
    });
}

ko.bindingHandlers.numeric = {
    init: function (element, valueAccessor) {
        $(element).on("keydown", function (event) {
            // Allow: backspace, delete, tab, escape, and enter
            if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
                // Allow: Ctrl+A
                (event.keyCode == 65 && event.ctrlKey === true) ||
                // Allow: . ,
                (event.keyCode == 188 || event.keyCode == 190 || event.keyCode == 110) ||
                // Allow: home, end, left, right
                (event.keyCode >= 35 && event.keyCode <= 39)) {
                // let it happen, don't do anything
                return;
            }
            else {
                // Ensure that it is a number and stop the keypress
                if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                    event.preventDefault();
                }
            }
        });
    }
};

app.addViewModel({
    name: "Feeding",
    bindingMemberName: "feedingMgt",
    factory: Feeding
});