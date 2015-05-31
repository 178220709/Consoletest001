
define(function (require, exports, module) {


    var model = avalon.define("ctrlSpiderIndex", function (vm) {
        vm.result = "";
        vm.SpiderClick = function () {

        };
        vm.CheckClick = function () {

        };
    });

    function init() {
        $(function () {
            avalon.scan(document.getElementById("divContainContent"), model);
        });
    }



    return {
        model: model,
        init: init
    }
});



