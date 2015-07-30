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
            
})();