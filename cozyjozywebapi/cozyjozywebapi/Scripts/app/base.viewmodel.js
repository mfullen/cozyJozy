function BaseVm(app, dataModel, options) {
    if (arguments.length == 0) return;
    var self = this;

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

    //The selected date we are looking at for feedings
    self.sDate = ko.observable();

    self.dateFormater = 'MM/DD/YYYY';
    self.dateTimeFormater = 'hh:mm a z';

    self.previousDateButtonEnabled = ko.observable(true);
    self.nextDateButtonEnabled = ko.observable(true);

    self.toggleDateButtons = function () {
        self.previousDateButtonEnabled(!self.previousDateButtonEnabled());
        self.nextDateButtonEnabled(!self.nextDateButtonEnabled());
    }

    self.sDate.subscribe(function (newValue) {
        self.sDateChanged(newValue);
    });

    self.sDateChanged = function (newValue) {
        self.items.removeAll();
        self.toggleDateButtons();
        self.fetchItems({
            childId: app.selectedChild().child().id(),
            startDate: self.sDate(),
            endDate: self.sDate()
        }, self.toggleDateButtons, self.toggleDateButtons);
    }

    app.selectedChild.subscribe(function (newValue) {
        self.sDateChanged(self.sDate());
    });

    self.formattedDateTime = function (d, t) {
        if (t === 'time') {
            return moment(d).format(self.dateTimeFormater);
        } else {
            return moment(d).format(self.dateFormater);
        }
    }

    self.previousDate = function () {
        var m = moment(self.sDate());
        self.sDate(m.subtract(1, 'days').format(self.dateFormater));
    }

    self.nextDate = function () {
        var m = moment(self.sDate());
        self.sDate(m.add(1, 'days').format(self.dateFormater));
    }

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
        if (confirm('Are you sure you want to Delete this ' + itemName + '?')) {
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

    self.fetchItems = function (p, sfunc, ffunc) {
        var pp = p || { childId: app.selectedChild().child().id() };
        $.ajax({
            url: baseUrl,
            cache: false,
            headers: dataModel.getSecurityHeaders(),
            data: pp,
            contentType: 'json',
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    self.items.push(new modelFunction(data[i]));
                }

                if (sfunc != null) {
                    sfunc();
                }
            },
            error: function (xhr, textStatus, err) {
                addError("Failed to retrieve " + itemName + "s. Please try again!");
                console.log("Error", xhr, textStatus, err);
                if (ffunc != null) {
                    ffunc();
                }
            }
        });
    }

    self.canAdd = ko.computed(function () {
        return !app.selectedChild().readOnly;
    }, self);

    self.canEdit = ko.computed(function () {
        return !app.selectedChild().readOnly;
    }, self);

    self.canDelete = ko.computed(function () {
        return !app.selectedChild().readOnly;
    }, self);

    self.testMe = function () {
        console.log("TEST ME WORKS AS BASE CLASS");
    }

    self.sDate(moment().format(self.dateFormater));
}