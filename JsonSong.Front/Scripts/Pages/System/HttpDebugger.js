
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
            model.target = getTarget();
        };
        vm.postClick = function () {
            var opts = getParasObj();
            $.post(model.postURL, { url: model.url, paras: JSON.stringify(opts) }, function (context) {
                //var data = JSON.parse(context);
                //vm.result = JSON.stringify(data, 4, "\t");
                model.result = context;
            });
        };
    });
    model.$watch("paras.length", function () {
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

    //ping
    var model2 = avalon.define("ctrlSystemPingDebugger", function (vm) {
        vm.host = "";
        vm.result = "";
        vm.PingClick = function () {
            $.post(util.getAbsUrl("System/GetPingResult"), { host: model2.host }, function (context) {
                model2.result = context;
            });
        };
    });

    $("input[data-condition]").keydown(function (event) {
        if (event.keyCode == 13)
            //  $("a[data-search]").trigger("click");
            $("a[data-search]")[0].click();
    });

    return {
        model:model,
        model2: model2,
        init:function () {
            $(function () {
                avalon.scan();
            });
        }
    };

});



