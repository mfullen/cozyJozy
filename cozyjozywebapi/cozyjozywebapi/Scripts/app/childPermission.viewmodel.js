function ChildPermissionViewModel(app, dataModel, options) {
    BaseVm.apply(this, arguments);
    var self = this;

    self.availableTitles = app.availableTitles();

    self.canSave = function () {

    }

   
    self.sortedByDate = ko.computed(function () {
        var s = self.items.slice(0).sort(function (l, r) {
            return r.user().userName() > l.user().userName() ? -1 : 1;
        });
        return s;
    }, self);
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
        ChildPermissionViewModel.prototype = new BaseVm();
        ChildPermissionViewModel.prototype.constructor = ChildPermissionViewModel;

        return new ChildPermissionViewModel(app, dataModel, options);
    }
});