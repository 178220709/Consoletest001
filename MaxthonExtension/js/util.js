var util = (function() {
    var rt = window.external.mxGetRuntime();
    // 获取浏览器接口
    var browser = rt.create("mx.browser");
   
    var o = {
        getBrowser: function () {
            return browser;
        },
        getCurrentTag: function () {

        },
        showEveryAttr: function(obj) {
            var str = "";
            for (var attr in obj) {
                str += attr + ":" + obj[attr] ;
            }
            return str;
        }

    };

    return o;
})();




