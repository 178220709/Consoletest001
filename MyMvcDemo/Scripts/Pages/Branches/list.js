$(function () {
    var widget_header = $(".widget-header"),
     grid_selector = $("#grid-table"),
     pager_selector = "#grid-pager",
     searchForm = $("#searchForm"),
     defLang = util.getDefaultLanguage();

    var languageSelect = $("#Language", searchForm);

    var groupData = [];
    var gridUrl = "/Branches/List";
   
    grid_selector.jqGrid({
        url: gridUrl,
        height: 350,
        grouping: true,
        sortname: "CreatedOn",
        sortorder: "desc",
        groupingView: {
            groupField: ['RowId'],
            groupOrder: ['desc'],
            groupColumnShow: false
        },
        colNames: ['Id', jqGridUtil.actionsColumn.name, 'RowId', "语言", '名称', "所在城市", '门店位置', "电话", "排序"],
        colModel: [
            { name: 'Id', index: 'Id', hidden: true, key: true },
            jqGridUtil.actionsColumn.model,
            jqGridUtil.rowIdGroupDoCopyColumn,

            { name: 'LanguageName', index: 'LanguageName', width: 30, sortable: false },
            {
                name: 'Name', index: 'Name', width: 30, editable: true, edittype: 'textarea', editrules: {
                    required: $$validations.Branches.Rules.Name.required
                },
                editoptions: { maxlength: $$validations.Branches.Rules.Name.maxlength }
            },
             {
                 name: 'ProvinceCity', index: 'ProvinceCity', width: 40
             },
             {
                 name: 'Address', index: 'Address', width: 40
             },
            {
                name: 'Telephone', index: 'Telephone', width: 30, editable: true, editrules: {
                    required: $$validations.Branches.Rules.Sort.required,
                   
                }
            },
            {
                name: 'Sort', index: 'Sort', width: 30, editable: true, editrules: {
                    required: CommonValidation.Rules.Sort.required,
                    integer: CommonValidation.Rules.Sort.digits,
                    minValue: CommonValidation.Rules.Sort.min,
                    maxValue: CommonValidation.Rules.Sort.max
                }
            }
        ],
        pager: pager_selector,
        myOptions: {
            editUrl: "/Branches/GridEdit",
            deleteUrl: "/Branches/delete"
        }
    });

    // 组删除
    $(".rowDelete", grid_selector).die();
    $(".rowDelete", grid_selector).live("click", function () {
        var rowId = $(this).attr("data-rowId");
        util.ask("删除", "您确定要删除该组文章吗？", function () {

            $.ajax({
                url: "/Branches/DeleteByRowId",
                type: "post",
                dataType: "json",
                data: { rowId: rowId },
                success: function (data) {

                    if (data.Success) {
                        var msg = "删除成功。";
                        if (!_.isEmpty(data.Message)) {
                            msg = data.Message;
                        }
                        util.note({ title: '成功', text: msg });

                        grid_selector.jqGrid().trigger("reloadGrid");
                    }
                    else {
                        util.err("删除失败", data.Errors);
                    }
                }
            });

        });
    });

    $(".btnRefresh", widget_header).click(function () {

        jqGridUtil.doRefresh(grid_selector);
    });
    $('.btnAdd', widget_header).click(function () {
        loadContentByVName(null, null, "/Branches/AddEdit");
    });

    $('.btnEdit', widget_header).click(function () {

        var selRowId = grid_selector.jqGrid('getGridParam', 'selrow');
        var rowId = grid_selector.jqGrid('getRowData', selRowId).RowId;

        loadContentByVName(null, { data: { RowId: rowId } }, "/Branches/AddEdit");
    });

    $('.btnDel', widget_header).click(function () {
        jqGridUtil.doDelete(grid_selector, null, "您确定要删除选中的记录吗？");
    });
    $("#grid_search", searchForm).click(function () {
        doSearch();
    });
    $("#searchForm_reset", searchForm).click(function () {
        searchForm[0].reset();
        $("#Language", searchForm).select2('val', '');
        $("#Page", searchForm).select2('val', '');
        doSearch();
    });

    function doSearch() {
        grid_selector.jqGrid('setGridParam', {
            search: true,
            datatype: 'json',
            postData: {
                name: $("#name", searchForm).val(),
                languageId: languageSelect.val(),
            },
            page: 1
        }).trigger("reloadGrid");
    }

    // 从 栏目页点查看文件 时的处理
    var searchLanguage = $("#searchLanguageId").val();
    if (!_.isEmpty(searchLanguage)) {
        languageSelect.val(searchLanguage);
        languageSelect.change();
        //doSearch();
    }
});
