function ChildPermissionViewModel(app, dataModel, options) {
    BaseVm.apply(this, arguments);
    var self = this;

    self.availableTitles = app.availableTitles();


    self.canSave = ko.computed(function () {
        self.clearErrors();

        if (!(self.isEditing() || self.isCreatingNew())) {
            return false;
        }

        if (!self.item().user()) {
            self.addError('Must have a user selected.');
            return false;
        }

        if (!self.item().user().userName()) {
            self.addError('Must have a user selected.');
            return false;
        }

        if (self.item().user().userName() === '') {
            self.addError('Must have a user selected.');
            return false;
        }

        return true;

    }, self);


    self.sortedByDate = ko.computed(function () {
        var s = self.items.slice(0).sort(function (l, r) {
            return r.user().userName() > l.user().userName() ? -1 : 1;
        });
        return s;
    }, self);

    self.onUpdated = function ($e, datum) {
        self.item().user(new User(datum));
    };

    self.baOptions = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('userName'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        //  prefetch: '../data/films/post_1960.json',
        remote: {
            url: "api/search?username=%QUERY",
            wildcard: '%QUERY',
            filter: function (data) {
                return data;
            }
        }
    });


    self.applyTypeAhead = function () {
        $(".typeahead")
        .typeahead({
            hint: false,
            minLength: 3,
            highlight: true
        }, {
            displayKey: 'userName',
            source: self.baOptions.ttAdapter(), // states,
            templates: {
                empty: [
                        '<div class="empty-message">',
                          'No matching User found',
                        '</div>'
                ].join('\n'),
                suggestion: Handlebars.compile('<div>{{userName}}</div>')
            }
        })
        .on('typeahead:autocompleted', self.onUpdated)
        .on('typeahead:selected', self.onUpdated); // for knockoutJS

        $("span.twitter-typeahead").attr('style', 'display: inline');
    }




    self.isCreatingNew.subscribe(function (newvalue) {
        self.applyTypeAhead();
    });

    self.isEditing.subscribe(function (newvalue) {
        self.applyTypeAhead();
    });


}

app.addViewModel({
    name: "ChildPermissionViewModel",
    bindingMemberName: "cpvm",
    factory: function (app, dataModel) {
        var newDc = function () {
            return new Permission({
                id: 0,
                child: ko.toJS(app.selectedChild().child()),
                user: { id: 0 },
                feedingWriteAccess: null,
                diaperChangeWriteAccess: null,
                sleepWriteAccess: null,
                measurementWriteAccess: null,
                childManagementWriteAccess: null,
                permissionsWriteAccess: null,
            });
        };
        var options = {
            modelFunc: Permission,
            newItem: newDc,
            url: 'api/childpermission',
            itemName: 'Permission',
            canCrud: app.canWritePermissions()
        };
        ChildPermissionViewModel.prototype = new BaseVm();
        ChildPermissionViewModel.prototype.constructor = ChildPermissionViewModel;

        return new ChildPermissionViewModel(app, dataModel, options);
    }
});