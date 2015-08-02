
define(["util"], function (util) {

    var model = avalon.define("ctrlTempAppParticipant", function (vm) {
        vm.BaseList = [];
        vm.AddName = "";
        vm.AddClick = function () {
            if (!vm.AddName) {
                getList();
                return;
            }
            $.post(util.getAbsUrl("TempApp/Add"), {name:vm.AddName}, function (res) {
                getList();
            });
        };
    });

    function init() {
        getList();
        $(function () {
            avalon.scan(document.getElementById("divContainContent"), model);
        });
    }

    function getList() {
        $.post(util.getAbsUrl("TempApp/GetList"), {}).done(function (data) {
            model.BaseList = data;
        });
    }

    return {
        model: model,
        init: init
    };
});



