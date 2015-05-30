
define(function (require, exports, module) {
    var host = function () {
        var _host = "";
        var js = document.scripts, jsPath = js[js.length - 1].src;
        return _host ? _host : jsPath.substring(0, jsPath.indexOf("/Scripts"));
        //eg:http://localhost/SpartanLv.Web
    }();


    var o = {};

    o.optsToStr = function (opts) {
        var str = "";
        if (opts) {
            var index = 0;
            for (var name in opts) {
                str += (index == 0 ? "?" : "&") + name + "=" + opts[name];
                index++;
            }
        }
        return str;
    };
    o.getHost = function () {
        return host;
    };
    //path eg: controllerName/actionName
    o.getAbsUrl = function (path) {
        if (path.indexOf('/') == 0) {
            return host + path;
        }
        return host + "/" + path;
    };
    module.exports = o;
});
