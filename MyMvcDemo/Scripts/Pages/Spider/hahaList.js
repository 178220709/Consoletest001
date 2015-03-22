
define(function (require, exports, module) {
    var avalon = require("avalon.js");
    var filters = require("public/avalon/filters.js");
    var pagerInit = require("public/avalon/page.js");

    var getList = function(pageIndex,callback) {
        $.post("spider/getList", { PageSize: 4, PageIndex: pageIndex }, function (data) {
            if (_.isFunction(callback)) {
                callback(data);
            }
        });
    };

    var model = avalon.define("ctrlSpiderHahaList", function (vm) {
        var pager = new pagerInit(vm);
        vm.list = [];
        vm.pageIndex = 1;
        vm.pageCount = 1;
        vm.current = {};
        vm.getWeightClass = filters.getColorLevel;
        vm.trClick = function (row) {
            vm.current = row;
        };
        vm.refreshClick = function() {
            getList(vm.pageIndex, function(data) {
                vm.list = data.Rows;
                vm.pageCount = data.PageCount;
                vm.current = _.first(vm.list);
            });
        };
        vm.getContent = filters.getHahaContent;
        vm.changePage = pager.changePage;
        vm.del = function (row,delFun) {
            if (confirm("sure?")) {


                if (_.isFunction(delFun)) {
                    delFun();
                }
            }
            $.post("spider/Delete", row.Flag, function (data) {
                if (data.success) {
                    alert("更新成功");
                } else {
                    alert("更新错误");
                }
            });
        };
        vm.updateClick = function (row) {
            //row是avalon设置监听过的复杂对象  不能直接去序列化它
            var m1 = row;
            var m2 = row.$model;
            $.post("spider/update", m2, function (data) {
                if (data.success) {
                    alert("更新成功");
                } else {
                    alert("更新错误");
                }
            });
        };
    });
    model.$watch("pageIndex", function (p1, p2, p3, p4) {
        model.refreshClick();
    });

    $(function (parameters) {
        //initpage
        getList(1, function (data) {
            model.list = data.Rows;
            model.pageCount = data.PageCount;
            avalon.scan(document.getElementById("divContainContent"), model);
           
        });
      
    });


});



