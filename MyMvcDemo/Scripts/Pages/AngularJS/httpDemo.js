'use strict';

/* Controllers */

var phonecatApp = angular.module('phonecatApp', []);

phonecatApp.controller('PhoneListCtrl', ['$scope', '$http', function ($scope, $http) {
    var getList = function (pageIndex) {
        $http.post('/spider/getList', { PageSize: 10, PageIndex: $scope.pageIndex, typeId: 1 }).success(function (data) {
            $scope.phones = data.Rows;
        });
    };
   
    $scope.orderProp = 'Url';
    $scope.pageIndex = 1;
    $scope.pageChange = function(parameters) {
        console.log("current pageIndex is " + $scope.pageIndex  );
    };
    $scope.$watch('pageIndex', function () {
        getList($scope.pageIndex);
    });
   

    getList($scope.pageIndex);
}]);
