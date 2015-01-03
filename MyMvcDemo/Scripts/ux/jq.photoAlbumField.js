(function ($) {

    $.fn.photoAlbumField = function (options) {
        if (options == 'getValue') {

            return getValue($(this));

        } else {
            // 初始化
            var opts = $.extend({}, $.fn.photoAlbumField.defaults, options);

            var result = this.each(function () {
                $this = $(this);
                var value = []; // 优先参数的赋值
                if (!_.isEmpty(opts.value)) { // value 为数组
                    value = opts.value;
                } else if (!_.isEmpty($this.attr('data-value'))) {
                    value = JSON.parse($this.attr('data-value'));
                }
                var height = 500;
                if (!_.isEmpty(opts.height)) { 
                    height = opts.value;
                } else if (!_.isEmpty($this.attr('data-height'))) {
                    height = JSON.parse($this.attr('data-height'));
                }
                var bigImageDivHeight = height - 170;
                var fieldName = $this.attr('data-name');
                if (!_.isEmpty(opts.name)) {
                    fieldName = opts.name;
                }
                var html = '<div class="widget-box photoAlbumFild_container">'
                            + '<div class="widget-header header-color-blue">'
                                + '<div>'
                                    + '<button class="btn btn-xs btn-success " type="button" name="btn-photoAdd">'
                                        + '<i class="icon-caret-down"></i>'
                                    + '添加图片'
                                + '</button>'
                                + '<button class="btn btn-xs btn-danger " type="button" name="btn-photoClearAll" style="margin-left: 10px;">'
                                    + '<i class="icon-remove"></i>'
                                    + '清除所有图片'
                                + '</button>'
                            + '</div>'
                        + '</div>'
                        + '<div class="widget-body product-photo">'
                        + '<div class="photo-list">'
                                + '<ul></ul>'
                            + '</div>'
                            + '<div class="photo-bigImage-container">'
                                + '<div class="photo-bigImage" style="height:' + bigImageDivHeight + 'px"></div>'
                            + '</div>'
                        + '</div>'
                    + '</div>';

                if ($this.find(".photoAlbumFild_container").length == 0) {

                    $this.append(html);
                    if (opts.is360) {
                        var el360 = '<div class="pull-right" style="margin: 6px 30px;">'
                            + '<button class="btn btn-xs btn-purple " type="button" name="btn-photo360Start">'
                                        + '<i class="glyphicon glyphicon-play"></i>'
                                    + '360度预览'
                                + '</button>'
                                + '<button class="btn btn-xs btn-danger " type="button" name="btn-photo360Stop" style="margin-left: 10px;">'
                                    + '<i class="glyphicon glyphicon-stop"></i>'
                                    + '停止'
                                + '</button><div>'
                        $this.find(".widget-header").append(el360)
                    }
                    var $thumblist = $this.find('.photo-list ul');
                    //  排序
                    $thumblist.sortable();

                    if (!_.isEmpty(value)) {
                        _.each(value, function (item) {
                            var imageUrl = item.ImageUrl;
                            imageName = imageUrl.replace(/(.*\/){0,}([^\.]+).*/ig, "$2");
                            appendImage($thumblist, {
                                value: imageUrl,
                                src: imageUrl.AsUrl(),
                                fieldName: fieldName,
                                width: opts.thumbImageWidth,
                                height: opts.thumbImageHeight,
                                imageName: imageName
                            });
                        });
                        setSelect($this, 0);
                    }

                    // 添加
                    $('button[name="btn-photoAdd"]', $this).click(function () {

                        selectFm($(this), fieldName, opts.thumbImageWidth, opts.thumbImageHeight);
                    });

                    // 清除
                    $('button[name="btn-photoClearAll"]', $this).click(function () {
                        clearAll($(this));
                    });

                    // 添加
                    $('button[name="btn-photo360Start"]', $this).click(function () {

                        do360View($(this), opts.opts360.imgHeight, opts.opts360.imgWidth);
                    });

                    // 清除
                    $('button[name="btn-photo360Stop"]', $this).click(function () {
                        do360Stop($(this));
                    });

                }

            });

            return result;
        }
       
    };

    $.fn.photoAlbumField.defaults = {
        name: "photoImageUrls",
        thumbImageHeight: 64,
        thumbImageWidth: 64,
        value: "",
        is360: false,
        opts360: {
            imgHeight:400,
            imgWidth:400
        }
    };
       
    function appendImage($thumblist, opts) {

        var liEl = '<li class="photo-item" >'
                            + '<input class="valueinput" type="hidden" value="<%= value %>" name="<%= fieldName %>"/>'
                            + '<button class="photo-btnClose">×</button>'
                            + '<div class="photo-thumb">'
                                + '<img src="<%= src %>?width=<%= width %>&height=<%= height %> " title="<%= imageName %>" >'
                            + '</div>'
                            + '<div class="photo-name" title="<%= imageName %>"><%= imageName %></div>'
                        + '</li>';
        var $thumb = $(_.template(liEl,opts));
        $thumblist.append($thumb);

        $thumb.click(function () {
          
            thumbImageClick($(this));
        });
        $thumb.find('.photo-btnClose').click(function () {
            deleteImage($(this));
        });
    }

    function selectFm($btn, fieldName, imgWidth, imgHeight) {
        util.fm(
        {
            multi: true,
            isImage: true,
            btnSelectImageSelector: $btn,
            onSelected: function (data) {

                var btn = this.dialogAttrs.btnSelectImageSelector;

                if (!_.isEmpty(data)) {
                    var $con = $(btn).closest('.photoAlbumFild_container');
                    var $thumblist = $con.find('.photo-list ul');
                    _.each(data.files, function (item) {

                        appendImage($thumblist, {
                            value: item.RelativePath,
                            src: item.Uri,
                            fieldName: fieldName,
                            width: imgWidth,
                            height: imgHeight,
                            imageName: item.FileName
                        });
                    });

                    // 设置最后一个显示大图
                    do360Destroy($con);
                    setSelect($con, $thumblist.find('li').length-1);

                }

            }
        });
    }

    function clearAll($btn) {
        util.ask("删除", "您确定要删除相册图片吗？", function () {
            var $con = $btn.closest('.photoAlbumFild_container');
            $con.find('.photo-list ul').empty();
            $con.find('.photo-bigImage').empty();

            do360Destroy($con);
        });
    }

    function deleteImage($btn)
    {
        util.ask("删除", "您确定要删除选中的图片吗？", function () {

            var $con = $btn.closest('.photoAlbumFild_container');
            var $item = $btn.closest('li.photo-item');
            var $allItem = $item.closest('.photo-list ul').find('li');
            var index = $allItem.index($item);

            if ($allItem.length == 1) {
                $item.remove();
                $con.find('.photo-bigImage').empty();
            } else if (index == $allItem.length -1) {
                setSelect($con, index - 1);
                $item.remove();
            } else {
                setSelect($con, index + 1);
                $item.remove();
            }

        });
    }

    function thumbImageClick($this) {

        if (!$this.hasClass('photo-li-selected')) {
            
            var index = $this.closest('.photo-list ul').find('li').index($this);
            var $con = $this.closest('.photoAlbumFild_container');
            setSelect($con, index);
        }
    }

    function setSelect($con, index) {

        var $this = $con.find('.photo-list ul li').eq(index);

        var imageUrl = $this.find('input.valueinput').val();
        var $bigImageDiv = $con.find('.photo-bigImage');
        
        do360Destroy($con);
        $bigImageDiv.show();
        $bigImageDiv.html("<img src=" + imageUrl.AsUrl() + ">");

        $this.addClass('photo-li-selected').siblings().removeClass('photo-li-selected');
    }

    function getValue($this) {
        
        var data = null;
        var $valueInput = $this.find('.photo-list ul li input.valueinput');
        if ($valueInput.length>0) {
            data = [];
            var sort = 0;
            _.each($valueInput, function (item) {
                data.push({
                    ImageUrl: $(item).val(),
                    Sort: sort
                });
                sort++;
            });
        }

        return data;
    }

    function do360View($btn, imgWidth, imgHeight) {
      
        var $this = $($btn).closest('.photoAlbumFild_container');
        
        var images = getValue($this);
        if (images.length > 0) {

            var $bigImageCon = $this.find('.photo-bigImage-container');
            $bigImageCon.find('.photo-bigImage').hide();

            do360Destroy($this);

            $bigImageCon.append("<div class='spritespin360' style='margin: auto;'></div>");

            for (var i = 0; i < images.length; i++) {
                images[i] = images[i].ImageUrl.AsUrl() + "?width=" + imgWidth + "&height=" + imgHeight;
            }
            $(".spritespin360", $bigImageCon).spritespin({
                width: imgWidth,
                height: imgHeight,
                frames: images.length,
                sense: 1,
                behavior: "drag",
                loop: true,
                source: images
            });
        } 
    }
    function do360Stop($btn) {
        var $this = $($btn).closest('.photoAlbumFild_container');
        var $bigImageCon = $this.find('.photo-bigImage-container');
        $(".spritespin360", $bigImageCon).spritespin("animate", false);
        $(".spritespin360", $bigImageCon).spritespin("frame", 0);
    }

    function do360Destroy($con) {
        $(".spritespin360", $con).spritespin("destroy");
        $(".spritespin360", $con).remove();
    }
})(window.jQuery);
