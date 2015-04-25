
define(function (require, exports, module) {
    var avalon = require("avalon.js");
    var filters = require("public/avalon/filters.js");
    var pagerInit = require("public/avalon/page.js");

    var getList = function(pageIndex,callback) {
        $.post("spider/getList", { PageSize: 10, PageIndex: pageIndex, typeId: model.typeId }, function (data) {
            if (_.isFunction(callback)) {
                callback(data);
            }
        });
    };

    var model = avalon.define("ctrlSpiderHahaList", function (vm) {
        var pager = new pagerInit(vm);
        vm.list = [];
        vm.typeId = 1;
        vm.pageIndex = 1;
        vm.pageCount = 1;
        vm.current = {};
        vm.IsPartial = false;
        vm.pList = [];
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
      //  vm.changePage = pager.changePage;
        vm.del = function (row) {
            if (confirm("sure?")) {
                $.post("spider/Delete", { url: row.Url, typeId: model.typeId }, function (data) {
                    if (data.success) {
                        alert("Delete成功");
                        model.refreshClick();
                    } else {
                        alert("Delete错误");
                    }
                });
            }
        };
        vm.updateClick = function (row) {
            //row是avalon设置监听过的复杂对象  不能直接去序列化它
            var m1 = row;
            var m2 = row.$model;
            $.post("spider/update", { modelStr: JSON.stringify(m2) }, function (data) {
                if (data.success) {
                    alert("更新成功");
                } else {
                    alert("更新错误");
                }
            });
        };
        vm.nextClick = function (p1) {
            var list = vm.list.$model;
            var url = vm.current.Url;
            for (var i = 0; i < list.length; i++) {
                if (url == list[i].Url) {
                    if (i == list.length - 1) {
                        vm.pageIndex = vm.pageIndex + 1;
                    } else {
                         vm.current = list[i+1];
                    }
                } 
            }
        };

    });
    model.$watch("pageIndex", function (p1, p2, p3, p4) {
        model.refreshClick();
    });
    model.$watch("typeId", function (p1, p2, p3, p4) {
        model.refreshClick();
    });
    model.$watch("current", function (p1) {
        var content = model.current.Content;
        var ps = $(content).find("p");
        if (ps.length>15) {
            model.IsPartial = true;
        }
        model.pList = _.map(ps, function(p) {
            return $(p).prop("outerHTML");
        });
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



