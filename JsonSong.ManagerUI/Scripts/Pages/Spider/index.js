
define(["util"], function (util) {

    var model = avalon.define("ctrlSpiderIndex", function (vm) {
        vm.result = "";
        vm.SpiderClick = function () {
            $.post(util.getAbsUrl("Spider/SpiderRecommand"), { typeId :1}, function(res) {
                
            });
        };
        vm.CheckClick = function () {
            $.post(util.getAbsUrl("Spider/GetSpiderInfo"), { typeId: 1 }, function (res) {
                vm.result = res.result;
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



