function RegisterViewModel(app, dataModel) {
    var self = this;

    // Data
    self.userName = ko.observable("").extend({ required: true });
    self.password = ko.observable("").extend({ required: true });
    self.confirmPassword = ko.observable("").extend({ required: true, equal: self.password });
    self.firstName = ko.observable("").extend({ required: false });
    self.lastName = ko.observable("").extend({ required: false });
    self.email = ko.observable("").extend({ required: true, email: true });
    self.acceptAgreement = ko.observable(false).extend({
        equal: {
            message: "You MUST",
            params: true
        }
    });

    // Other UI state
    self.registering = ko.observable(false);
    self.errors = ko.observableArray();
    self.validationErrors = ko.validation.group([self.userName, self.password, self.confirmPassword, self.email, self.acceptAgreement]);

    // Operations
    self.register = function () {
        self.errors.removeAll();
        if (self.validationErrors().length > 0) {
            self.validationErrors.showAllMessages();
            return;
        }
        self.registering(true);

        dataModel.register({
            userName: self.userName(),
            password: self.password(),
            confirmPassword: self.confirmPassword(),
            email: self.email(),
            firstName: self.firstName(),
            lastName: self.lastName()
    }).done(function (data) {
            dataModel.login({
                grant_type: "password",
                username: self.userName(),
                password: self.password()
            }).done(function (data) {
                self.registering(false);

                if (data.userName && data.access_token) {
                    app.navigateToLoggedIn(data.userName, data.access_token, false /* persistent */);
                } else {
                    self.errors.push("An unknown error occurred.");
                }
            }).failJSON(function (data) {
                self.registering(false);

                if (data && data.error_description) {
                    self.errors.push(data.error_description);
                } else {
                    self.errors.push("An unknown error occurred.");
                }
            });
        }).failJSON(function (data) {
            var errors;

            self.registering(false);
            errors = dataModel.toErrorsArray(data);

            if (errors) {
                self.errors(errors);
            } else {
                self.errors.push("An unknown error occurred.");
            }
        });
    };

    self.login = function () {
        app.navigateToLogin();
    };
}

app.addViewModel({
    name: "Register",
    bindingMemberName: "register",
    factory: RegisterViewModel
});
