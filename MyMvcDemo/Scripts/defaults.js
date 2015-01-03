
_defaults =
    {
        date: {//jQgrid 必须用大写 ，date-picker 用小写。。。
            fullDT: "YYYY-MM-DD ddd HH:mm",
            defaultDT: "YYYY-MM-DD HH:mm",
            defaultWeekDT: "YYYY-MM-DD HH:mm",
            DT_short: "YYYY-MM-DD",
            DT: "YYYY-MM-DD ddd",
            ISO8601: "YYYY-MM-DDTHH:mm:ss",
            shortDate:"yyyy-mm-dd"
        },
        moneySymbols: {
            RMB: "￥"
        }
    };

fullCal = {
    defaults:
        {
            timeFormat: 'H:mm'
        },
    locale:
        {
            "zh-cn":
            {
                monthNames: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
                monthNamesShort: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
                dayNames: ['星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'],
                dayNamesShort: ["周日", "周一", "周二", "周三", "周四", "周五", "周六"],
                buttonText: {
                    today: '今天',
                    month: '月',
                    week: '周',
                    day: '日'
                }
            }
        }
}
