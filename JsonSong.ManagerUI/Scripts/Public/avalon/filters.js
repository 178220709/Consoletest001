
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

    o.getColorLevel = function (weight) {
        if (weight < 0) {
            return "label-danger";
        }
        if (weight == 0) {
            return "label-warning";
        }
        if (weight > 0 && weight < 100) {
            return "label-primary";
        }
        return "label-success";
    };



    module.exports = o;
});