
define(function(require, exports, module) {
    var avalon = require("avalon/avalon.shim.js");
    var _ = require("underscore/underscore.string");
    var baseUrl = seajs.config.base;
    var o = {};
    o.CreateCrumbs= function(parameters) {
        
    }
    o.CombinPath = function (root ,path) {
        return _.rtrim(root, '/') +"/"+ _.ltrim(path, '/');
    }


    module.exports = o;
});