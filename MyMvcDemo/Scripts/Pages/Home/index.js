﻿
define(function(require, exports, module) {
    var avalon = require("avalon/mmRouter.js");
    var _ = require("underscore/underscore.string");
    var indexModel = avalon.define("msTopContent", function (vm) {
        vm.showContent = true;//content is load over 
        vm.crumbs = [{url:"hahaha",name:"name1"},{url:"ha22a",name:"name2"}];//the crumbs on the top 
        vm.contentUrl = "home/indexContent"; 
      
        vm.pageName = "Home"; 
        vm.pageDescription = "";
    });

    avalon.router.get("/*path", callback); //劫持url hash并触发回调
    avalon.history.start(); //历史记录堆栈管理
    function callback(el, e) {
        var hash = avalon.history.fragment;
        if (this.path === "/"  ) {
            indexModel.contentUrl = "home/indexContent";
        } else {
            // indexModel.contentUrl = this.path;  //动态修改pageUrl属性值
            if (_.contains(hash, "LteDemo")) {
                seajs.use("lte/dashboard");
                seajs.use("lte/demo");
               
            }
            indexModel.contentUrl = hash;
        }
    }

    $(function() {
        avalon.scan();
    });     
  

});