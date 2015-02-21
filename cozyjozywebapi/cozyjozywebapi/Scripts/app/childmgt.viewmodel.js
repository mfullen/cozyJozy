function ChildManagement(app, dataModel) {
    var self = this;

    self.id = ko.observable("");
    self.dob = ko.observable("").extend({ required: true });
    self.firstName = ko.observable("").extend({ required: true });
    self.middleName = ko.observable("");
    self.lastName = ko.observable("");
    self.male = ko.observable(false);

    self.child = ko.observable();
    self.children = ko.observableArray(dataModel.getChildren());

    self.reset = function () {
        self.dob("");
        self.firstName("");
        self.lastName("");
        self.middleName("");
        self.male(false);
        self.child(null);
    }

    self.create = function () {
        var childObj = {
            firstName: self.firstName,
            lastName: self.lastName,
            dateOfBirth: self.dob,
            middleName: self.middleName,
            male: self.male
        };

        self.child(childObj);

        $.ajax({
            url: 'api/children',
            cache: 'false',
            type: 'POST',
            contentType: 'application/json',
            headers: dataModel.getSecurityHeaders(),
            data: ko.toJSON(self.child),
            success: function (data) {
                self.children.push(data);
                self.reset();
                dataModel.setChildren(self.children());
            }
        });
    }

    self.edit = function (c) {
        self.child(c);
    }

    self.update = function () {
        var uChild = self.child();
        $.ajax({
            url: 'api/children/' + uChild.id,
            cache: 'false',
            type: 'PUT',
            contentType: 'application/json',
            headers: dataModel.getSecurityHeaders(),
            data: ko.toJSON(uChild),
            success: function (data) {
                var tempChildren = ko.observableArray();
                tempChildren(self.children().slice(0));
                self.children.removeAll();
                self.children(tempChildren());

                self.reset();
                dataModel.setChildren(self.children());
            },
            failure: function (xhr, textStatus, err) {
                console.log("Error", xhr, textStatus, err);
            }
        });
    }

    self.delete = function(c) {
        
        if (confirm('Are you sure you want to Delete ' + c.firstName + ' ?')) {
            $.ajax({
                url: 'api/children/' + c.id,
                cache: 'false',
                type: 'DELETE',
                contentType: 'application/json',
                headers: dataModel.getSecurityHeaders(),
                success: function (data) {
                    self.children.remove(c);
                    dataModel.setChildren(self.children());
                },
                failure: function (xhr, textStatus, err) {
                    console.log("Error", xhr, textStatus, err);
                }
            });
        }
    }

    self.cancel = function () {
        self.reset();
    }

    self.canSave = ko.computed(function () {
        return (self.firstName() != "" && self.dob() != "");
    });

    //$.ajax({
    //    url: 'api/children',
    //    cache: false,
    //    headers: dataModel.getSecurityHeaders(),
    //    contentType: 'json',
    //    success: function (data) {
    //        //var vm = ko.mapping.fromJS(data);
    //        //self.children(vm());
    //        self.children(data);
    //    }
    //});
}

app.addViewModel({
    name: "ChildManagement",
    bindingMemberName: "childmgt",
    factory: ChildManagement
});