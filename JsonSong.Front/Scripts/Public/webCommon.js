
//页面初始化执行的js
function getContentHeight() //函数：获取尺寸 
{
    var contentHeight = 850;
    //获取窗口高度 
    if (window.innerHeight)
        contentHeight = window.innerHeight;
    else if ((document.body) && (document.body.clientHeight))
        contentHeight = document.body.clientHeight;
        //通过深入Document内部对body进行检测，获取窗口大小 
    else if (document.documentElement && document.documentElement.clientHeight ) {
        contentHeight = document.documentElement.clientHeight;
    }
    return contentHeight;
}

$(function () {
    var initBodyHeigh = function () {
        var h = getContentHeight();
        h = h - 60 - 40; //去掉头部和底部的高度
      //  $("#main-content").css("min-height", h);
    }();





});
