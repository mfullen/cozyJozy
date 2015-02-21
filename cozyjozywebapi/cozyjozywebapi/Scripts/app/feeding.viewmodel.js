function Feeding(app, dataModel) {
    var self = this;
    self.id = ko.observable("");
    self.startTime = ko.observable("");
    self.endTime = ko.observable("");
    self.deliveryType = ko.observable("");
    self.amount = ko.observable("");
    self.dateReported = ko.observable("");
    self.childId = ko.observable("");
    self.spitUp = ko.observable();



    self.feedings = ko.observableArray();

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

app.addViewModel({
    name: "Feeding",
    bindingMemberName: "feedingMgt",
    factory: Feeding
});