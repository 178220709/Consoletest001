
define(function(require, exports, module) {
    var avalon = require("avalon.js");
    console.log("init page");
    var model = avalon.define("ctrlContent", function (vm) {
        vm.str = "this is str ";
        vm.number = 888;
        vm.radio = true;
        vm.selArr = ["A","B","C","D","E"];
        vm.sel = "C";
       
    });

    var result = avalon.define("ctrlResult", function (vm) {
        vm.result = "";
    });

    model.$watch("$all", function(p1,p2,p3,p4) {
        result.result = JSON.stringify(model.$model,4, "\t");
    });

    $(function(parameters) {
        avalon.scan(document.getElementById("divContainContent"), indexModel);
    });


});



