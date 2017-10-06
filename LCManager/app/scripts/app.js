'use strict';
angular.module('LCManager', ['ui.router'])
  
.config(function($stateProvider, $urlRouterProvider) {
  $stateProvider
              // route for the home page
      .state('app', {
          url:'/',
          views: {
              'header': {
                  templateUrl : ''
              },
              'content': {
                templateUrl : 'views/authorize.html',
                  controller  : 'AuthorizeController'
              },
              'footer': {
                  templateUrl : ''
              }
          }
      });
      
      $urlRouterProvider.otherwise('/');
})
;