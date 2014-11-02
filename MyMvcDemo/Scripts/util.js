//
String.prototype.AsUrl = function() {
    var result = this;
    if (!_.isEmpty(result)) {
        result = $$sc.StaticsUrl + this;
    }

    return result;
};

$(function () {
    // ajax 错误处理
    $.ajaxSetup({
        error: function (jqXHR, textStatus, errorThrown) {
     
            util.checkAjaxStatus(jqXHR.status, jqXHR.responseText);
        }
    });

    $.fn.cke_setHeight = function (height) {
       
        return this.each(function () {
            var $this = $(this);
            $this.next("div.cke").find(".cke_contents").css("height", height);
        });
    };

});

//初始化artdialog
seajs.config({
    base: "/content/libs"
});
seajs.use(['artDialog-6.0/src/dialog'], function(dialog) {
    $.dialog = dialog;
});

var util = (function () {
    var o = {};

    o.openPage = function (options) {
        var id = options.id || _.uniqueId(),
            url = options.url;

        options.url = null;

        var args = _.extend(
        {
            fixed: true,
            lock: true,
            //长宽设置成60以显示loading图标
            width: 60,
            height: 60,
            onshow: function () {
                var me = this,
                    conrentArea = $("[i='content']", this.node.firstChild);

                var fullUrl = url;
                if (options.data && !_.isEmpty(options.data)) {
                    var query = $.param(options.data),
                    ch = '?';

                    if (_.str.endsWith(ch)) {
                        ch = '&';
                    }

                    fullUrl = fullUrl + ch + query;
                }
                $(conrentArea).load(fullUrl, function (responseText, textStatus, XMLHttpRequest) {
                    var dialog = $.dialog.get(id);
                    //将宽高设置成auto以让窗口自适应内容大小
                    dialog.width("auto");
                    dialog.height("auto");

                    if (!options.title || _.isEmpty(options.title)) {
                        var title = $("title", this).text();
                        dialog.title(title);
                    }

                    if ($.isFunction(options.onLoad)) {
                        options.onLoad.apply(me, [responseText, textStatus, options]);
                    }
                });
            },
            onclose: function () {
                if ($.isFunction(options.onClose)) {
                    options.onClose.apply(this);
                }
            }
        }, options);

        var d = $.dialog(args);

        if (args.lock) {
            d.showModal();
        }
        else {
            d.show();
        }

        return d;
    }

    o.block = function (ele, message) {
        var html = '<div style="margin-bottom: 5px;"><%=message%></div><img  src="/Content/imgs/spinner-md.gif" >';

        if (_.isEmpty(message)) {
            html = _.template(html, { message: "正在执行,请稍等..." });
        } else {
            html = _.template(html, { message: message });
        }

        $(ele).block(
            {
                css: {
                    border: 0,
                    backgroundColor: 'rgba(255, 255, 255, 0)'
                },
                overlayCSS: {
                    opacity:0.5,
                    backgroundColor: '#fff',
                    cursor: 'wait'
                },
                message: html
            });
    }

    o.unblock = function (ele) {
        $(ele).unblock();
    }

    // 列表内编辑删除 成功消息框
    o.note = function (options) {
        $.gritter.add(options);
    }

    // 错误消息框
    o.err = function (title, content, closeFun) {
      
        var d = $.dialog({
            id: "errorMessageDialog",
            title: title,
            content: $.isArray(content) ? content.join("<br/>") : content,
            cancelValue: '关闭',
            cancel: function () {
                if ($.isFunction(closeFun)) {
                    closeFun.call(this);
                }
            },
            onshow: function () {
                var conrentArea = $("[i='content']", this.node.firstChild);
                conrentArea.css(
                    {
                        minWidth: 150
                    });
            }
        }).showModal();
    }

    // 通知消息框
    o.message = function (title, content, closeFun) {

        var d = $.dialog({
            id: "messageDialog",
            title: title,
            content: $.isArray(content) ? content.join("<br/>") : content,
            cancelValue: '关闭',
            cancel: function () {
                if ($.isFunction(closeFun)) {
                    closeFun.call(this);
                }
            },
            onshow: function () {
                var conrentArea = $("[i='content']", this.node.firstChild);
                conrentArea.css(
                    {
                        minWidth: 150
                    });
            }
        }).showModal();
    }

    // 询问对话框
    o.ask = function (title, content, okFun, cancelFun) {
       
        var d = $.dialog(
           {
               title: title,
               content: content,
               ok: function () {
                   if (_.isFunction(okFun)) {
                       okFun.apply(this);
                   }
               },
               cancel: function () {
                   if (_.isFunction(cancelFun)) {
                       cancelFun.apply(this);
                   }
               },
               onshow: function () {
                   var conrentArea = $("[i='content']", this.node.firstChild);

                   conrentArea.css(
                       {
                           minWidth:150
                       });
               }
           }).showModal();
    }

    // 成功添加询问框
    o.success = function (title, content, continueFun, redirectFun, continueBtnText, redirectBtnText) {
        if (_.isEmpty(continueBtnText)) {
            continueBtnText = '继续添加/编辑';
        }
        if (_.isEmpty(redirectBtnText)) {
            redirectBtnText = '转到列表';
        }

        var d = $.dialog({
            title: title,
            content: content,
            lock: true,
            fixed: true,
            button: [
                {
                    value: continueBtnText,
                    callback: function () {
                        if ($.isFunction(continueFun)) {
                            continueFun.call(this);
                        }
                    },
                    autofocus: true
                },
                {
                    value: redirectBtnText,
                    callback: function () {
                        if ($.isFunction(redirectFun)) {
                            redirectFun.call(this);
                        }
                    }
                }
            ]
        }).showModal();
    }

    // 正则判断
    o.regex = function (value, regex) {
        var reg = new RegExp(regex);
        return reg.test(value);
    }

    // 检查状态码 403 访问被拒绝（无权访问）；678，登录超时；500服务器错误；正常返回True 
    o.checkAjaxStatus = function (status,ms)
    {

        var result = false;
        switch (status) {
            case 403:
                util.err("错误", "拒绝访问!");
                break;
            case 678:                
                util.err("错误", "登录超时，请重新登录。", function () {
                    location.reload();
                });
                break;
            case 500:
                util.err("错误", "程序发生错误!");
                break;
            default:
                if (!_.isEmpty(ms)) {// 服务端验证
                    util.err("错误", ms);
                }
                result = true;
                break;
        }
        return result;
    }

    o.getDefaultLanguage = function () {
        var cookie_defaultLnag = "shopfun.v2.cookie.defaultLnag";
        var lang = $.cookie(cookie_defaultLnag);
        if (lang && lang.length && lang != "null" ) {
           
            return JSON.parse(lang);
        } else {
            $.ajax({
                url: "/language/GetDefaultLang",
                type: "post",
                dataType: "json",
                async:false,
                success: function (data) {
                    lang = data;
                    $.cookie(cookie_defaultLnag, JSON.stringify(data));
                }
            });
            return lang;
        }
    }

    o.fm = function (options) {
        options = options || {};
        o.openPage(
            {
                id: "dFM",
                url: "/fm/index",
                padding: 0,
                width: 850,
                height: 600,
                title:"文件管理器",
                dialogAttrs: options,
                onClose: function () {
                 
                    if ($.isFunction(options.onSelected)) {
                        options.onSelected.apply(this, [this.returnValue]);
                    }
                }
            });
    }

    o.bytesToSize = function(bytes, decimals) {
        var units = ['bytes', 'KB', 'MB', 'GB', 'TB', 'PB'],
            size = bytes,
            unit = units[0],
            decimals = decimals ? decimals : 2;

        if (size > 0) {
            var e = Math.floor(Math.log(bytes) / Math.log(1024));
            size = (bytes / Math.pow(1024, Math.floor(e))).toFixed(decimals);
            //去除掉末尾的零，如果小数点后全部是零则再把小数点也去掉
            size = size.toString().replace(/(\.[0-9]*?)0+$/, "$1").replace(/\.$/, "");
            unit = units[e];
        }

        return { size: size, unit: unit };
    };

    o.openNewWindow = function(url, opts) {
        if (opts) {
            var index = 0;
            for (var name in opts) {
                url += index = 0 ? "?" : "&" + name + opts[name];
                index++;
            }

        }
        window.open(url);
    };
    return o;
})();
