
define(["util"], function (util) {
    var model = avalon.define("ctrlSystemHttpDebugger", function (vm) {
        vm.paras = [{ key: "pageIndex", value: 1 }, { key: "pageSize", value: 10 }, { key: "cnName", value: "spider" }];
        vm.url = "http://localhost:18080/api/spider/getSpiderList";
        vm.target = "";
        vm.result = "";
        vm.postURL = "";

        vm.addPara = function () {
            model.paras.push({ key: "", value: "" });
        };
        vm.getTarget = function () {
            vm.target = getTarget();
        };
        vm.postClick = function () {
            var opts = getParasObj();
            $.post(model.postURL, { url: model.url, paras: JSON.stringify(opts) }, function (context) {
                //var data = JSON.parse(context);
                //vm.result = JSON.stringify(data, 4, "\t");
                vm.result = context;
            });
        };

    });
    model.paras.$watch("length", function () {
        model.target = getTarget();
    });
    model.$watch("url", function () {
        model.target = getTarget();
    });

    function getParasObj() {
        var opts = {};
        _.each(model.paras, function (para) {
            if (para.key && para.value) {
                opts[para.key] = para.value;
            }
        });
        return opts;
    }

    function getTarget() {
        var opts = getParasObj();
        return model.url + util.optsToStr(opts);
    }


    return {
        model:model,
        init:function (parameters) {
            $(function () {
                avalon.scan(document.getElementById("divContainContent"), model);
            });
        }
    };

});



