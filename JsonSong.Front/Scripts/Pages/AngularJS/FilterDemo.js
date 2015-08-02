

var phonecatFilters = angular.module('phonecatFilters', []);
phonecatFilters.filter('checkmark', function () {
    return function (input) {
        return input ? '\u2713' : '\u2718';
    };
});
phonecatFilters.filter('toUrl', function () {
    return function (url) {
        return "<a src='"+url+"'></a>";
    };
});
angular.module('phonecatFilters2', []).filter('to_trusted', ['$sce', function ($sce) {
    return function (text) {
        return $sce.trustAsHtml(text);
    };
}]);

angular.module('phonecatFilters', []).filter('to_trusted', function(parameters) {
    return function (text) {
        return text + "to_trusted";
    };
});

var phonecatApp = angular.module('phonecatApp', ['phonecatFilters']);

phonecatApp.controller('PhoneListCtrl', ['$scope', '$http', function ($scope, $http) {
    var getList = function (pageIndex) {
        $http.post('/spider/getList', { PageSize: 10, PageIndex: $scope.pageIndex, typeId: 1 }).success(function (data) {
            $scope.phones = data.Rows;
        });
    };

    $scope.orderProp = 'Url';
    $scope.pageIndex = 1;
    $scope.pageChange = function (parameters) {
        console.log("current pageIndex is " + $scope.pageIndex);
    };
    $scope.$watch('pageIndex', function () {
        getList($scope.pageIndex);
    });

    getList($scope.pageIndex);
}]);



