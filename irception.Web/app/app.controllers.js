(function () {
    'use strict';

    var app = angular.module('ircApp');

    app.controller('HomeController', function ($state, Session) {
        var vm = this;

        vm.Session = Session;

        vm.goLogin = function () {
            $state.go('login');
        };

        vm.goUser = function () {
            $state.go('me')
        }

        return vm;
    });

    app.controller('ArtController', function ($state, Session) {
        var vm = this;

        vm.Session = Session;
        vm.art = "hlaoe bulaocheu aloeuh";

        return vm;
    });

    app.controller('SessionController', function ($http, $state, $stateParams, Session) {
        var vm = this;

        vm.Message = '';

        $http
          .get('/api/connect.ashx?SUID=' + $stateParams.SUID)
	      .success(function (data) {

	          Session.Auth(data);

	          $state.go('channel', {
	              networkSlug: data.NetworkSlug,
	              channelSlug: data.ChannelSlug
	          });
	      });

        return vm;
    });

    app.controller('ConfigController', function ($state, DataService) {
        var vm = this;

        vm.Networks = [];

        DataService.getConfig(function(data) {
            vm.Networks = data.Networks;
        });
        
        vm.urlLink = function (network, channel) {
            $state.go('channel', {
                networkSlug: network.Slug,
                channelSlug: channel.Slug
            });
        };

        return vm;
    });

    app.controller('MeController', function ($state, Session) {
        var vm = this;

        vm.Session = Session;

        vm.logout = function () {
            Session.reset();
            $state.go('login');
        }

        vm.login = function () {
            $state.go('login');
        };

        return vm;
    });

    app.controller('LoginController', function ($state, $timeout, Session, DataService) {
        var vm = this;

        vm.username = '';
        vm.password = '';

        vm.login = function () {
            if (vm.username.length == 0) {
                $timeout(function () {
                    $('#inputUsername').focus();
                });
                return;
            }

            if (vm.password.length == 0) {
                $timeout(function () {
                    $('#inputPassword').focus();
                });
                return;
            }

            DataService.login(vm.username, vm.password, function (data) {
                if (data.success) {
                    Session.loggedIn(data);

                    $state.go('channel', {
                        channelSlug: 'dropnet',
                        networkSlug: 'dr'
                    });

                    vm.error = undefined;
                } else {
                    vm.error = data.UserMessage;
                }
            });
        };

        $timeout(function () {
            $('#inputUsername').focus();
        });

        return vm;
    });
    
    app.controller('ChannelController', function ($scope, $stateParams, $interval, DataService, UI, Session) {
        var vm = this;

        vm.Channel = { ChannelName: '#' };
        vm.URLs = [];

        var lastURLUpdateHistoryID = 0;

        vm.refreshElapsed = function (secondsPassed) {
            vm.URLs.forEach(function (url) {

                // TODO: improve this
                if (!url.SecondsElapsed)
                    url.SecondsElapsed = 0;

                url.SecondsElapsed += secondsPassed;
                url.ElapsedString = UI.elapsedString(url.SecondsElapsed);
            });
        };

        vm.updateURLs = function (urls) {
            urls.forEach(function (url) {
                var idx = -1;

                for (var i = 0; i < vm.URLs.length; i++) {
                    if (vm.URLs[i].URLID == url.URLID) {
                        idx = i;
                        break;
                    }
                }

                if (idx == -1) {
                    vm.URLs.push(url);
                } else {
                    vm.URLs[idx] = url;
                }
            });
        };

        DataService.getChannel($stateParams.networkSlug, $stateParams.channelSlug, function (data) {
            vm.Channel = data.Channel;
            vm.NetworkName = data.NetworkSlug;
            vm.updateURLs(data.URLs);
            vm.refreshElapsed(0);

            UI.tabTitle(data.NetworkSlug + '/' + data.ChannelSlug + ' - irception.org')

            vm.lastURLUpdateHistoryID = data.luuhid;

            onLogin();
        });

        var interval = $interval(function () {

            DataService.getChannelFromLast($stateParams.networkSlug, $stateParams.channelSlug, vm.lastURLUpdateHistoryID, function (data) {
                vm.updateURLs(data.URLs);
                vm.refreshElapsed(0);
                vm.lastURLUpdateHistoryID = data.luuhid;
            });

            vm.refreshElapsed(5);
        }, 5000);

        $scope.$on('$destroy', function () {
            if (interval) {
                $interval.cancel(interval);
            }
        });
                
        function onLogin() {
            vm.Permission = {};
            vm.Permissions = Session.getChannelPermissions(vm.Channel.ChannelID, vm.Permission);
        };

        $scope.$on('session.login', onLogin);

        vm.setNSFW = function (urlID) {
            DataService.setAttr(urlID, 'nsfw', function () { });            
        };

        vm.unsetNSFW = function (urlID) {
            DataService.unsetAttr(urlID, 'nsfw', function () { });
        };

        return vm;
    });
    
})();
