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

        return service;
    });

})();
