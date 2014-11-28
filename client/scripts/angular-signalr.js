angular.module('signalRApp', [])
.value('$', $)
.service('demoHubService', function ($, $rootScope) {
    var proxy = null;
    var connection;

    this.initialize = function () {
        connection = $.hubConnection('http://localhost:8088');
        connection.logging = true;

        this.proxy = connection.createHubProxy('demoHub');
        
        // messages from the signalR hub
        this.proxy.on('sendDemoHubMessage', function (message) {
            $rootScope.$broadcast("MessageEvent", message);
        });

        this.proxy.on('sendMessageFromClient', function (message) {
            $rootScope.$broadcast("MessageFromOtherClientEvent", message);
        });

// { transport: ['webSockets', 'serverSentEvents', 'longPolling', 'foreverFrame'] }
        connection.start(
            // { transport: ['serverSentEvents', 'longPolling'] }
            ).done(function () {
            $rootScope.$broadcast("SignalREvent", "started");
        });

        connection.reconnected(function() {
           $rootScope.$broadcast("SignalREvent", "reconnected"); 
        });

        connection.disconnected(function() {
           $rootScope.$broadcast("SignalREvent", "disconnected"); 
        });

        connection.reconnecting(function() {
           $rootScope.$broadcast("SignalREvent", "reconnecting"); 
        });

        connection.error(function(error) {
           $rootScope.$broadcast("SignalREvent", "error: " + error); 
        });
    };

    this.sendMessage = function(message) {
        this.proxy.invoke('sendMessage', message);
    };

    this.stopConnection = function(message) {
        connection.stop();
        $rootScope.$broadcast("SignalREvent", "Connection stopped"); 
    };

    this.connectionId = function() {
        return this.proxy.connection.id;
    };
})
.controller('MessagesController', function($scope, $sce, demoHubService) {
     var body = "";
     $scope.status = "no connection";
     $scope.messageFromOtherClient = "Still no message from other client :-(";

     $scope.$on("MessageEvent", function (event, message) {
        body = body + message.Body;
        $scope.message = $sce.trustAsHtml(body);

        $scope.$apply();
    });

     $scope.$on("SignalREvent", function (event, message) {
        $scope.status = message;
        $scope.connectionId = demoHubService.connectionId();

        $scope.$apply();
    });

      $scope.$on("MessageFromOtherClientEvent", function (event, message) {
        $scope.messageFromOtherClient = message;

        $scope.$apply();
    });

     $scope.makeConnection = function() {
        demoHubService.initialize();  
     }

     $scope.stopConnection = function() {
        demoHubService.stopConnection();  
     }

     $scope.sendMessage = function(message) {
        demoHubService.sendMessage(message);  
     };
}); 
