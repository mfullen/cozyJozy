function DiaperManagement(app, dataModel) {
    var self = this;

    self.id = ko.observable("");
    self.occurredOn = ko.observable("").extend({ required: true });
    self.notes = ko.observable("");
    self.urine = ko.observable(false);
    self.stool = ko.observable(false);

    self.diaperChange = ko.observable();
    self.diaperChanges = ko.observableArray();

    self.canSave = function () {

    }

    self.create = function () {
        var dc2Send = {
            occurredOn: self.occurredOn,
            notes: self.notes,
            urine: self.urine,
            stool: self.stool,
            childId: app.selectedChild().id
        };

        self.diaperChange(dc2Send);
        console.log("ko.toJson.Diaper", ko.toJSON(self.diaperChange));
        console.log("self.diaperChange", self.diaperChange());
        $.ajax({
            url: 'api/diaperchanges',
            cache: 'false',
            type: 'POST',
            contentType: 'application/json',
            headers: dataModel.getSecurityHeaders(),
            data: ko.toJSON(self.diaperChange),
            success: function (data) {
                self.diaperChanges.push(data);
                self.reset();
            }
        });
    }

    self.cancel = function () {
        self.reset();
    }

    self.reset = function () {
        self.id("");
        self.occurredOn("");
        self.notes("");
        self.urine(false);
        self.stool(false);
        self.diaperChange(null);
    }

    self.edit = function (f) {
        self.diaperChange(f);
    }

    self.update = function () {
        var dc = self.diaperChange();
        $.ajax({
            url: 'api/diaperchanges/' + dc.id,
            cache: 'false',
            type: 'PUT',
            contentType: 'application/json',
            headers: dataModel.getSecurityHeaders(),
            data: ko.toJSON(dc),
            success: function (data) {
                var temp = ko.observableArray();
                temp(self.diaperChanges().slice(0));
                self.diaperChanges.removeAll();
                self.diaperChanges(temp());

                self.reset();
            },
            failure: function (xhr, textStatus, err) {
                console.log("Error", xhr, textStatus, err);
            }
        });
    }

    self.delete = function (f) {
        if (confirm('Are you sure you want to Delete this diaper change?')) {
            $.ajax({
                url: 'api/diaperchanges/' + f.id,
                cache: 'false',
                type: 'DELETE',
                contentType: 'application/json',
                headers: dataModel.getSecurityHeaders(),
                success: function (data) {
                    self.diaperChanges.remove(f);
                },
                failure: function (xhr, textStatus, err) {
                    console.log("Error", xhr, textStatus, err);
                }
            });
        }
    }

    $.ajax({
        url: 'api/diaperchanges',
        cache: false,
        headers: dataModel.getSecurityHeaders(),
        data: { childId: app.selectedChild().id },
        contentType: 'json',
        success: function (data) {
            self.diaperChanges(data);
        }
    });

}

app.addViewModel({
    name: "Diaper",
    bindingMemberName: "diapermgt",
    factory: DiaperManagement
});