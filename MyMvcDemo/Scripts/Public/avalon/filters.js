
define(function(require, exports, module) {
   
    var o = {};
    o.getHahaContent = function(content) {
        var text = $(content).find("p:first").text();
        if (text && text != "") {
            return text;
        } else {
            if ($(content).find("img").length > 0) {
                return $(content).find("img").attr("src");
            } else {
                return content;
            }
        }
    };
   


    module.exports = o;
});