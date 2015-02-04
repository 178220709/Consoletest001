
define(function(require, exports, module) {
    var avalon = require("avalon/avalon.shim.js");
    var model = avalon.define("testcon", function(vm) {
        vm.aaa = true;
        
        vm.bbb = "@@@";
        vm.ccc = "&&&";
        vm.active = "active";
    });
});