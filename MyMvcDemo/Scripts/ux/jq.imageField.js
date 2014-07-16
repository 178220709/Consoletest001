(function ($) {

    $.fn.imageField = function (options) {
        if (options == 'getValue') {

            return getValue($(this));

        } else {
            var opts = $.extend({}, $.fn.imageField.defaults, options);

            var result = this.each(function () {
                $this = $(this);
                var value =""; // 优先参数的赋值
                if (!_.isEmpty(opts.value)) {
                    value = opts.value;
                }
                if (_.isEmpty(value) && !_.isEmpty($this.attr('data-imageUrl'))) {
                    value = $this.attr('data-imageUrl');
                }
                
                var name = $this.attr('data-name');
                if (!_.isEmpty(opts.name)) {
                    name = opts.name;
                }

                if ($this.find(".control_imageField_container").length == 0) {

                    var $field = $('<div class="control_imageField_container"></div>');
                    $field.append('<input class="valueInput" type="hidden" value="' + value + '" name="' + name + '"/>');

                    var imgDiv = '<div class="control_imageField_container_imageDiv" style="width: ' + (opts.width + 6) + 'px;height:' + (opts.height + 6) + 'px;line-Height:' + (opts.height) + 'px" data-width="' + opts.width + '" data-height="' + opts.height + '"></div>'
                    if (!_.isEmpty(value) && value!=undefined) {
                        var imgHtml = _.template('<img src="<%= src %>?width=<%= width %>&height=<%= height %> ">',
                                      {
                                          src: value.AsUrl(),
                                          width: opts.width,
                                          height: opts.height
                                      });
                        $field.append($(imgDiv).append(imgHtml));
                    } else {
                        var blankHtml = '<div style="font-size:12px;">暂无</div>';
                        $field.append($(imgDiv).append(blankHtml));
                    }

                    var toolbarHtml = '<div class="control_imageField_toobbar" style="height:' + opts.height + 'px;">'
                        + '<div class="control_imageField_toobbar_div">'
                        + '    <button class="btn btn-xs btn-info" name="selectImage" type="button"><i class="icon-plus"></i>选择</button>';

                    if (opts.canCanel) {
                        toolbarHtml += '    <button class="btn btn-xs btn-danger" name="canelImage" style="margin-top:5px;"  type="button"><i class="icon-trash "></i>取消</button>';
                    }
                    toolbarHtml += '</div></div>';

                    $this.append($field.append(toolbarHtml));
                    $field.css("min-width", opts.width + 85);

                    // 选择按钮
                    $('button[name="selectImage"]', $field).click(function () {

                        var $btn = $(this);
                        util.fm(
                          {
                              multi: false,
                              isImage: true,
                              btnSelectImageSelector: $btn,
                              onSelected: function (data) {

                                  var $this = this.dialogAttrs.btnSelectImageSelector;
                                  var $imgDiv = $this.closest("div.control_imageField_container").find(".control_imageField_container_imageDiv");
                                  var $valueInput = $this.closest("div.control_imageField_container").find("input.valueInput");

                                  if (!_.isEmpty(data) && data.files.length > 0) {

                                      var imgsrc = _.template('<img src="<%= src %>?width=<%= width %>&height=<%= height %> ">',
                                          {
                                              src: data.files[0].RelativePath.AsUrl(),
                                              width: $imgDiv.attr("data-width"),
                                              height: $imgDiv.attr("data-height")
                                          });

                                      $imgDiv.empty().append(imgsrc);
                                      $valueInput.val(data.files[0].RelativePath);

                                  }
                              }
                          });
                    });

                    //清除按钮
                    $('button[name="canelImage"]', $field).click(function () {
                        var $imgDiv = $(this).closest("div.control_imageField_container").find(".control_imageField_container_imageDiv").empty().append("<div>暂无图片</div>");
                        var $valueInput = $(this).closest("div.control_imageField_container").find("input.valueInput").val("");
                    });
                }

            });

            return result;
        }
    };

    $.fn.imageField.defaults = {
        mame: "imageUrl",
        width: 100,
        height: 100,
        value: "",
        canCanel:true
    };
   
    function getValue($this) {

        return $(this).find('input.valueInput').val();
        
    }
})(window.jQuery);
