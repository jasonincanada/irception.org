(function () {
    'use strict';

    var app = angular.module('ircApp');

    app.directive('irceptionUser', function() {
        return {
            templateUrl: 'templates/directives/irception-user.html',
            restrict: 'E',
            scope: {
                user: '='
            }
        }
    });

    app.directive('signature', function () {
        return {
            templateUrl: 'templates/directives/signature.html',
            restrict: 'E',
            scope: {
                signature: '='
            }
        }
    });
            
})();