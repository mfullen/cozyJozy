function AppViewModel(dataModel) {
    // Private state
    var self = this;

    // Private operations
    function cleanUpLocation() {
        window.location.hash = "";

        if (typeof (history.pushState) !== "undefined") {
            history.pushState("", document.title, location.pathname);
        }
    }

    function getFragment() {
        if (window.location.hash.indexOf("#") === 0) {
            return parseQueryString(window.location.hash.substr(1));
        } else {
            return {};
        }
    }

    function parseQueryString(queryString) {
        var data = {},
            pairs, pair, separatorIndex, escapedKey, escapedValue, key, value;

        if (queryString === null) {
            return data;
        }

        pairs = queryString.split("&");

        for (var i = 0; i < pairs.length; i++) {
            pair = pairs[i];
            separatorIndex = pair.indexOf("=");

            if (separatorIndex === -1) {
                escapedKey = pair;
                escapedValue = null;
            } else {
                escapedKey = pair.substr(0, separatorIndex);
                escapedValue = pair.substr(separatorIndex + 1);
            }

            key = decodeURIComponent(escapedKey);
            value = decodeURIComponent(escapedValue);

            data[key] = value;
        }

        return data;
    }

    function verifyStateMatch(fragment) {
        var state;

        if (typeof (fragment.access_token) !== "undefined") {
            state = sessionStorage["state"];
            sessionStorage.removeItem("state");

            if (state === null || fragment.state !== state) {
                fragment.error = "invalid_state";
            }
        }
    }

    // Data
    self.Views = {
        Loading: {} // Other views are added dynamically by app.addViewModel(...).
    };

    //cozyJozy specific
    self.selectedChild = ko.observable();

    self.availableChildren = ko.computed(function () {
        return dataModel.getChildren();
    });

    self.availableTitles = ko.observableArray();

    self.setAvailableTitles = function(t) {
        self.availableTitles(t);
    }

    self.blueTheme = '#4285F4'; //blue theme
    self.pinkTheme =  "#e91e63"; //pink;

    self.themeBackgroundColorClassName = ko.computed(function () {
        if (self.selectedChild()) {
            var sc = self.selectedChild();
            if (sc.child().male()) {
                return self.blueTheme;
            } else {
                return self.pinkTheme;
            }
        }
        return self.blueTheme;
    }, self);

    self.childDisplayName = function (f) {
        if (!f) {
            return '';
        }
        return f.child().firstName() + ' ' + f.child().middleName() + ' ' + f.child().lastName();
    }


    self.activeMenuClass = function(bit) {
        return ko.computed({
            read: function() {
                if (bit === self.view().name) {
                    return "active";
                }
                return "";
            },
            write: function(cname) {

                return "";
            }
        }, self);
    };


    // UI state
    self.errors = ko.observableArray();
    self.user = ko.observable(null);
    self.view = ko.observable(self.Views.Loading);

    self.loading = ko.computed(function () {
        return self.view() === self.Views.Loading;
    });

    self.loggedIn = ko.computed(function () {
        return self.user() !== null;
    });

    // UI operations
    self.archiveSessionStorageToLocalStorage = function () {
        var backup = {};

        for (var i = 0; i < sessionStorage.length; i++) {
            backup[sessionStorage.key(i)] = sessionStorage[sessionStorage.key(i)];
        }

        localStorage["sessionStorageBackup"] = JSON.stringify(backup);
        sessionStorage.clear();
    };

    self.restoreSessionStorageFromLocalStorage = function () {
        var backupText = localStorage["sessionStorageBackup"],
            backup;

        if (backupText) {
            backup = JSON.parse(backupText);

            for (var key in backup) {
                sessionStorage[key] = backup[key];
            }

            localStorage.removeItem("sessionStorageBackup");
        }
    };

    self.navigateToLoggedIn = function (userName, accessToken, persistent) {
        self.errors.removeAll();

        if (accessToken) {
            dataModel.setAccessToken(accessToken, persistent);
        }

        dataModel.fetchChildren(function() {
            self.user(new UserInfoViewModel(self, userName, dataModel));

            dataModel.fetchTitles(function (d) { return self.availableTitles(d); });

            if (dataModel.children().length === 0) {
                self.navigateToChildManagement();
            } else {
                self.navigateToHome();
            }
        });
    };

    self.navigateToLoggedOff = function () {
        self.errors.removeAll();
        dataModel.clearAccessToken();
        self.navigateToLogin();
    };

    // Other navigateToX functions are added dynamically by app.addViewModel(...).

    // Other operations
    self.addViewModel = function (options) {
        var viewItem = {},
            navigator;

        // Add view to AppViewModel.Views enum (for example, app.Views.Home).
        self.Views[options.name] = viewItem;
        viewItem.name = options.name;

        // Add binding member to AppViewModel (for example, app.home);
        self[options.bindingMemberName] = ko.computed(function () {
            if (self.view() !== viewItem) {
                return null;
            }

            return new options.factory(self, dataModel);
        });

        if (typeof (options.navigatorFactory) !== "undefined") {
            navigator = options.navigatorFactory(self, dataModel);
        } else {
            navigator = function () {
                self.errors.removeAll();
                self.view(viewItem);
            };
        }

        // Add navigation member to AppViewModel (for example, app.NavigateToHome());
        self["navigateTo" + options.name] = navigator;
    };

    self.initialize = function () {
        var fragment = getFragment(),
            externalAccessToken, externalError, loginUrl;

        self.restoreSessionStorageFromLocalStorage();
        verifyStateMatch(fragment);

        if (sessionStorage["associatingExternalLogin"]) {
            sessionStorage.removeItem("associatingExternalLogin");

            if (typeof (fragment.error) !== "undefined") {
                externalAccessToken = null;
                externalError = fragment.error;
                cleanUpLocation();
            } else if (typeof (fragment.access_token) !== "undefined") {
                externalAccessToken = fragment.access_token;
                externalError = null;
                cleanUpLocation();
            } else {
                externalAccessToken = null;
                externalError = null;
                cleanUpLocation();
            }

            dataModel.getUserInfo()
                .done(function (data) {
                    if (data.userName) {
                        self.navigateToLoggedIn(data.userName);
                        self.navigateToManage(externalAccessToken, externalError);
                    } else {
                        self.navigateToLogin();
                    }
                })
                .fail(function () {
                    self.navigateToLogin();
                });
        } else if (typeof (fragment.error) !== "undefined") {
            cleanUpLocation();
            self.navigateToLogin();
            self.errors.push("External login failed.");
        } else if (typeof (fragment.access_token) !== "undefined") {
            cleanUpLocation();
            dataModel.getUserInfo(fragment.access_token)
                .done(function (data) {
                    if (typeof (data.userName) !== "undefined" && typeof (data.hasRegistered) !== "undefined"
                        && typeof (data.loginProvider) !== "undefined") {
                        if (data.hasRegistered) {
                            self.navigateToLoggedIn(data.userName, fragment.access_token, false);
                        }
                        else if (typeof (sessionStorage["loginUrl"]) !== "undefined") {
                            loginUrl = sessionStorage["loginUrl"];
                            sessionStorage.removeItem("loginUrl");
                            self.navigateToRegisterExternal(data.userName, data.loginProvider, fragment.access_token,
                                loginUrl, fragment.state);
                        }
                        else {
                            self.navigateToLogin();
                        }
                    } else {
                        self.navigateToLogin();
                    }
                })
                .fail(function () {
                    self.navigateToLogin();
                });
        } else {
            dataModel.getUserInfo()
                .done(function (data) {
                    if (data.userName) {
                        self.navigateToLoggedIn(data.userName);
                    } else {
                        self.navigateToLogin();
                    }
                })
                .fail(function () {
                    self.navigateToLogin();
                });
        }
    }
}

var app = new AppViewModel(new AppDataModel());
