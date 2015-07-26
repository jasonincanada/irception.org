(function () {
    "use strict";

    angular.module('common.services', [])

    angular
		.module('common.services')
		.service('UI', UI);

    function UI($window) {

        // From: http://jonaquino.blogspot.ca/2006/12/twitter-increasing-number-of-twitters.html
        this.elapsedString = function (seconds) {

            //var seconds = Math.round(ticks / 10000 / 1000);

            var s = function (n) { return n == 1 ? '' : 's' };

            if (seconds <= 0) {
                return 'just now';
            }
            if (seconds < 100) {
                var n = seconds;
                return n + ' sec';
            }
            if (seconds < 60 * 100) {
                var n = Math.floor(seconds / 60);
                return n + ' min';
            }
            if (seconds < 60 * 60 * 100) {
                var n = Math.floor(seconds / 60 / 60);

                var divisor_for_minutes = seconds % (60 * 60);
                var minutes = Math.floor(divisor_for_minutes / 60);
                return n + ' hr';
            }

            var n = Math.floor(seconds / 60 / 60 / 24);
            return n + ' day' + s(n);
        };

        this.tabTitle = function (title) {
            $window.document.title = title;
        };

        return this;
    };

}());