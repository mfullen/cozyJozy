function DC(data) {
    this.id = ko.observable(data.id);
    this.occurredOn = ko.observable(data.occurredOn).extend({ required: true });
    this.notes = ko.observable(data.notes);
    this.urine = ko.observable(data.urine);
    this.stool = ko.observable(data.stool);
    this.childId = ko.observable(data.childId);
}

function DiaperManagement(app, dataModel,options) {
    var self = this;
    BaseVm.call(self, app, dataModel, options);

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

        return DiaperManagement(app, dataModel, options);
    }
});