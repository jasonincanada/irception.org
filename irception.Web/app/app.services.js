(function() {
    'use strict';

    var app = angular.module('ircApp');

    app.service('Session', function ($rootScope) {
        var service = this;
        
        service.Auth = function (data) {
            service.Nick = data.Nick;
        };
        
        service.loggedIn = function (data) {
            service.UserID = data.User.UserID;
            service.Username = data.User.Username;
            service.Permissions = data.User.Permissions;
            service.Token = data.Token.Token;
            service.IP = data.Token.IP;

            $rootScope.$broadcast('session.login');
        };

        service.reset = function () {
            service.UserID = 0;
            service.Username = '';
            service.Nick = undefined;
            service.Token = '';
            service.IP = '';
        }

        service.getChannelPermissions = function (channelID, permObj) {
            var perms = [];

            if (service.Permissions)
            {
                service.Permissions.forEach(function (permission) {
                    if (permission.ChannelID == channelID) {
                        perms.push(permission.Permission);

                        // Set object members to make permission checks more convenient in the templates
                        if (permission.Permission == 'attr') {
                            permObj.attr = true;
                        }
                    }
                });                
            }

            return perms;
        }

        service.reset();

        return service;
    });

})();
