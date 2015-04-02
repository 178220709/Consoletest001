
define(function (require, exports, module) {
    var avalon = require("avalon.js");

    var indexModel = window.indexModel = avalon.define("msTopContent", function (vm) {
        vm.showContent = true;//content is load over 
        vm.crumbs = [{ url: "#!", name: "name1" }];//the crumbs on the top 
        vm.contentUrl = "";

        vm.pageName = "Home";
        vm.pageDescription = "";
        vm.refresh = function () {
            for (var key in seajs.cache) {
                if (_.contains(key, "/Scripts/Pages")) {
                    seajs.cache[key].destroy();
                }
            }
            //var currentUrl = vm.contentUrl;
            //vm.contentUrl = "home/waiting";
            vm.contentUrl = vm.contentUrl;
        };
    });

    exports.VM = indexModel;
    exports.initPage = function() {
        $(function () {
            avalon.scan(document.getElementById("divTopContent"), indexModel);
        });
    }



  
  


});