function ForgotPassword(app, dataModel) {
    var self = this;

    // Private operations
    function reset() {
        self.errors.removeAll();
        self.oldPassword(null);
        self.newPassword(null);
        self.confirmPassword(null);
        self.changing(false);
        self.validationErrors.showAllMessages(false);
    }

    // Data
    self.name = ko.observable(name);
    self.email = ko.observable("").extend({ required: true, email: true });
    self.oldPassword = ko.observable("").extend({ required: true });
    self.newPassword = ko.observable("").extend({ required: true });
    self.confirmPassword = ko.observable("").extend({ required: true, equal: self.newPassword });
    self.code = ko.observable("").extend({ required: true });

    // Other UI state
    self.changing = ko.observable(false);
    self.errors = ko.observableArray();
    self.validationErrors = ko.validation.group([self.code, self.newPassword, self.confirmPassword]);
    self.forgotPasswordValidationErrors = ko.validation.group([self.email]);
    // Operations
    self.forgotPasswordEmail = function () {
        self.errors.removeAll();
        if (self.forgotPasswordValidationErrors().length > 0) {
            self.forgotPasswordValidationErrors.showAllMessages();
            return;
        }
        self.changing(true);
        dataModel.forgotPassword({
            email: self.email()
        }).done(function (data) {
            self.changing(false);
            reset();
            console.log("An email has been sent to " + self.email() + ". Please check it and use the code to reset your password");
        }).failJSON(function (data) {
            var errors;
            self.changing(false);
            errors = dataModel.toErrorsArray(data);

            if (errors) {
                self.errors(errors);
            } else {
                self.errors.push("An unknown error occurred.");
            }
        });
    }

    self.resetPassword = function () {
        self.errors.removeAll();
        if (self.validationErrors().length > 0) {
            self.validationErrors.showAllMessages();
            return;
        }
        self.changing(true);
        dataModel.resetPassword({
            email: self.email(),
            password: self.newPassword(),
            confirmPassword: self.confirmPassword(),
            code: self.code()
        }).done(function (data) {
            reset();
            self.changing(false);
            console.log("Your password has been reset. Please try to login");
        }).failJSON(function (data) {
            var errors;
            self.changing(false);
            errors = dataModel.toErrorsArray(data);

            if (errors) {
                self.errors(errors);
            } else {
                self.errors.push("An unknown error occurred.");
            }
        });
    }


}


app.addViewModel({
    name: "ForgotPassword",
    bindingMemberName: "forgotPassword",
    factory: ForgotPassword
});