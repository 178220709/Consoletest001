(function ($) {

    $.fn.optionField = function (options) {
        if (options == 'getValue') {
            return getValue($(this));
        } else {
            var opts = $.extend({}, $.fn.imageField.defaults, options);

            var result = this.each(function () {
                $this = $(this);
                init($this, opts);
            });
            
            return result;
        }
    };

    $.fn.optionField.defaults = {
        height: 200,
        value: "",
    };

    function getValue($this) {
        
        var data = null;
        var $trs = $this.find(".options_ctl_Table .st-body-table tr");
        if ($trs.length>0) {
            data = [];
            $trs.each(function () {
                var v = $(this).find('input[name="OptionValue"]').val();
                if (!_.isEmpty(v)) {
                    data.push(v);
                }
            });
        }
        return data;
    }

    function init($this, opts) {
        var value = []; // 优先参数的赋值
        if (!_.isEmpty(opts.value)) { // value 为数组
            value = opts.value;
        } else if (!_.isEmpty($this.attr('data-value'))) {
            value = JSON.parse($this.attr('data-value'));
        }
        var height = 200;
        if (!_.isEmpty(opts.height) || opts.height > 0) {
           
            height = opts.height;
        } else if (!_.isEmpty($this.attr('data-height'))) {
            height = JSON.parse($this.attr('data-height'));
        }

        var conEl = ' <div class="widget-box options_ctl_container">'
	                    + '<div class="widget-header header-color-blue">'
		                   + '<div><button class="btn btn-xs btn-success " name="btn-AddOptions" type="button">'
				            + '<i class="icon-plus"></i>添加选项'
                        + '</button></div></div>'
                        + '<div class="widget-body">'
                           + '<table class="table table-bordered options_ctl_Table options_ctl_Table" style="margin-bottom:0;">'
                           + '<thead>'
                            + '<tr><th width="65%">选项</th><th width="35%">删除</th></tr>'
                                 + '</thead>'
                                 + '<tbody ></tbody>'
                            + ' </table>'
                            + '</div></div>';
        var trEl = '<tr><td><input type="text" title="请输入选项" name="OptionValue" maxlength="100" autocomplete="off" style="width: 95%;" value="<%=value%>"></td>'
            + '<td><button class="btn btn-xs btn-danger" type="button" name="btn_tr_remove"><i class="icon-trash "></i>删除</button></td></tr>';

        var $cont = $(conEl);
        $this.append($cont);

        // 初始
        if (!_.isEmpty(value) && value.length > 0) {
            _.each(value, function (v) {

                var $tr = $(_.template(trEl, { value: v }));
                $cont.find("tbody").append($tr);

                $tr.find('button[name="btn_tr_remove"]').click(function () {
                    var $trbtn = $(this);
                    util.ask("删除", "您确定要删除该选项吗？", function () {
                        $trbtn.closest("tr").remove();
                    });
                });
            });
        }

        // 滚动条
        $cont.find(".options_ctl_Table").scrolltable({
            stripe: true,
            height: height
        });

        $cont.find(".options_ctl_Table .st-body-table tbody").sortable();
        $cont.find(".st-body-scroll").css("overflow-x", "hidden");
        // 添加选项
        $cont.find('button[name="btn-AddOptions"]').click(function () {
            var $tr = $(_.template(trEl, {value:""}));
            $(this).closest(".options_ctl_container").find(".options_ctl_Table .st-body-table tbody").append($tr);

            $tr.find('button[name="btn_tr_remove"]').click(function () {
                var $trbtn = $(this);
                util.ask("删除", "您确定要删除该选项吗？", function () {
                    $trbtn.closest("tr").remove();
                });
            });
        });
        
    }

})(window.jQuery);