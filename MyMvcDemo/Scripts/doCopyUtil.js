var doCopyUtil = (function () {
    var o = {};

    o.initDoCopy = function (el,valOrFun,beforeFun,callFun) {
        var isIe = false;
        if (window.clipboardData) {
            isIe = true;
        }
        
        el.each(function () {
            var $this = $(this);
         
            var value = valOrFun;
            if ($.isFunction(valOrFun)) {
                value = valOrFun.apply(this, [$this]);
            }

            //// IE 处理
            if (isIe) {
                $this.attr("data-copyData-doCopy", value)
                $this.die("click");
                $this.live("click", function () {
                    window.clipboardData.setData("Text", $this.attr("data-copyData-doCopy"));
                    util.note({ title: '成功', text: "复制成功。" });
                });
                
            } else { // 非IE
                $this.closest(".btn-copy").find(".zclip").remove();
                $this.zclip({
                    path: '/Content/libs/jquery.zclip.1.1.1/ZeroClipboard.swf',
                    copy: value,
                    afterCopy: function () {
                        util.note({ title: '成功', text: "复制成功。" });
                    }
                });
            }            
        });
        
    }
   
    return o;
})();