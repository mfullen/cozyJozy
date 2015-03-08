function DashboardStats(data) {
    this.dateOfBirth = ko.observable(data.dateOfBirth);
    this.lastFeeding = ko.observable(data.lastFeeding);
    this.lastDiaperChange = ko.observable(data.lastDiaperChange);
    this.totalFeedings = ko.observable(data.totalFeedings);
    this.totalDiaperChanges = ko.observable(data.totalDiaperChanges);
    this.numberOfRecentFeedings = ko.observable(data.numberOfRecentFeedings);
    this.numberOfRecentDiaperChanges = ko.observable(data.numberOfRecentDiaperChanges);
    this.recentAmountPerFeed = ko.observable(data.recentAmountPerFeed);
    this.totalAmountPerFeed = ko.observable(data.totalAmountPerFeed);
    this.totalRecentAmount = ko.observable(data.totalRecentAmount);
    this.childId = ko.observable(data.childId);
}

function HomeViewModel(app, dataModel) {
    var self = this;

    self.stats = ko.observable();

    self.timeSinceBirth = ko.computed(function () {
        if (self.stats()) {
            return moment(self.stats().dateOfBirth()).fromNow();
        }
        return null;
    }, self);

    self.timeTillBirthday = ko.computed(function () {
        if (self.stats()) {
            //todo need to fix
            var dob = moment(self.stats().dateOfBirth());
            var nowDob = moment();
            nowDob.month(dob.month());
            nowDob.day(dob.day());

            var hasPast = moment().isAfter(nowDob);
            if (hasPast) {
                nowDob.year(nowDob.year() + 1);
            }
            return moment(nowDob).from();
        }
        return null;
    }, self);

    self.timeSinceLastFeeding = ko.computed(function () {
        if (self.stats()) {
            return moment(self.stats().lastFeeding()).fromNow();
        }
        return null;
    }, self);

    self.timeSinceLastDiaperChange = ko.computed(function () {
        if (self.stats()) {
            return moment(self.stats().lastDiaperChange()).fromNow();
        }
        return null;
    }, self);

    self.dob = ko.computed(function () {
        if (self.stats()) {
            return moment(self.stats().dateOfBirth()).format('MMMM Do YYYY, h:mm a');
        }
        return null;
    }, self);

    self.convertToOz = function(v) {
        return self.round2Decimals(v * 0.033814)
    }

    self.round2Decimals = function(v) {
        return +(Math.round((v) + "e+2") + "e-2");
    }


    $.ajax({
        url: 'api/dashboard',
        cache: false,
        headers: dataModel.getSecurityHeaders(),
        data: { childId: app.selectedChild().id },
        contentType: 'json',
        success: function (data) {
            self.stats(new DashboardStats(data));
        },
        error: function (xhr, textStatus, err) {
            app.errors.push("Failed to retrieve Dashboard stats. Please try again!");
            console.log("Error", xhr, textStatus, err);
        }
    });
    // HomeViewModel currently does not require data binding, so there are no visible members.
}

app.addViewModel({
    name: "Home",
    bindingMemberName: "home",
    factory: HomeViewModel
});
