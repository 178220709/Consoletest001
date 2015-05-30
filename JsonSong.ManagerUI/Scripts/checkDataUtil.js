var checkDataUtil = (function () {
    var o = {};


    o.IsEmail = function(str) {
        if (str.length != 0) {
            var reg = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
            return reg.test(str);
        }
        return false;
    };

    o.IsMobile = function(str) {
        if (str.length != 0) {
            var reg = /^((\(\d{2,3}\))|(\d{3}\-))?13\d{9}$/;
            return reg.test(str);
        }
        return false;
    };
    o.checkIsNumber = function (selector) {
        return !/^-?(?:\d+|\d{1,3}(?:,\d{3})+)(?:\.\d+)?$/.test($(selector).val());
    };
    return o;
})();