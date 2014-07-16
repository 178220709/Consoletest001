var select2Util = (function () {
    var o = {};
    o.select2Ajax = function (element, url, options) {
        var defaultOptions = {
            placeholder: "--请选择--",
            minimumInputLength: 2,
            ajax: {
                url: url,
                dataType: 'json',
                type: 'POST',
                quietMillis: 100,
                data: function (term, page) {
                    return {
                        name: term, //search term
                        iDisplayLength: 10, // page size
                        pageNumber: page
                    };
                },
                results: function (data, page) {
                    var more = (page * 10) < data.records;
                    return { results: data.rows, more: more };
                }
            },
            formatResult: function (data) {
                return data.Name;
            },
            formatSelection: function (data) {
                return data.Name;
            },
            dropdownCssClass: "bigdrop"
        }
        $.extend(defaultOptions, options);

        $(element).select2(defaultOptions);

    };

    o.productSelect = function (element, langId, moldId, options) {
        var defaultOptions = {
            placeholder: "请输入商品编号/名称",
            minimumInputLength: 2,
            allowClear: true,
            id: function (data) { return data.Id },
            ajax: {
                url: "/Product/ProductList",
                quietMillis: 100,
                dataType: 'json',
                type: 'POST',
                data: function (term, page) {
                    return {
                        name: term,
                        languageId: langId,
                        moldId: moldId,
                        iDisplayLength: 15,
                        pageNumber: page
                    };
                },
                results: function (data, page) {
                    var more = (page * 15) < data.records;
                    return { results: data.rows, more: more };
                }
            },
            formatResult: function (data) {
                var html = '<div class="item">';
                if (_.isEmpty(data.ThumbImageUrl)) {
                    html = html + '<div  class="itemImg">无</div>';
                } else {
                    html = html + '<div class="itemImg"><img src="' + data.ThumbImageUrl.AsUrl() + '?width=40&height=40"></div>';
                }

                html = html + '<div class="itemTitle">' + data.Name + '</div></div>';

                return html;
            },
            formatSelection: function (data) {
                return data.Name;
            },
            dropdownCssClass: "bigdrop",
            initSelection: function (element, callback) {
               
                //var id = $(element).val();
                var id = $(element).select2("val");
                if (!_.isEmpty(id)) {
                    if ($(element).attr("multiple") != null) {

                        $.ajax({
                            contentType: "application/json; charset=utf-8",
                            url: "/Product/GetByIds",
                            type: "post",
                            dataType: "json",
                            data: JSON.stringify(
                                {
                                    ids: id
                                }
                            ),
                            success: function (data) {
                                callback(data);
                            }
                        });
                       
                    } else {

                        $.post("/Product/GetById",
                            {
                                id: id
                            }, function (data) {
                                callback(data);
                            });

                    }
                }
            }
        }
        $.extend(defaultOptions, options);

        $(element).select2(defaultOptions);
    };

    o.productGoodsSelect = function (element, langId, options) {
        var defaultOptions = {
            placeholder: "请输入商品/货品的编号或名称",
            minimumInputLength: 2,
            allowClear: true,
            id: function (data) { return data.Id },
            ajax: {
                url: "/Product/SearchProductGoods",
                quietMillis: 100,
                dataType: 'json',
                type: 'POST',
                data: function (term, page) {
                    return {
                        name: term,
                        iDisplayLength: 15,
                        pageNumber: page,
                        languageId: langId,
                    };
                },
                results: function (data, page) {
                    var more = (page * 15) < data.records;
                    return { results: data.rows, more: more };
                }
            },
            formatResult: function (data) {
                var imageUrl = data.ImageUrl;
               
                var name = data.Name;
                
                var html = '<div class="item">';
                if (_.isEmpty(imageUrl)) {
                    html = html + '<div  class="itemImg">无</div>';
                } else {
                    html = html + '<div class="itemImg"><img src="' + imageUrl.AsUrl() + '?width=40&height=40"></div>';
                }

                html = html + '<div class="itemTitle">' + name + '</div></div>';

                return html;
            },
            formatSelection: function (data) {
                var name = data.Name;
                if (_.isEmpty(name)) {
                    name = data.Product.Name
                }
                return name;
            },
            dropdownCssClass: "bigdrop",
            initSelection: function (element, callback) {
                var id = $(element).select2("val");
                if ($(element).attr("multiple") != null) {
                    if (!_.isEmpty(id)) {
                       
                        $.ajax({
                            contentType: "application/json; charset=utf-8",
                            url: "/Product/GetProductGoodsByIds",
                            type: "post",
                            dataType: "json",
                            data: JSON.stringify(
                                {
                                    ids: id
                                }
                            ),
                            success: function (data) {
                                callback(data);
                            }
                        });
                    }
                } else {
                    if (!_.isEmpty(id) || id > 0) {
                        $.post("/Product/GetProductGoodsById",
                            {
                                id: id
                            }, function (data) {
                                callback(data);
                            });
                    }
                }
            }
        }
        $.extend(defaultOptions, options);

        $(element).select2(defaultOptions);
    };

    o.userSelect = function (element, langId, options) {
        var defaultOptions = {
            placeholder: "请输入用户名",
            minimumInputLength: 2,
            allowClear: true,
            id: function (data) { return data.Id },
            ajax: {
                url: "/User/List",
                quietMillis: 100,
                dataType: 'json',
                type: 'POST',
                data: function (term, page) {
                    return {
                        name: term,
                        iDisplayLength: 15,
                        languageId: langId,
                        pageNumber: page
                    };
                },
                results: function (data, page) {
                    var more = (page * 15) < data.records;
                    return { results: data.rows, more: more };
                }
            },
            formatResult: function (data) {
                return data.Name;
            },
            formatSelection: function (data) {
                return data.Name;
            },
            dropdownCssClass: "bigdrop",
            initSelection: function (element, callback) {
                var id = $(element).val();
                if (!_.isEmpty(id) && id > 0) {
                    $.post("/User/GetById",
                        {
                            id: id
                        }, function (data) {
                            callback(data);
                        });
                }
            }
        }
        $.extend(defaultOptions, options);

        $(element).select2(defaultOptions);
    };

    o.provinceCityAndCityAreaSelect = function (pSelEl, cSelEl, caSelEl) {

        var $pselect = $(pSelEl);
        var $citySelect = $(cSelEl);
        var $cityAreaSelect = $(caSelEl);

        $pselect.select2('destroy');
        $.post("/Region/GetAllProvince", null, function (data) {
            $pselect.empty();
            doAddOptions($pselect, data, '请选择省份');

            var value = $pselect.attr("data-value");
            if (!_.isEmpty(value)) {
                $pselect.select2("val", value)
                $pselect.select2().change();
                $pselect.removeAttr("data-value");
            }
        });

        $pselect.select2().change(function (e) {

            var $option = $pselect.find('option:selected');
            var type = $option.attr("type");
            var id = $option.attr("value");

            $cityAreaSelect.select2('destroy');
            $cityAreaSelect.empty();
            $citySelect.select2('destroy');
            $citySelect.empty();

            if (type == "p") { // 省份
                doLoadCity($citySelect, $cityAreaSelect, id, type);
                
            } else if (type == "pz") {// 直辖市

                $citySelect.attr("disabled", "disabled");
                doLoadCityArea($cityAreaSelect, id, type);
            }

        });

        function doLoadCity($citySelect, $cityAreaSelect, id, type) {
            $citySelect.select2('destroy');
            $citySelect.empty();

            $.post("/Region/GetChildData", {
                id: id,
                attributes: type
            }, function (data) {

                doAddOptions($citySelect, data, '请选择城市');
                $citySelect.removeAttr("disabled");

                // 初始化时 如果有城市Id 则初始 区域
                var value = $citySelect.attr("data-value");
                if (!_.isEmpty(value)) {
                 
                    var $option = $citySelect.find('option:selected');
                    var ctype = $option.attr("type");
                   
                    $citySelect.select2("val", value)
                    doLoadCityArea($cityAreaSelect, value, ctype);                    
                    $citySelect.removeAttr("data-value");
                }

                $citySelect.select2().change(function () {
                    var $option = $citySelect.find('option:selected');

                    var ctype = $option.attr("type");
                    var id = $option.attr("value");
                    if (!_.isEmpty(id)) {
                        doLoadCityArea($cityAreaSelect, id, ctype)
                    }
                });
            });
        }

        function doLoadCityArea($cityAreaSelect, id, type) {

            $cityAreaSelect.select2('destroy');
            $cityAreaSelect.empty();

            $.post("/Region/GetChildData", {
                id: id,
                attributes: type
            }, function (data) {

                doAddOptions($cityAreaSelect, data, '请选择市区');
                $cityAreaSelect.select2();

                var value = $cityAreaSelect.attr("data-value");
                if (!_.isEmpty(value)) {
                    $cityAreaSelect.select2("val", value)
                    $cityAreaSelect.removeAttr("data-value");
                    $cityAreaSelect.removeAttr("data-value");
                }
            });

        }

        function doAddOptions($select, data,text) {
            $select.append('<option value="">--' + text + '--</option>');

            var oEl = '<option value="<%=id%>" type="<%=attributes%>"><%=text%></option>';
            var oElSelected = '<option value="<%=id%>" type="<%=attributes%>" selected="selected"><%=text%></option>';
            var value = $select.attr("data-value");
            _.each(data, function (item) {

                if (!_.isEmpty(value) && item.id == value) {
                    $select.append(_.template(oElSelected, item));
                } else {
                    $select.append(_.template(oEl, item));
                }
            });
        }
    };


    return o;
})();