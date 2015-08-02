
define(function () {

    var wait = function (interval) {
        var dtd = $.Deferred(); // 新建一个deferred对象
        var tasks = function () {
            alert("执行1完毕！");
            var p1 = "p1";
            var p2 = "p2";

            dtd.resolve(p1, p2); // 改变deferred对象的执行状态
        };
        setTimeout(tasks, interval);
        return dtd.promise();
    };
    var wait2 = function (interval) {
        var dtd = $.Deferred(); // 新建一个deferred对象
        var tasks = function () {
            alert("执行2完毕！");
            var p1 = "p21";
            var p2 = "p22";

            dtd.resolve(p1, p2); // 改变deferred对象的执行状态
        };
        setTimeout(tasks, interval);
        return dtd.promise();
    };

    $.when(wait(3000))
        .done(function (p1,p2,p3) {
            var me = this;
            console.log("1");
        })
        //.then(wait2(2000))
        .done(function (p1, p2, p3) {
            var me = this;
            console.log("2");
        });


});



