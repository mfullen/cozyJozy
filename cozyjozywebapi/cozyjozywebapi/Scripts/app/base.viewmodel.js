function BaseVm(app, dataModel, options) {
    var self = this;

    //options = options || {
    //    newItem: function () {
    //     return;
    //    },
    //    url: 'api/test',
    //itemName: 'testName'};
    var modelFunction = options.modelFunc;
    var newItemFunction = options.newItem;
    var baseUrl = options.url;
    var itemName = options.itemName;


    var clearErrors = function () {
        app.errors.removeAll();
    }

    var addError = function (error) {
        app.errors.push(error);
    }

    self.item = ko.observable(newItemFunction());

    self.items = ko.observableArray();

    self.isEditing = ko.observable(false);
    self.isCreatingNew = ko.observable(false);

    self.openNew = function () {
        self.isEditing(false);
        self.item(newItemFunction());
        self.isCreatingNew(true);
    }

    self.create = function () {
        self.item().childId(app.selectedChild().child().id());

        $.ajax({
            url: baseUrl,
            cache: 'false',
            type: 'POST',
            contentType: 'application/json',
            headers: dataModel.getSecurityHeaders(),
            data: ko.toJSON(self.item()),
            success: function (data) {
                self.items.push(new modelFunction(data));
                self.reset();
            },
            error: function (xhr, textStatus, err) {
                addError("Failed to create " + itemName + ". Please try again!");
                console.log("Error", xhr, textStatus, err);
            }
        });
    }

    self.cancel = function () {
        self.reset();
    }

    self.reset = function () {
        self.item(newItemFunction());
        self.isEditing(false);
        self.isCreatingNew(false);
        clearErrors();
    }

    self.edit = function (f) {
        self.item(new modelFunction(ko.toJS(f)));
        self.isEditing(true);
        self.isCreatingNew(false);
    }

    self.update = function () {
        var dc = self.item();
        $.ajax({
            url: baseUrl + '/' + dc.id(),
            cache: 'false',
            type: 'PUT',
            contentType: 'application/json',
            headers: dataModel.getSecurityHeaders(),
            data: ko.toJSON(dc),
            success: function (data) {
                var target = ko.utils.arrayFirst(self.items(), function (titem) {
                    return titem.id() === data.id;
                });

                self.items.replace(target, new modelFunction(data));
                self.reset();
            },
            error: function (xhr, textStatus, err) {
                addError("Failed to update " + itemName + ". Please try again!");
                console.log("Error", xhr, textStatus, err);
            }
        });
    }

    self.deleteItem = function (f) {
        if (confirm('Are you sure you want to Delete this ' + itemName +'?')) {
            $.ajax({
                url: baseUrl + '/' + f.id(),
                cache: 'false',
                type: 'DELETE',
                contentType: 'application/json',
                headers: dataModel.getSecurityHeaders(),
                success: function (data) {
                    self.items.remove(f);
                },
                error: function (xhr, textStatus, err) {
                    addError("Failed to delete the record.");
                    console.log("Error", xhr, textStatus, err);
                }
            });
        }
    }

    self.fetchItems = function() {
        $.ajax({
            url: baseUrl,
            cache: false,
            headers: dataModel.getSecurityHeaders(),
            data: { childId: app.selectedChild().child().id() },
            contentType: 'json',
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    self.items.push(new modelFunction(data[i]));
                }
            },
            error: function (xhr, textStatus, err) {
                addError("Failed to retrieve " + itemName + "s. Please try again!");
                console.log("Error", xhr, textStatus, err);
            }
        });
    }
   
    self.testMe = function() {
        console.log("TEST ME WORKS AS BASE CLASS");
    }
}