function HomeViewModel(app, dataModel) {
    var self = this;

    self.id = ko.observable("");
    self.startTime = ko.observable("");
    self.endTime = ko.observable("");
    self.breast = ko.observable("");
    self.amount = ko.observable("");
    self.dateReported = ko.observable("");
    self.childId = ko.observable("");

    var Feeding = {
        Id: self.id,
        StartTime: self.startTime,
        EndTime: self.endTime,
        Breast: self.breast,
        Amount: self.amount,
        DateReported: self.dateReported,
        ChildId: self.childId
    };

    self.feedings = ko.observableArray();

    $.ajax({
        url: 'api/feeding',
        cache: false,
        headers:  dataModel.getSecurityHeaders,
        contentType: 'json',
        success: function(data) {
            self.feedings(data);
        }
    });

    // HomeViewModel currently does not require data binding, so there are no visible members.
}

app.addViewModel({
    name: "Home",
    bindingMemberName: "home",
    factory: HomeViewModel
});
