angular.module('signalRApp', [])
.value('$', $)
.service('breakingNewsService', function ($, $rootScope) {
    var proxy = null;
  
    this.initialize = function () {
        console.log('create connection');

        //Getting the connection object
        var connection = $.hubConnection('http://localhost:8088');
        connection.logging = true;

        console.log('create hub proxy');
        //Creating proxy
        this.proxy = connection.createHubProxy('breakingNewsHub');
  
        //Publishing an event when server pushes a message
        this.proxy.on('breakingNews', function (message) {
            $rootScope.$broadcast("breakingNews", message);
        });
  
        console.log('start connection');

        //Starting connection
        setTimeout(function () {
            console.log('start');
            
            connection.start().done(function () {
                //Do interesting stuff
                console.log('started');
            });
        }, 1000);
    };
})
.controller('MessagesController', function($scope, breakingNewsService) {
    breakingNewsService.initialize();

    $scope.messages = [];

     $scope.$on("breakingNews", function (event, message) {
        $scope.messages.push(message);

        $scope.$apply();
    });
}); 
