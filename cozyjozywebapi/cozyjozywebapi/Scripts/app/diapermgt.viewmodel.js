function DiaperManagement(app, dataModel, options) {
    BaseVm.apply(this, arguments);
    var self = this;

    self.canSave = function () {

    }

    self.sortedByDate = ko.computed(function () {
        var s = self.items.slice(0).sort(function (l, r) {
            return moment(r.occurredOn()).isBefore(moment(l.occurredOn())) ? -1 : 1;
        });
        return s;
    }, self);

    self.fetchItems();
}

app.addViewModel({
    name: "Diaper",
    bindingMemberName: "diapermgt",
    factory: function(app, dataModel) {
        var newDc = function() {
            return new DC({
                id: 0,
                occurredOn: '',
                notes: '',
                urine: false,
                stool: false
            });
        };
        var options = {
            modelFunc: DC,
            newItem: newDc,
            url: 'api/diaperchanges',
            itemName: 'diaper'
        };
        DiaperManagement.prototype = new BaseVm(app, dataModel, options);
        DiaperManagement.prototype.constructor = DiaperManagement;

        return new DiaperManagement(app, dataModel, options);
    }
});