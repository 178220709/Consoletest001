$(function () {
    $.fn.datetimepicker.defaults = {
        language: 'zh-CN',
        pickerPosition: "bottom-left",
        weekStart: 1,
        todayBtn: 1,
        autoclose: 1,
        todayHighlight: 1,
        startView: 2,
        forceParse: 0,
        showMeridian: 1,
        startDate: '1990-01-01',
        endDate: '2099-12-12',
        format: 'yyyy-mm-dd hh:ii:ss',
        initialDate: new Date()
    };

    $.fn.datepicker.defaults = {
        autoclose: true,
        language: "zh-CN",
        todayHighlight: true,
        todayBtn: 'linked',
        format: _defaults.date.shortDate
    };
});
