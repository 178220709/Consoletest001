
define(["util"], function (util) {

    var model = avalon.define("ctrlSpiderIndex", function (vm) {
        vm.Result = "";
        vm.TypeId = 0;
        vm.SpiderClick = function () {
            $.post(util.getAbsUrl("Spider/SpiderRecommand"), { typeId: vm.TypeId }, function (res) {
                
            });
        };

        vm.CheckClick = function () {
            $.post(util.getAbsUrl("Spider/GetSpiderInfo"), { }, function (res) {
                var list = res.result;
                vm.Result = list.join("\n");
            });
        };
        vm.SyncYouminClick = function () {
            $.post(util.getAbsUrl("Spider/SyncYoumin"), {}, function (res) {
                var list = res.result;
                vm.Result = list.join("\n");
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



