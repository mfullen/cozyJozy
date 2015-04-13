function ChildPermissionViewModel(app, dataModel, options) {
    var self = this;
    BaseVm.call(self, app, dataModel, options);

    self.canSave = function () {

    }

    self.sortedByDate = ko.computed(function () {
        var s = self.items.slice(0).sort(function (l, r) {
            return r.user().userName().compare(l.user().userName()) ? -1 : 1;
        });
        return s;
    }, self);

    self.fetchItems();
}

app.addViewModel({
    name: "ChildPermissionViewModel",
    bindingMemberName: "cpvm",
    factory: function (app, dataModel) {
        var newDc = function () {
            return new Permission({
                id: 0,
                child: ko.toJS(app.selectedChild().child()),
                readOnly: true,
                user: { id: 0 }
        });
        };
        var options = {
            modelFunc: Permission,
            newItem: newDc,
            url: 'api/childpermission',
            itemName: 'Permission'
        };
        ChildPermissionViewModel.prototype = new BaseVm(app, dataModel, options);
        ChildPermissionViewModel.prototype.constructor = ChildPermissionViewModel;

        return ChildPermissionViewModel(app, dataModel, options);
    }
});