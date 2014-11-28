angular.module('signalRApp', [])
.value('$', $)
.service('breakingNewsService', function ($, $rootScope) {
    var proxy = null;
  
    this.initialize = function () {
        console.log('create connection');
        var connection = $.hubConnection('http://localhost:8088');
        connection.logging = true;

        console.log('create hub proxy');
        this.proxy = connection.createHubProxy('breakingNewsHub');
  
        this.proxy.on('sendBreakingNews', function (message) {
            $rootScope.$broadcast("breakingNewsEvent", message);
        });
  
        console.log('start connection');

        setTimeout(function () {
            console.log('start');
            
            connection.start().done(function () {
                console.log('started');
            });
        }, 1000);
    };
})
.controller('MessagesController', function($scope, $sce, breakingNewsService) {
    breakingNewsService.initialize();

     var body = "";
    
     $scope.$on("breakingNewsEvent", function (event, message) {
        body = body + message.Body;
        $scope.message = $sce.trustAsHtml(body);
        $scope.$apply();
    });
}); 
