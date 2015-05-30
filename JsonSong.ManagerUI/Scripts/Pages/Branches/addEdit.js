$(function() {
    var branchesForm = $("#BranchesAddEdit");
    var branchesAttributePanel = new AttributePanel(branchesForm, {
        attributeAddVName: "BranchesAttributeAddEdit",
        entityType: "Branches"
    });

    var IsEdit = $("#RowId").val() != "";
    var gc = new BMap.Geocoder(); //百度地理位置查询类

    //根据访问者ip 获取当前城市的信息 
    var myCity = new BMap.LocalCity();
    myCity.get(initIpPoint);
    var ipPoint; //当前访问者ip的大致点位
    var maps = new Array(); //存储不同langForm里面的map的实例,避免重复初始化map

    function initIpPoint(result) {
        ipPoint = new BMap.Point(result.center.lng, result.center.lat); //取得当前ip的坐标
        initAddress(ipPoint);

        var actLi = $("#lang-nav-tabs li[class=active]");
        InitMapByLi(actLi);
    }

    function initAddress(point) {
        try {
            gc.getLocation(point, function(rs) {
                var addComp = rs.addressComponents; //获得当前ip坐标的详细地理位置信息
                var provinceSelectStr = _.template(".ProvinceClass option:contains('<%=name%>')", { name: addComp.province });
                var citySelectStr = _.template(".CityClass option:contains('<%=name%>')", { name: addComp.city });
                $(provinceSelectStr).attr("selected", true);
                $(".ProvinceClass").each(function() {
                    initCitySection(this);
                });
                $(citySelectStr).attr("selected", true);
                //alert(addComp.province + ", " + addComp.city + ", " + addComp.district + ", " + addComp.street + ", " + addComp.streetNumber);
            });
        } catch(e) {
            console.log(e);
        }
    }

    function initOneAddress(div, point) {
        try {
            gc.getLocation(point, function(rs) {
                var addComp = rs.addressComponents; //获得当前ip坐标的详细地理位置信息
                var provinceSelectStr = _.template(".ProvinceClass option:contains('<%=name%>')", { name: addComp.province });
                var citySelectStr = _.template(".CityClass option:contains('<%=name%>')", { name: addComp.city });
                $(provinceSelectStr).attr("selected", true);
                initCitySection($(div).find(".ProvinceClass"));
                $(citySelectStr).attr("selected", true);
            });
        } catch(e) {
            console.log(e);
        }
    }

    function InitMapByLi(li) {
        var langId = $(li).attr("data-langid");
        var selectStr = _.template("form[data-langid=<%=langId%>]", { langId: langId });
        var activeForm = $(selectStr);
        InitMap(ipPoint, langId, activeForm);
    }

    $(".tabAction").click(function(parameters) {
        InitMapByLi($(this).closest("li"));
    });

    function InitMap(initPoint, langId, activeForm) {
        if (maps[langId]) {
            return;
        }
        var mapContainer = activeForm.find(".BaiduMapDiv")[0];
        var map = new BMap.Map(mapContainer); // 创建地图实例
        maps[langId] = map;
        var marker;
        if (!IsEdit) {
            map.centerAndZoom(initPoint, 15); // 初始化地图，如果获取了当前访问者的ip并成功定位 则直接定位到此城市
            marker = new BMap.Marker(initPoint); // 创建标注 使用当前地图中心点
        } else {
            var point = checkInitPoint(activeForm);
            map.centerAndZoom(point, 15); // 初始化地图，如果获取了当前访问者的ip并成功定位 则直接定位到此城市
            marker = new BMap.Marker(point); // 创建标注 使用当前地图中心点
        }
        map.enableScrollWheelZoom();
        map.addControl(new BMap.NavigationControl()); //添加默认缩放平移控件
        map.addControl(new BMap.ScaleControl({ anchor: BMAP_ANCHOR_BOTTOM_LEFT })); // 添加默认比例尺控件 左下

        marker.enableDragging();
        map.addOverlay(marker); // 将标注添加到地图中
        //创建信息窗口
        var infoWindow = new BMap.InfoWindow("点位信息");
        marker.addEventListener("click", function() {
            ShowPopInfo(activeForm, this, infoWindow);
        });

        marker.addEventListener("dragend", function(e) {
            ShowPoPInfo(activeForm, e.point);
        });

        activeForm.find(".btnSearchMap").click(function(parameters) {
            var province = activeForm.find(".ProvinceClass").find("option:selected").text();
            var city = activeForm.find(".CityClass").find("option:selected").text();
            var searchStr = province + city + activeForm.find("input[name=AddressKey]").val();
            gc.getPoint(searchStr, function(spoint) {
                if (spoint) {
                    map.centerAndZoom(spoint, 15);
                    marker.setPosition(spoint);
                }
                ShowPoPInfo(activeForm, spoint);
            }, province);
        });
        //  });
    }

    ;

    function ShowPopInfo(activeForm, obj, infoWindow) {
        var name = $(activeForm).find("input[name=Name]").val();
        var address = $(activeForm).find("input[name=Address]").val();
        if (address == "") {
            address = "拖动标记点以定位门店地址";
        }
        infoWindow.setTitle(name);
        infoWindow.setContent(address);
        obj.openInfoWindow(infoWindow);
    }

    function ShowPoPInfo(activeForm, pt) {
        gc.getLocation(pt, function(rs) {
            activeForm.find("input[name=Lng]").val(pt.lng);
            activeForm.find("input[name=Lat]").val(pt.lat);
            activeForm.find("input[name=Address]").val(rs.address);
        });
        initOneAddress(activeForm, pt);
    }

    //编辑状态的时候 根据已有坐标信息生成点位并返回

    function checkInitPoint(div) {
        var lng = $(div).find("input[name=Lng]").val();
        var lat = $(div).find("input[name=Lat]").val();
        var point = new BMap.Point(lng, lat);
        if (point && point.lng) {
            return point;
        } else {
            return new BMap.Point(114.025974, 22.546054);
        }
    }

    ;

    //根据省下拉选择器 初始化城市
    function initCitySection(provinceSection) {
        var addOption = function(cmb, str, val) {
            var option = document.createElement("OPTION");
            cmb.options.add(option);
            option.innerHTML = str;
            option.value = val;
            option.obj = val;
        };

        var ddlCity = $(provinceSection).closest(".tab-pane-lang").find(".CityClass");
        var id = $(provinceSection).val();
        var attributes = $(provinceSection).find("option:selected").attr("attributes");
        $.ajax({
            contentType: "application/json; charset=utf-8",
            async: false,
            url: "/Region/GetChildData",
            type: "post",
            data: JSON.stringify({ id: id, attributes: attributes }),
            success: function(result) {
                ddlCity[0].length = 0;
                $.each(result, function(parameters) {
                    addOption(ddlCity[0], this.text, this.id);
                });
            },
        });
        $(provinceSection).closest(".tab-pane-lang").find("input[name=AddressKey]").val("");
    };


    //省市联动
    $(".ProvinceClass").change(function() {
        initCitySection(this);
        $(this).closest(".tab-pane-lang").find("input[name=AddressKey]").val("");
    });
    $(".CityClass").change(function() {

        $(this).closest(".tab-pane-lang").find("input[name=AddressKey]").val("");
    });


    // 相册
    $(".branches-photos-container", branchesForm).photoAlbumField();

    $('textarea.editor', branchesForm).ckeditor();
    CKEDITOR.on('instanceReady', function() {
        $("textarea.editor").cke_setHeight(400);
    });

    var errorPlacement = function(error, element) {
        var errText = $(error).html(),
            formControl = $(element).closest(".form-group"),
            err = $($.format('<label class="control-label errors">{0}</label>', errText));

        formControl.removeClass("has-success").addClass("has-error");
        $("label.errors", formControl).remove();
        $(".control_label_container", formControl).append(err);

    };

    var unhighlight = function(element, errorClass, validClass) {

        var formControl = $(element).closest(".form-group");
        formControl.removeClass("has-error").addClass("has-success");
        $("label.errors", formControl).remove();
    };

    // 给表单加验证
    $('#langForms form').each(function() {
        $(this).validate(
            {
                rules: $$validations.Branches.Rules,
                messages: $$validations.Branches.Messages,
                ignore: "",
                errorPlacement: errorPlacement,
                unhighlight: unhighlight
            });
    });

    // 提交
    $("#btnSave", branchesForm).click(function() {
        var forms = $("#langForms form", branchesForm);
        var tabsLi = $("#lang-nav-tabs li", branchesForm);
        var tabs = $("#lang-nav-tabs", branchesForm);

        var allValid = true;
        for (var i = 0; i < forms.length; i++) {
            if (!tabsLi.eq(i).hasClass("disabled")) { // 不验证语言不可用的表单
                var form = forms[i];
                var isvalid = $(form).valid();
                var vttIsValid = branchesAttributePanel.validate(form); // 扩展属性验证

                var langId = $(form).attr("data-langId");
                if (!isvalid || !vttIsValid) {
                    tabs.find('li[data-langId="' + langId + '"]').addClass("errors");
                    allValid = false;
                } else {
                    tabs.find('li[data-langId="' + langId + '"]').removeClass("errors");
                }
            }
        }

        // 验证通过 获取表单数据 提交表单        
        if (allValid) {

            var datas = getFormsData();
            if (datas.length <= 0) {
                util.err("提示", "没有开启任何语言，不需要提交。");
                return;
            }

            var data = {
                RowId: $("#RowId", branchesForm).val(),
                models: datas
            };

            $.ajax({
                contentType: "application/json; charset=utf-8",
                url: "/Branches/AddEdit",
                type: "post",
                dataType: "json",
                data: JSON.stringify(data),
                beforeSend: function() {
                    util.block(branchesForm);
                },
                success: function(responseText) {
                    util.unblock(branchesForm);

                    if (responseText.Success) {

                        util.success("成功", "保存成功", function() {
                            doContentRefresh();
                        }, function() {
                            loadContentByVName("BranchesList");
                        });

                    } else {
                        util.err("错误", responseText.Errors);
                        var errorTab = tabs.find('li[data-langId="' + responseText.dto.LanguageId + '"]');
                        errorTab.addClass("errors").siblings().removeClass("errors");
                        errorTab.find(".tabAction").click();
                    }
                }
            });
        }

    });

    // 列表
    $("#btnList", branchesForm).click(function() {
        loadContentByVName("BranchesList");
    });
    // 重置
    $("#btnReset", branchesForm).click(function() {
        doContentRefresh();
    });




    var getFormsData = function() {
        var panels = $("#langForms .tab-pane-lang");
        var tabsLi = $("#lang-nav-tabs li");

        var data = [];

        for (var i = 0; i < panels.length; i++) {
            var panel = panels.eq(i);

            // 判断当前语言是否可以 不可以 不提交
            if (!tabsLi.eq(i).hasClass("disabled")) {
                var langId = panel.find("form").attr("data-langId");
                // 属性
                var attributeValues = branchesAttributePanel.getValue(panel);
                data.push({
                    Id: panel.find('input[name="Id"]').val(),
                    LanguageId: langId,
                    Name: panel.find('input[name="Name"]').val(),
                    Address: panel.find('input[name="Address"]').val(),
                    Sort: panel.find('input[name="Sort"]').val(),
                    Telephone: panel.find('input[name="Telephone"]').val(),
                    Province: panel.find('.ProvinceClass').find("option:selected").text(),
                    City: panel.find('.CityClass').find("option:selected").text(),
                    Lat: panel.find('input[name="Lat"]').val(),
                    Lng: panel.find('input[name="Lng"]').val(),

                    PageTitle: panel.find('textarea[name="PageTitle"]').val(),
                    MetaKeywords: panel.find('textarea[name="MetaKeywords"]').val(),
                    MetaDescription: panel.find('textarea[name="MetaDescription"]').val(),
                    Description: panel.find('textarea[name="Description"]').val(),
                    Images: panel.find('.branches-photos-container').photoAlbumField('getValue'),
                    AttributeValues: attributeValues
                });
            }
        }
        return data;
    };
});