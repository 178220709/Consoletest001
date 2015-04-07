
define(function(require, exports, module) {
    var avalon = require("avalon.js");
   
     var indexModel = window.indexModel = avalon.define("msTopContent", function (vm) {
        vm.showContent = true;//content is load over 
        vm.crumbs = [];//the crumbs on the top 
        vm.contentUrl = "home/indexContent";
      
        vm.pageName = "Home"; 
        vm.pageDescription = "pageDescription";
        vm.refresh = function() {
           
        };

    });

    avalon.router.get("/*path", callback); //劫持url hash并触发回调
    avalon.history.start(); //历史记录堆栈管理
    function callback(el, e) {
        var hash = avalon.history.fragment;
        //不缓存每次都重新加载 供测试
       delete avalon.templateCache[hash];

        if (this.path === "/") {
            indexModel.contentUrl = "home/indexContent";
        } else {
            if (_.contains(hash, "LteDemo")) {
            }
            avalon.router.setLastPath(hash);
            indexModel.contentUrl = hash;
            indexModel.pageName = hash;
        }
    }

    $(function() {
        avalon.scan(document.getElementById("divTopContent"), indexModel);
        
        var hash = avalon.history.fragment;
        if (hash && hash!=="/") {
            indexModel.contentUrl = hash;
        } else {
            var lastPath = avalon.router.getLastPath();
            if (lastPath && lastPath !== "/") {
                indexModel.contentUrl = lastPath;
            } else {
                //host/#!/Home/ContextContent 如果都没命中 则填充默认页面
                indexModel.contentUrl = "home/indexContent";
            }
        }
    });     
  


});