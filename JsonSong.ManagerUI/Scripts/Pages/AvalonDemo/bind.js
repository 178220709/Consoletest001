
define(function(require, exports, module) {
    var arr = [1,2,3,4,5];
    var _ = require("libs/underscore/underscore.string"); 
    var str1 = _.ltrim("~test~",'~');
     _ = require("libs/underscore/underscore");
     var arr2 = _.filter(arr, function(i) {
         return i % 2 == 0;
     });
     _ = require("libs/underscore/underscore.string");
     var str2 = _.rtrim("~test~", '~');
});