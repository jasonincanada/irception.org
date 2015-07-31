(function () {
    'use strict';

    var app = angular.module('ircApp');

    app.service('DataService', function ($http) {
        var service = this;
        
        service.getConfig = function (successFunc) {
            $http
	            .get('/api/config.ashx')
	            .success(successFunc);
        };

        service.login = function (params, successFunc) {
            $http
                .post('/api/login.ashx', params)
                .success(successFunc);            
        };

        service.getChannel = function (networkSlug, channelSlug, successFunc) {
            $http
                .get('/api/chan.ashx?network=' + networkSlug + '&channel=' + channelSlug)
                .success(successFunc);
        };
        
        service.getChannelFromLast = function (networkSlug, channelSlug, lastURLUpdateID, successFunc) {
            $http
                .get('/api/chan.ashx?network=' + networkSlug + '&channel=' + channelSlug + '&luuhid=' + lastURLUpdateID)
                .success(successFunc);
        };

        service.getChannelModData = function (channelID, successFunc) {
            $http
                .get('/api/mod.ashx?id=' + channelID)
                .success(successFunc);
        };

        service.setAttr = function (urlID, attr, successFunc) {
            $http
                .post('/api/attr.ashx', {
                    URLID: urlID,
                    SetAttr: attr
                })
                .success(successFunc);
        };

        service.unsetAttr = function (urlID, attr, successFunc) {
            $http
                .post('/api/attr.ashx', {
                    URLID: urlID,
                    UnsetAttr: attr
                })
                .success(successFunc);
        };

        service.getInviteeNick = function (SUID, successFunc) {
            $http
                .post('/api/inviteeNick.ashx', {
                    SUID: SUID
                })
                .success(function (data) {
                    successFunc(data.nick);
                });
        };

        service.register = function (params, successFunc) {
            $http
                .post('/api/register.ashx', params)
                .success(successFunc);
        };

        service.getUser = function (username, successFunc) {
            $http
                .get('/api/user.ashx?username=' + username)
                .success(successFunc);
        };

        return service;
    });

})();
