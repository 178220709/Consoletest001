
define(function (require, exports, module) {
    var avalon = require("avalon.js");
    var util = require("public/util.js");
    
   
    var model = avalon.define("ctrlSystemHttpDebugger", function (vm) {
        vm.paras = [{ key: "pageIndex", value: 1 }, { key: "pageSize", value: 10 }];
        vm.url = "http://";
        vm.target = "";
        vm.result = "";

        vm.addPara = function () {
            model.paras.push({ key: "", value: "" });
        };
        vm.getTarget = function() {
            vm.target = getTarget();
        };
        vm.postClick = function() {
            $.post(model.url, { queryData: util.getQueryString("queryData"), direction: util.getQueryString("direction") }, function(data) {
                $scope.pros = data.rows;
            });
        };

    });
    model.paras.$watch("length", function () {
        model.target = getTarget();
    });
    model.$watch("url", function () {
        model.target = getTarget();
    });

    function getTarget() {
        var opts = {};
        _.each(model.paras, function (para) {
            if (para.key && para.value) {
                opts[para.key] = para.value;
            }
        });
       return model.url + util.optsToStr(opts);
    }



    exports.VM = model;
    exports.initPage  = function(parameters) {
        $(function () {
            avalon.scan(document.getElementById("divContainContent"), model);
        });
    }



});



