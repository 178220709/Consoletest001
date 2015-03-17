
define(function (require, exports, module) {
    var avalon = require("avalon.js");

    var getList = function(pageIndex,callback) {
        $.post("spider/getHahaList", { PageSize: 4, PageIndex: pageIndex }, function (data) {
            if (_.isFunction(callback)) {
                callback(data);
            }
        });
        
    };

    var model = avalon.define("ctrlSpiderHahaList", function (vm) {
        vm.list = [];
        vm.pageIndex = 1;
        vm.current = {};
        vm.getWeightClass = function(weight) {
            if (weight<0) {
                return "label-danger";
              
            }
            if (weight == 0) {
                return "label-warning";
            }
            if (weight > 0 && weight <100 ) {
                return "label-primary";
            }
            return "label-success";
        };

        vm.refreshClick = function() {
            getList(vm.pageIndex, function (data) {
                vm.list = data.Rows;
            });
        }
        vm.trClick = function (row) {
            vm.current = row;
        }
    });

    var result = avalon.define("ctrlResult", function (vm) {
        vm.result = "";
    });

    model.$watch("pageIndex", function (p1, p2, p3, p4) {
        console.log("$watch pageIndex");
        getList(model.pageIndex, function (data) {
            model.list = data.Rows;
        });
    });


   

    $(function (parameters) {
        //initpage
        getList(1, function (data) {
            model.list = data.Rows;
            avalon.scan(document.getElementById("divContainContent"), model);
        });
      
    });


});



