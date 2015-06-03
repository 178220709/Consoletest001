
define(["util"], function (util) {

    var model = avalon.define("ctrlSpiderIndex", function (vm) {
        vm.Result = "";
        vm.TypeId = 0;
        vm.SpiderClick = function () {
            $.post(util.getAbsUrl("Spider/SpiderRecommand"), { typeId: vm.TypeId }, function (res) {
                
            });
        };

        vm.CheckClick = function () {
            $.post(util.getAbsUrl("Spider/GetSpiderInfo"), { typeId: 1 }, function (res) {
                vm.Result = res.result;
            });
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



