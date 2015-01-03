function loadContent(options) {
    if (_.isEmpty(options) || _.isEmpty(options.url)) {
        throw "必须指定要ajax读取的url";
    }
        
    var fullUrl = options.url;
    var loadUrl = "";
    if (options.data && !_.isEmpty(options.data)) {
        var query = $.param(options.data),
        ch = '?';

        if (_.str.endsWith(ch)) {
            ch = '&';
        }

        fullUrl = fullUrl + ch + query;        
    }

    var timestamp = (new Date()).valueOf();
    if (fullUrl.indexOf("?") >= 0) {
        loadUrl = fullUrl + "&timestamp=" + timestamp;
    } else {
        loadUrl = fullUrl + "?timestamp=" + timestamp;
    };

    util.block("#main-content");
    $("#main-content").attr("data-url", fullUrl);

    $("#main-content").load(loadUrl, function (responseText, textStatus, XMLHttpRequest) {
        util.unblock("#main-content");
        //var re = util.checkAjaxStatus(XMLHttpRequest.status);
        //if (re) {
            if ($.isFunction(options.onLoad)) {
                options.onLoad.apply(this, [responseText, textStatus, options]);
            }
        //}
        // 清除浮动
        $("#main-content").append("<div class='clear'></div>");
    });
}


function loadContentByVName(vname, options, url) {

    var li = $(_.template("#sidebar li[data-vname='<%= vname %>']", { vname: vname }));
    var url = !_.isEmpty(url) ? url : appGetModule(vname);
    $('#sidebar li').removeClass("active");
    if (!_.isEmpty(url)) {
        if (!_.isEmpty(vname)) {           
            li.addClass("active");
        }
        options = $.extend(options,
            {
                url: url
            });
        loadContent(options);
    }
}
var allModules; //saveAllModule
function appGetModule(vname) {
    if (_.isEmpty(allModules)) {
        $.ajax({
            type: 'POST',
            url: "/Module/GetAllModules",
            dataType: "JSON",
            async: false,
            success: function (result) {
                allModules = result;
            }
        });
    }
    var url = "";
    if (!_.isEmpty(vname)) {
        var m = _.find(allModules, function (item) {
            return item.VName == vname
        });
        url = m.Url;
    }
    return url;
}

function doContentRefresh() {
    var url = $("#main-content").attr("data-url");
    loadContent(
        {
            url: url
        });
}

$(function () {
    // === Sidebar navigation === //
    $('#menu_ul li').click(function () {
        //if (!$(this).hasClass("active")) {
            var url = $(this).attr("data-url");

            if (!_.isEmpty(url)) {
                $('#menu_ul li').removeClass("active");
                $(this).addClass("active");
            }

            if (!_.isEmpty(url)) {
                loadContent(
                    {
                        url: url
                    });
            }
        //}       
    });

    $("#btn-logout").click(function () {

        var href = $(this).attr("data-href");
 
        util.ask("退出", "您确定要退出系统吗？", function () {
          
            window.location = href;

        });

    });

    $("#btn-myPassword").click(function () {
        util.openPage(
            {
                id: "ChangePassword",
                title: "修改密码",
                url: "/Administrator/ChangeMyPassword",
                padding: 0
            });
    });

    $("#btn-FileManager").click(function () {
    
        util.fm(
           {
               multi: true
           });

    });

    $("#btn-myInfor").click(function () {
        
        loadContentByVName("", "", "/Administrator/UpdateMyProfile");
    });

    $("#main-content").on("click", "#refreshContent", function () {

        doContentRefresh();
    });

    $(document).click(function (me, a, b, c, d) { // 页面点击清除popover

        if ($(me.target).closest(".popover").length<=0) {
            $(".popover_autoClose").popover("destroy");
        }       
    });

    // 默认选中菜单第一项 现在是部门列表 到确定后在调整
    //appGetModule();
    var menuli = $('#menu_ul li');
    menuli.first().addClass("open");
    menuli.first().find("ul").show();
    for (var i = 0; i < menuli.length; i++) {
        var url = $(menuli[i]).attr("data-url");
        if (!_.isEmpty(url)) {
            $(menuli[i]).click();
            return;
        }
    }
    
});


