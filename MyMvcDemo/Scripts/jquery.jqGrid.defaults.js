var jqGridUtil = (function () {
    var o = {};

    o.resize = function() {

        // 延迟处理 不然IE8 会卡死
        setTimeout(function() {
            if ($(".ui-jqgrid").length > 0) {

                $(".ui-jqgrid").each(function() {

                    var $this = $(this);
                    var width = $this.parents().first().width();

                    var gid = $this.attr('id').replace("gbox_", "");
                    var $grid = $("#" + gid);

                    if (width > 0) {
                        var autowidth = $grid.jqGrid().getGridParam("autowidth");
                        if (autowidth) {
                            $grid.jqGrid().setGridWidth(width - 1);
                            $this.find(".ui-jqgrid-bdiv").css("width", width);

                        }
                    } else {
                        //console.log("cc");
                        //$this.find(".ui-jqgrid-bdiv").css("width", "100%");
                        //$this.find(".ui-jqgrid-hdiv").css("width", "100%");
                    }
                });
            }
        }, 200);
    }
    // 设置工具栏按钮状态    
    o.setToolbarStatus = function (me) {
        var toolbar = $(me).closest(".widget-box").find("#my_jqGrid_toolbar");

        if (!_.isEmpty(toolbar)) {

            var ids = $(me).jqGrid('getGridParam', 'selarrrow');
            var selId = $(me).jqGrid('getGridParam', 'selrow');

            if (ids.length > 1) {
                $('.btnDel', toolbar).removeAttr("disabled");
                $('.btnEdit', toolbar).attr("disabled", "disabled");

            } else if (ids.length == 1 || !_.isEmpty(selId)) {
                $('.btnDel', toolbar).removeAttr("disabled");
                $('.btnEdit', toolbar).removeAttr("disabled");

            } else {
                $('.btnEdit', toolbar).attr("disabled", "disabled");
                $('.btnDel', toolbar).attr("disabled", "disabled");
            }

        }
    }

    o.resetToolbarStatus = function (me) {
        var toolbar = $("#my_jqGrid_toolbar");

        if (!_.isEmpty(toolbar)) {

            $('.btnEdit', toolbar).attr("disabled", "disabled");
            $('.btnDel', toolbar).attr("disabled", "disabled");
        }
    }

    // 翻页控件更新
    o.updatePagerIcons = function (me) {
        var replacement =
        {
            'ui-icon-seek-first': 'icon-double-angle-left bigger-140',
            'ui-icon-seek-prev': 'icon-angle-left bigger-140',
            'ui-icon-seek-next': 'icon-angle-right bigger-140',
            'ui-icon-seek-end': 'icon-double-angle-right bigger-140'
        };
        $('.ui-pg-table:not(.navtable) > tbody > tr > .ui-pg-button > .ui-icon').each(function () {
            var icon = $(this);
            var $class = $.trim(icon.attr('class').replace('ui-icon', ''));

            if ($class in replacement) icon.attr('class', 'ui-icon ' + replacement[$class]);
        })
    }
    // 行内编辑及删除 #UserPermissionIsCanEdit 为ToolBar中的隐藏控件

    o.doRefresh = function (grid) {
        $(grid).jqGrid().trigger("reloadGrid");
    }

    // 调用此方法必须配置myOptions.deleteUrl 服务器端接收的参数 单选为id,多选为ids
    o.doDelete = function(grid, options, message, callback) {
        var grid = $(grid);
        var myOptions = grid.jqGrid().getGridParam("myOptions");

        if (_.isEmpty(myOptions) || _.isEmpty(myOptions.deleteUrl)) {
            console.log("未设置删除URL");
        }
        if (_.isEmpty(message)) {
            message = "您确定要删除选中的记录吗？";
        }

        if (_.isEmpty(options)) { // 服务端的参数必须是 ids
            var ids = new Array();
            var nodes = grid.jqGrid('getGridParam', 'selarrrow'); // 多选
            if (nodes.length > 0) {

                _.each(nodes, function(item) {
                    ids.push(grid.jqGrid('getRowData', item).Id);
                });

                options = { ids: ids };
            } else {
                var selRowId = grid.jqGrid('getGridParam', 'selrow');
                var id = grid.jqGrid('getRowData', selRowId).Id;
                ids.push(id);
                options = { ids: id };
            }

        }

        if (!_.isEmpty(options)) {
            util.ask("删除", message, function() {
                $.ajax({
                    contentType: "application/json; charset=utf-8",
                    url: myOptions.deleteUrl,
                    type: "post",
                    dataType: "json",
                    data: JSON.stringify(options),
                    success: function(data) {
                        // 返回的json {Success Message Errors}
                        if (data.Success) {
                            var msg = "删除成功。";
                            if (!_.isEmpty(data.Message)) {
                                msg = data.Message;
                            }
                            util.note({ title: '成功', text: msg });

                            $(grid).jqGrid().trigger("reloadGrid");
                            if ($.isFunction(callback)) {
                                callback.call(this);
                            }
                        } else {
                            util.err("删除失败", data.Errors);
                        }
                    }
                });

            });
        }
    };

    o.actionsColumn = {
        name: "",
        model: {
            name: 'actions',
            index: 'Id',
            width: 80,
            fixed: true,
            sortable: false,
            resize: false,
            title: false,
            formatter: function(cellvalue, options, rowObject) {

                var html = '<div style="margin-left:8px;" class="my_grid_actions" data-Id=' + rowObject.Id + '>';

                html = html + '<div title="编辑所选记录" style="float:left;cursor:pointer;" class="ui-pg-div ui-inline-edit"  onclick="jqGridUtil.inlineActions(this,\'edit\')" >'
                    + '<span class="ui-icon ui-icon-pencil"></span>'
                    + '</div>'
                    + '<div title="提交" style="float:left;display:none" class="ui-pg-div ui-inline-save"  onclick="jqGridUtil.inlineActions(this,\'save\')"  >'
                    + '<span class="ui-icon ui-icon-disk"></span>'
                    + '</div>'
                    + '<div title="取消" style="float:left;display:none;margin-left:5px;" class="ui-pg-div ui-inline-cancel"  onclick="jqGridUtil.inlineActions(this,\'cancel\')" >'
                    + '<span class="ui-icon ui-icon-cancel"></span>'
                    + '</div>';

                html = html + '<div title="删除所选记录" style="float:left;margin-left:5px;" class="ui-pg-div ui-inline-del"  onclick="jqGridUtil.inlineActions(this,\'delete\')" >'
                    + '<span class="ui-icon ui-icon-trash"></span>'
                    + '</div>';

                html = html + '</div>';
                return html;
            }
        }
    };

    o.inlineActions = function (el, act) {

        var $tr = $(el).closest("tr.jqgrow"),
           $td = $(el).closest("td"),
           rid = $tr.attr("id"),
           $gid = $(el).closest("table.ui-jqgrid-btable").attr('id').replace(/_frozen([^_]*)$/, '$1'),
           $grid = $("#" + $gid),
           Id = $(el).closest(".my_grid_actions").attr("data-Id");

        var myOptions = $grid.jqGrid().getGridParam("myOptions")
        switch (act) {
            case 'edit':
                $grid.editRow(rid, true);
                $td.find(".ui-inline-edit").hide();
                $td.find(".ui-inline-save").show();
                $td.find(".ui-inline-cancel").show();
                $td.find(".ui-inline-del").hide();

                break;
            case 'save':

                if (_.isEmpty(myOptions) || _.isEmpty(myOptions.editUrl)) {
                    console.log("未设置编辑URL");
                }

                $grid.jqGrid('saveRow', rid,
                {
                    successfunc: function (response) {

                        $td.find(".ui-inline-save").hide();
                        $td.find(".ui-inline-edit").show();
                        $td.find(".ui-inline-cancel").hide();
                        $td.find(".ui-inline-del").show();

                        if (response.responseJSON.Success) {
                            util.note({ title: '成功', text: '保存成功。' });

                            $grid.jqGrid().resetSelection();
                            $grid.jqGrid().trigger("reloadGrid");
                            return true;
                        } else {
                            util.err("保存失败", response.responseJSON.Errors);
                            $grid.restoreRow(rid);
                            return false;
                        }

                    },
                    errorfunc: function (response, result) {

                        $td.find(".ui-inline-save").hide();
                        $td.find(".ui-inline-edit").show();
                        $td.find(".ui-inline-cancel").hide();
                        $td.find(".ui-inline-del").show();
                        // 登录超时还需判断 
                        util.err("保存失败", "服务器保存失败");
                        $grid.restoreRow(rid);
                    },
                    url: myOptions.editUrl,
                    extraparam: { id: Id }
                });
                break;
            case 'cancel':
                $td.find(".ui-inline-cancel").hide();
                $td.find(".ui-inline-edit").show();
                $td.find(".ui-inline-save").hide();
                $td.find(".ui-inline-del").show();
                $grid.restoreRow(rid);
                break;
            case 'delete':

                this.doDelete($grid, { ids: Id });
                break;
        }
    }

    o.GetTreeGridDoCopyHtml = function (value) {
        return '<div class="rowIdCopy" ><input type="text" readonly="readonly"  role="textbox" style="width: 145px;" value="' + value + '" ><div class="btn-copy ui-pg-div" title="复制标识符" ><span class=" icon-copy btn-copy-btn purple" data-copyData="' + value + '"></span> </div></div>';
    }

    o.rowIdGroupDoCopyColumn = {
        name: 'RowId', index: 'RowId', hidden: true, formatter: function (cellval, opts, rowObject, action) {

            var rows = $(this).getGridParam("allGridDatas").rows;
            if (typeof action == "undefined") { // 分组

                var defLang = util.getDefaultLanguage()
                var item = _.find(rows, function (data) {
                    return data.RowId == cellval && data.LanguageId == defLang.Id;
                });
                if (_.isEmpty(item)) {
                    item = _.find(rows, function (data) {
                        return data.RowId == cellval;
                    });
                }
                var delHtml = '<div title="删除类别组" class="ui-pg-div rowDelete" data-rowId="' + cellval + '"><span class="ui-icon ui-icon-trash"></span></div>';
                var copyhtml = '<div class="rowIdCopy" ><div style="float: left;margin-top: 2px;margin-left: 15px;">标识符：' + cellval + '</div><div class="btn-copy" title="复制标识符" ><span class="icon-copy purple btn-copy-btn" data-copyData="' + cellval + '"></span> </div></div>';
                var html = '<div class="gridGroupDiv"><b class="name overflowEllipsis" >' + item.Name + '</b>' + copyhtml + delHtml + "</div>"
                return html;
            } else {
                return cellval;
            }
        }
    }

    o.rowIdGroupColumn = {
        name: 'RowId', index: 'RowId', hidden: true, formatter: function (cellval, opts, rowObject, action) {

            var rows = $(this).getGridParam("allGridDatas").rows;
            if (typeof action == "undefined") { // 分组

                var defLang = util.getDefaultLanguage()
                var item = _.find(rows, function (data) {
                    return data.RowId == cellval && data.LanguageId == defLang.Id;
                });
                if (_.isEmpty(item)) {
                    item = _.find(rows, function (data) {
                        return data.RowId == cellval;
                    });
                }
                var delHtml = '<div title="删除类别组" class="ui-pg-div rowDelete" data-rowId="' + cellval + '"><span class="ui-icon ui-icon-trash"></span></div>';
                var html = '<div class="gridGroupDiv"><b class="name overflowEllipsis" >' + item.Name + '</b>' + delHtml + "</div>"
                return html;
            } else {
                return cellval;
            }
        }
    }

    return o;
})();
$(function() {
    //==jgrid defaults==//********** 使用jqgrid 应调用ToolBar PartialView
    $.extend($.jgrid.defaults, {
        mtype: "post",
        datatype: "json",
        height: 350,
        multiSort: true,
        ajaxGridOptions: {
            contentType: "application/json; charset=utf-8"
        },
        ajaxSelectOptions: {
            type: "post",
            dataType: "json"
        },
        prmNames: {
            page: "pageNumber", // 表示请求页码的参数名称  
            rows: "iDisplayLength", // 表示请求行数的参数名称  
            sort: "sort", // 表示用于排序的列名的参数名称  sidx
            order: "dir", // 表示采用的排序方式的参数名称  
            search: "_search", // 表示是否是搜索请求的参数名称  
            nd: "nd", // 表示已经发送请求的次数的参数名称  
            id: "id", // 表示当在编辑数据模块中发送数据时，使用的id的名称
            editoper: "edit", // 当在edit模式中提交数据时，操作的名称  
            addoper: "add", // 当在add模式中提交数据时，操作的名称  
            deloper: "delete", // 当在delete模式中提交数据时，操作的名称  
            subgridid: "id", // 当点击以载入数据到子表时，传递的数据名称  
            npage: null,
            totalrows: "totalrows" // 表示需从Server得到总共多少行数据的参数名称，参见jqGrid选项中的rowTotal 
        },
        viewrecords: true,
        rowNum: 20,
        rowList: [20, 40, 60],
        altRows: true,
        multiselect: true,
        multiboxonly: true,
        autowidth: true,
        loadui: "disable",
        allGridDatas: "ccccc", // 自定义用于保存列表的request 回来的值
        beforeRequest: function() {

            // loadui ==disable 调用自定义的loading 
            if ($(this).jqGrid().getGridParam("loadui") == "disable") {
                var table = $(this).closest(".ui-jqgrid-bdiv");
                util.block(table);
            }

        },
        loadComplete: function(data) {

            var me = this;
            var veiw = $(me).closest(".ui-jqgrid-bdiv");
            var jqgrid = $(me).jqGrid();
            if ($(this).jqGrid().getGridParam("loadui") == "disable") {
                util.unblock(veiw);
            }
            jqgrid.resetSelection();
            setTimeout(function() {

                jqGridUtil.resetToolbarStatus(me);
                jqGridUtil.updatePagerIcons(); // 更新翻页控件
                var aa = $(".ui-jqgrid-bdiv");
                $(".jqgrid_empty", veiw).remove();
                if (data.records < 1) {
                    var emptyhtml = "<div class='jqgrid_empty text-muted bolder'>暂无数据</div>";
                    veiw.append(emptyhtml);
                }
            }, 10);
            // 自定义完成函数调用 
            var myOptions = jqgrid.getGridParam("myOptions");

            if (!_.isEmpty(myOptions) && $.isFunction(myOptions.loadCompleteFun)) {

                myOptions.loadCompleteFun.apply(this, [jqgrid, data]);
            }

            // 列表内 有复制操作
            if ($(".rowIdCopy", jqgrid).length > 0) {
                doCopyUtil.initDoCopy($(".rowIdCopy .btn-copy-btn", jqgrid), function(me) {

                    return $(me).attr("data-copyData");
                });
            }

        },
        onCellSelect: function(rowid, iCol, cellcontent, e) {
            // 当是树 及单元格编辑时 才需要处理 由于在这种情况下 选择不了行
            if ($(this).getGridParam("treeGrid") && $(this).getGridParam("cellEdit")) {
                $(this).setSelection(rowid);
                jqGridUtil.setToolbarStatus(this);
            }
        },
        onSelectRow: function(rowid, status) {
            jqGridUtil.setToolbarStatus(this);
        },
        onSelectAll: function(rowids, status) {
            jqGridUtil.setToolbarStatus(this);
        },
        serializeGridData: function(postData) {
            //处理排序POST参数 原：sort: "Name asc, Code " dir: "asc"
            if (!_.isEmpty(postData.sort)) {
                var sortings = [];
                var sortlist = postData.sort.split(",");

                _.each(sortlist, function(i) {
                    var sorts = i.split(" ");
                    var sort, dir = "";

                    _.each(sorts, function(c) {

                        if (!_.isEmpty(c) && _.isEmpty(sort)) {
                            sort = c
                        } else if (!_.isEmpty(sort)) {
                            dir = c
                        }
                    });
                    if (_.isEmpty(dir)) {
                        dir = postData.dir;
                    }

                    sortings.push({ sort: sort, dir: dir });
                });

                postData.sortings = sortings;
            }

            return JSON.stringify(postData);
        },
        beforeProcessing: function(data) {
            // 用参数allGridData 保存请求 返回的数据 如 {records: 4, rows: ｛行数据｝, total: 1}
            $(this).setGridParam({ allGridDatas: data });

        }
    });

    // 设置列表宽度
    $(window).off("resize");
    $(window).on("resize", function() {
        jqGridUtil.resize();
    });
});
