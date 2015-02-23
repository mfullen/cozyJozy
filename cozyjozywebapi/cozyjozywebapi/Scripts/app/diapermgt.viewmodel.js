﻿function DC(data) {
    this.id = ko.observable(data.id);
    this.occurredOn = ko.observable(data.occurredOn).extend({ required: true });
    this.notes = ko.observable(data.notes);
    this.urine = ko.observable(data.urine);
    this.stool = ko.observable(data.stool);
    this.childId = ko.observable(data.childId);
}

function DiaperManagement(app, dataModel) {
    var self = this;



    self.diaperChange = ko.observable(new DC({
        id: 0,
        occurredOn: '',
        notes: '',
        urine: false,
        stool: false
    }));

    self.diaperChanges = ko.observableArray();

    self.isEditing = ko.observable(false);
    self.isCreatingNew = ko.observable(false);

    self.canSave = function () {

    }

    self.openNew = function () {
        self.isEditing(false);
        self.isCreatingNew(true);
    }

    self.create = function () {
     
        self.diaperChange().childId(app.selectedChild().id);
    
        console.log("ko.toJson.Diaper", ko.toJSON(self.diaperChange));
        console.log("self.diaperChange", self.diaperChange());
        $.ajax({
            url: 'api/diaperchanges',
            cache: 'false',
            type: 'POST',
            contentType: 'application/json',
            headers: dataModel.getSecurityHeaders(),
            data: ko.toJSON(self.diaperChange()),
            success: function (data) {
                self.diaperChanges.push(new DC(data));
                self.reset();
            }
        });
    }

    self.cancel = function () {
        self.reset();
    }

    self.reset = function () {
        self.diaperChange(null);
        self.isEditing(false);
        self.isCreatingNew(false);
    }

    self.edit = function (f) {
        self.diaperChange(f);
        self.isEditing(true);
    }

    self.update = function () {
        var dc = self.diaperChange();
        $.ajax({
            url: 'api/diaperchanges/' + dc.id(),
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
                url: 'api/diaperchanges/' + f.id(),
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
            for (var i = 0; i < data.length; i++) {
                self.diaperChanges.push(new DC(data[i]));
            }
        }
    });

}

app.addViewModel({
    name: "Diaper",
    bindingMemberName: "diapermgt",
    factory: DiaperManagement
});