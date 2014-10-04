
var app = angular.module('signalRApp', []);

app.controller('MessagesController', function($scope) {
    $scope.messages = [{ creationDate: "11:30", body: "Hello! this is a message"}, 
    { creationDate: "11:33", body: "Hello! this is another message"} ]
}); 

app.service('breakingNewsService', function ($, $rootScope) {
    var proxy = null;
  
    var initialize = function () {
        //Getting the connection object
        connection = $.hubConnection();
  
        //Creating proxy
        this.proxy = connection.createHubProxy('helloWorldHub');
  
        //Starting connection
        connection.start();
  
        //Publishing an event when server pushes a message
        this.proxy.on('breakingNews', function (message) {
            $rootScope.$emit("breakingNews",message);
        });
    };
  
    return {
        initialize: initialize
    }; 
});