function DeveloperViewModel(app, dataModel) {
    var self = this;

    var baseUrl = 'api/developer/';

    self.forbidden = function () {
        $.ajax({
            url: baseUrl + 'forbidden',
            cache: false,
            headers: dataModel.getSecurityHeaders(),
            data: {},
            contentType: 'json',
            success: function (data) {
                console.log('Developer Success forbidden');
            }
        });
    }

    self.serverError = function () {
        $.ajax({
            url: baseUrl + 'serverError',
            cache: false,
            headers: dataModel.getSecurityHeaders(),
            data: {},
            contentType: 'json',
            success: function (data) {
                console.log('Developer Success serverError');
            }
        });
    }

    self.notFound = function () {
        $.ajax({
            url: baseUrl + 'notfound',
            cache: false,
            headers: dataModel.getSecurityHeaders(),
            data: {},
            contentType: 'json',
            success: function (data) {
                console.log('Developer Success notfound');
            }
        });
    }

    self.unauthorized = function () {
        $.ajax({
            url: baseUrl + 'badrequest',
            headers: dataModel.getSecurityHeaders(),
            success: function (data) {
                console.log('Developer Success badrequest');
            }
        });
    }


    // HomeViewModel currently does not require data binding, so there are no visible members.
}

app.addViewModel({
    name: "Developer",
    bindingMemberName: "developer",
    factory: DeveloperViewModel
});
