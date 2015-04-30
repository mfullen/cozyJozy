function HomeViewModel(app, dataModel) {
    var self = this;

    self.stats = ko.observable();

    self.appendTimeStringIfExists = function (org, duration, timeframe) {
        if (duration.get(timeframe) > 0) {
            org = org + duration.get(timeframe) + ' ' + timeframe + ' ';
        }
        return org;
    }

    self.timeSinceBirth = ko.computed(function () {
        if (self.stats()) {
            var dob = moment(self.stats().dateOfBirth());
            var duration = moment.duration(moment().diff(dob));

            var durationString = '';
            var timeframe = ['years', 'months', 'days', 'hours', 'minutes'];

            for (var i = 0; i < timeframe.length; i++) {
                durationString = self.appendTimeStringIfExists(durationString, duration, timeframe[i]);
            }

            return durationString;
        }
        return null;
    }, self);

    self.timeTillBirthday = ko.computed(function () {
        if (self.stats()) {
            var dob = moment(self.stats().dateOfBirth());
            var d = moment.duration(moment().diff(dob));
            var durationYears = d.get('years');
            var nextBirthday = moment(self.stats().dateOfBirth());
            nextBirthday.add(durationYears, 'years');
            var nbHasPassed = nextBirthday.isBefore(moment());

            if (nbHasPassed) {
                nextBirthday.add(1, 'years');
            }
           
            var duration = moment.duration(nextBirthday.diff(moment()));

            var durationString = '';
            var timeframe = ['years', 'months', 'days', 'hours', 'minutes'];

            for (var i = 0; i < timeframe.length; i++) {
                durationString = self.appendTimeStringIfExists(durationString, duration, timeframe[i]);
            }

            return durationString;
        }
        return null;
    }, self);

    self.timeSinceLastFeeding = ko.computed(function () {
        if (self.stats()) {
            if (self.stats().lastFeeding()) {
                return moment(self.stats().lastFeeding()).fromNow();
            }
            return "None";
        }
        return null;
    }, self);

    self.timeSinceLastDiaperChange = ko.computed(function () {
        if (self.stats()) {
            if (self.stats().lastDiaperChange()) {
                return moment(self.stats().lastDiaperChange()).fromNow();
            }
            return "None";
        }
        return null;
    }, self);

    self.dob = ko.computed(function () {
        if (self.stats()) {
            return moment(self.stats().dateOfBirth()).format('MMMM Do YYYY, h:mm a');
        }
        return null;
    }, self);

    self.convertToOz = function (v) {
        return self.round2Decimals(v * 0.033814)
    }

    self.round2Decimals = function (v) {
        return +(Math.round((v) + "e+2") + "e-2");
    }


    $.ajax({
        url: 'api/dashboard',
        cache: false,
        headers: dataModel.getSecurityHeaders(),
        data: { childId: app.selectedChild().child().id() },
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
