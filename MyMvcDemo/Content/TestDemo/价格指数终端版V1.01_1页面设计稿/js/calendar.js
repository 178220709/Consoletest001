
$(function(){
	$( "#datepicker" ).datepicker({
	  dateFormat: "yy-mm-dd",
	  dayNames: ["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"],
	  dayNamesMin:["日", "一", "二", "三", "四", "五", "六"],
	  dayNamesShort:["日", "一", "二", "三", "四", "五", "六"],
	  monthNames: ["1月","2月","3月","4月","5月","6月","7月","8月","9月","10月","11月","12月"],
	  monthNamesShort: ["1","2","3","4","5","6","7","8","9","10","11","12"],
	  yearSuffix: '年',
      defaultDate: "+1w",
	  regional: "zh-TW",
      onClose: function( selectedDate ) {
        $( "#to" ).datepicker( "option", "minDate", selectedDate );
      }
    });
	$( "#from" ).datepicker({
	  dateFormat: "yy-mm-dd",
	  dayNames: ["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"],
	  dayNamesMin:["日", "一", "二", "三", "四", "五", "六"],
	  dayNamesShort:["日", "一", "二", "三", "四", "五", "六"],
	  monthNames: ["1月","2月","3月","4月","5月","6月","7月","8月","9月","10月","11月","12月"],
	  monthNamesShort: ["1","2","3","4","5","6","7","8","9","10","11","12"],
	  yearSuffix: '年',
      defaultDate: "+1w",
	  regional: "zh-TW",
      onClose: function( selectedDate ) {
        $( "#to" ).datepicker( "option", "minDate", selectedDate );
      }
    });
    $( "#to" ).datepicker({
	  dateFormat: "yy-mm-dd",
	  dayNames: ["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"],
	  dayNamesMin:["日", "一", "二", "三", "四", "五", "六"],
	  dayNamesShort:["日", "一", "二", "三", "四", "五", "六"],
	  monthNames: ["1月","2月","3月","4月","5月","6月","7月","8月","9月","10月","11月","12月"],
	  monthNamesShort: ["1","2","3","4","5","6","7","8","9","10","11","12"],
	  yearSuffix: '年',
      defaultDate: "+1w",
      onClose: function( selectedDate ) {
        $( "#from" ).datepicker( "option", "maxDate", selectedDate );
      }
    });
});