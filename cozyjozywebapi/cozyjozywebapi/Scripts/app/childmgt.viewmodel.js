

function ChildManagement(app, dataModel) {
    var self = this;

    var newChild = function() {
        return new ChildClass({
            id: 0,
            male: false,
            lastName: '',
            middleName: ''
        });
    };

    var clearErrors = function () {
        app.errors.removeAll();
    }

    var addError = function (error) {
        app.errors.push(error);
    }

    self.item = ko.observable(newChild());
    self.items = ko.observableArray(dataModel.getChildren());

    self.isEditing = ko.observable(false);
    self.isCreatingNew = ko.observable(false);

    self.reset = function () {
        self.item(newChild());
        self.isEditing(false);
        self.isCreatingNew(false);
        clearErrors();
    }

    self.openNew = function () {
        self.isEditing(false);
        self.item(newChild());
        self.isCreatingNew(true);
    }


    self.create = function () {
        $.ajax({
            url: 'api/children',
            cache: 'false',
            type: 'POST',
            contentType: 'application/json',
            headers: dataModel.getSecurityHeaders(),
            data: ko.toJSON(self.item()),
            success: function (data) {
                self.items.push(new ChildPermission(data));
                self.reset();
                dataModel.setChildren(self.items());
            },
            error: function (xhr, textStatus, err) {
                addError("Failed to create " + itemName + ". Please try again!");
                console.log("Error", xhr, textStatus, err);
            }
        });
    }

    self.edit = function (c) {
        self.item(new ChildClass(ko.toJS(c.child)));
        self.isEditing(true);
        self.isCreatingNew(false);
    }

    self.update = function () {
        var uChild = self.item();
        $.ajax({
            url: 'api/children/' + uChild.id(),
            cache: 'false',
            type: 'PUT',
            contentType: 'application/json',
            headers: dataModel.getSecurityHeaders(),
            data: ko.toJSON(uChild),
            success: function (data) {
                var target = ko.utils.arrayFirst(self.items(), function (titem) {
                    return titem.child().id() === data.child.id;
                });

                self.items.replace(target, new ChildPermission(data));
                self.reset();
                dataModel.setChildren(self.items());
            },
            error: function (xhr, textStatus, err) {
                addError("Failed to update Please try again!");
                console.log("Error", xhr, textStatus, err);
            }
        });
    }

    self.deleteItem = function (c) {
        var theChild = c.child();
        if (confirm('Are you sure you want to Delete ' + theChild.firstName() + ' ?')) {
            $.ajax({
                url: 'api/children/' + theChild.id(),
                cache: 'false',
                type: 'DELETE',
                contentType: 'application/json',
                headers: dataModel.getSecurityHeaders(),
                success: function (data) {
                    self.items.remove(c);
                    dataModel.setChildren(self.items());
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

    self.canSave = function () {
        
    }

}

app.addViewModel({
    name: "ChildManagement",
    bindingMemberName: "childmgt",
    factory: ChildManagement
});