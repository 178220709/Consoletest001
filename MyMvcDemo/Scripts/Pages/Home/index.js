
define(function(require, exports, module) {
    var avalon = require("avalon.js");
   
    var _ = require("libs/underscore/underscore.string");
     _ = require("libs/underscore/underscore");
    var indexModel = avalon.define("msTopContent", function (vm) {
        vm.showContent = true;//content is load over 
        vm.crumbs = [{url:"#!",name:"name1"},{url:"ha22a",name:"name2"}];//the crumbs on the top 
        vm.contentUrl = "home/indexContent"; 
      
        vm.pageName = "Home"; 
        vm.pageDescription = "";
    });

    avalon.router.get("/*path", callback); //劫持url hash并触发回调
    avalon.history.start(); //历史记录堆栈管理
    function callback(el, e) {
        var hash = avalon.history.fragment;
        //不缓存每次都重新加载 供测试
       //delete avalon.templateCache[hash];

        if (this.path === "/") {
            indexModel.contentUrl = "home/indexContent";
        } else {
            if (_.contains(hash, "LteDemo")) {
            }
            avalon.router.setLastPath(hash);
            indexModel.contentUrl = hash;

        }
    }

    $(function() {
        avalon.scan(document.getElementById("divTopContent"), indexModel);
        var lastPath = avalon.router.getLastPath();
        indexModel.contentUrl = lastPath;
    });     
  

});