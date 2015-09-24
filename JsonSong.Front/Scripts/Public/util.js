
define(function () {
    var host = function () {
        var _host = "";
        var js = document.scripts;
        var index = js.length - 1;
        //amd异步加载 就在第一个 script标签加载就是最后一个
        if (require.define.amd) {
            index = 0;
        }
        var jsPath = js[index].src;
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
    o.setHost = function (path) {
        host = s.rtrim(path, "/");
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
    return o;
});


//avalon 公共
avalon.directive("enter", {
    init: function(binding) {
        var elem = binding.element
        var vmodels = binding.vmodels
        var remove = avalon(elem).bind("click", function(p1,p2) {
            if (p1) {
                var code = p2;
            }
        });
        binding.roolback = function() {
            avalon(elem).unbind("click", remove)
        }
    }

});
