
define(function () {

    function init (vm) {
        this.vm = vm;
    }

    init.prototype.changePage = function (num) {
        var sum = vm.pageIndex + num;
        if (sum < 1 || sum >= vm.pageCount) {
            return;
        }
        vm.pageIndex = sum;
    };

   return init;
});