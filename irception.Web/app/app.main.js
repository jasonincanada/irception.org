(function() {
	'use strict';

	var app = angular.module('ircApp', [
        'common.services',
        'ui.router'
	]);
   	
	app.config(function ($stateProvider, $urlRouterProvider) {
	   
	    $urlRouterProvider.otherwise('/');
	    
	    $stateProvider
            .state('home', {
                url: '/',
                templateUrl: 'templates/welcome.html'                
            })
            .state('config', {
                url: '/config',
                templateUrl: 'templates/config.html',
                controller: 'ConfigController as vm'
            })
            .state('login', {
                url: '/login',
                templateUrl: 'templates/login.html',
                controller: 'LoginController as vm'
            })
            .state('me', {
                url: '/me',
                templateUrl: 'templates/me.html',
                controller: 'MeController as vm'
            })
            .state('auth', {
                url: '/auth/{SUID:[a-fA-F0-9]{32}}',
                templateUrl: 'templates/login.html',
                controller: 'LoginController as vm'
            })
            .state('invite', {
                url: '/invite/{SUID:[a-fA-F0-9]{32}}',
                templateUrl: 'templates/invite.html',
                controller: 'InviteController as vm'
            })            
            .state('mod', {                
                url: '/mod/{networkSlug:[a-z]{1,2}}/{channelSlug:[a-zA-Z0-9]{1,20}}',
                templateUrl: 'templates/mod.html',
                controller: 'ModController as vm'
            })
            .state('user', {
                url: '/u/{Username:[a-zA-Z0-9_-]{1,20}}',
                templateUrl: 'templates/user.html',
                controller: 'UserController as vm'
            })
	        .state('channel', {
	            url: '/{networkSlug:[a-z]{1,2}}/{channelSlug:[a-zA-Z0-9]{1,20}}',
	            templateUrl: 'templates/channel.html',
	            controller: 'ChannelController as vm'
	        });
	});

	app.run(function () {
	    
        // Redirect to HTTPS
	    if (window.location.href.indexOf('http://ircbot') >= 0) {
	        window.location.href = window.location.href.replace('http://ircbot', 'https://ircbot');
	    }
	});
    
})();