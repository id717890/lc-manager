'use strict';
/*Inject the service into this controller, because this controller needs that service in order to function menuFactory*/ 
angular.module('LCManager')
.controller('AuthorizeController', ['$scope', function($scope){
  $scope.submitAuth = function(){
    console.log($scope.authorize)
  };
}])
;