function AppDataModel() {
    var self = this,
        // Routes
        addExternalLoginUrl = "/api/Account/AddExternalLogin",
        changePasswordUrl = "/api/Account/changePassword",
        loginUrl = "/Token",
        logoutUrl = "/api/Account/Logout",
        registerUrl = "/api/Account/Register",
        registerExternalUrl = "/api/Account/RegisterExternal",
        removeLoginUrl = "/api/Account/RemoveLogin",
        setPasswordUrl = "/api/Account/setPassword",
        siteUrl = "/",
        userInfoUrl = "/api/Account/UserInfo";

    // Route operations
    function externalLoginsUrl(returnUrl, generateState) {
        return "/api/Account/ExternalLogins?returnUrl=" + (encodeURIComponent(returnUrl)) +
            "&generateState=" + (generateState ? "true" : "false");
    }

    function manageInfoUrl(returnUrl, generateState) {
        return "/api/Account/ManageInfo?returnUrl=" + (encodeURIComponent(returnUrl)) +
            "&generateState=" + (generateState ? "true" : "false");
    }

    // Other private operations
    function getSecurityHeaders() {
        var accessToken = sessionStorage["accessToken"] || localStorage["accessToken"];

        if (accessToken) {
            return { "Authorization": "Bearer " + accessToken };
        }

        return {};
    }

    // Operations
    self.getSecurityHeaders = function() {
         return getSecurityHeaders();
    }

    self.clearAccessToken = function () {
        localStorage.removeItem("accessToken");
        sessionStorage.removeItem("accessToken");
    };

    self.setAccessToken = function (accessToken, persistent) {
        if (persistent) {
            localStorage["accessToken"] = accessToken;
        } else {
            sessionStorage["accessToken"] = accessToken;
        }
    };

    self.toErrorsArray = function (data) {
        var errors = new Array(),
            items;

        if (!data || !data.message) {
            return null;
        }

        if (data.modelState) {
            for (var key in data.modelState) {
                items = data.modelState[key];

                if (items.length) {
                    for (var i = 0; i < items.length; i++) {
                        errors.push(items[i]);
                    }
                }
            }
        }

        if (errors.length === 0) {
            errors.push(data.message);
        }

        return errors;
    };

    // Data
    self.returnUrl = siteUrl;


    //app specific
    self.children = ko.observableArray();
    self.setChildren = function (c) {
        self.children(c);
    }

    self.getChildren = function() {
        return self.children();
    }

    self.fetchChildren = function() {
        //$.ajax({
        //    url: 'api/children',
        //    cache: false,
        //    headers: self.getSecurityHeaders(),
        //    contentType: 'json',
        //    success: function (data) {
        //        self.setChildren(data);
        //    }
        //});

        var childrenData = JSON.parse($.ajax({
            url: 'api/children',
            cache: false,
            headers: self.getSecurityHeaders(),
            contentType: 'json',
            async: false
        }).responseText);

        var cTemp = [];
       
        for (var i = 0; i < childrenData.length; i++) {
            cTemp.push(new ChildPermission(childrenData[i]));
        }

        self.setChildren(cTemp);

    }

    self.fetchTitles = function(callbackFunc) {
        $.ajax({
            url: 'api/title',
            cache: true,
            headers: self.getSecurityHeaders(),
            contentType: 'json',
            success: function (data) {
                callbackFunc(data);
            }
        });
    }

    // Data access operations
    self.addExternalLogin = function (data) {
        return $.ajax(addExternalLoginUrl, {
            type: "POST",
            data: data,
            headers: getSecurityHeaders()
        });
    };

    self.changePassword = function (data) {
        return $.ajax(changePasswordUrl, {
            type: "POST",
            data: data,
            headers: getSecurityHeaders()
        });
    };

    self.getExternalLogins = function (returnUrl, generateState) {
        return $.ajax(externalLoginsUrl(returnUrl, generateState), {
            cache: false,
            headers: getSecurityHeaders()
        });
    };

    self.getManageInfo = function (returnUrl, generateState) {
        return $.ajax(manageInfoUrl(returnUrl, generateState), {
            cache: false,
            headers: getSecurityHeaders()
        });
    };

    self.getUserInfo = function (accessToken) {
        var headers;

        if (typeof (accessToken) !== "undefined") {
            headers = {
                "Authorization": "Bearer " + accessToken
            };
        } else {
            headers = getSecurityHeaders();
        }

        return $.ajax(userInfoUrl, {
            cache: false,
            headers: headers
        });
    };

    self.login = function (data) {
        return $.ajax(loginUrl, {
            type: "POST",
            data: data
        });
    };

    self.logout = function () {
        return $.ajax(logoutUrl, {
            type: "POST",
            headers: getSecurityHeaders()
        });
    };

    self.register = function (data) {
        return $.ajax(registerUrl, {
            type: "POST",
            data: data
        });
    };

    self.registerExternal = function (accessToken, data) {
        return $.ajax(registerExternalUrl, {
            type: "POST",
            data: data,
            headers: {
                "Authorization": "Bearer " + accessToken
            }
        });
    };

    self.removeLogin = function (data) {
        return $.ajax(removeLoginUrl, {
            type: "POST",
            data: data,
            headers: getSecurityHeaders()
        });
    };

    self.setPassword = function (data) {
        return $.ajax(setPasswordUrl, {
            type: "POST",
            data: data,
            headers: getSecurityHeaders()
        });
    };
}
