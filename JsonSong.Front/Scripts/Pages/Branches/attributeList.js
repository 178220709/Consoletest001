$(function () {
    var widget_header = $(".widget-header"),
     grid_selector = $("#grid-table"),
     pager_selector = "#grid-pager",
      searchForm = $("#searchForm");

    var gridUrl = "/Branches/AttributeList";
    grid_selector.jqGrid({
        url: gridUrl,
        height: 350,
        grouping: true,
        groupingView: {
            groupField: ['RowId'],
            groupOrder: ['desc'],
            groupColumnShow: false
        },
        colNames: ['Id', jqGridUtil.actionsColumn.name, 'RowId', "语言", '名称', "排序", "类型", "数组", "必填", "置顶","描述","选项"],
        colModel: [
            { name: 'Id', index: 'Id', hidden: true, key: true },
            jqGridUtil.actionsColumn.model,
             jqGridUtil.rowIdGroupDoCopyColumn,

            { name: 'LanguageName', index: 'LanguageName', width: 40, sortable: false },
            {
                name: 'Name', index: 'Name', width: 70, editable: true, edittype: 'textarea', editrules: {
                    required: $$validations.Branches.Rules.Name.required
                },
                editoptions: { maxlength: $$validations.Attribute.Rules.Name.maxlength }
            },             
            {
                name: 'Sort', index: 'Sort', width: 30, editable: true, editrules: {
                    required: $$validations.Attribute.Rules.Sort.required,
                    integer: $$validations.Attribute.Rules.Sort.digits,
                    minValue: $$validations.Attribute.Rules.Sort.min,
                    maxValue: $$validations.Attribute.Rules.Sort.max
                }
            },
            { name: 'TypeName', index: 'TypeName', width: 80, fixed: true, resize: false, sortable: false },
            {
                name: 'IsArray', index: 'IsArray', width: 80, fixed: true, sortable: false, resize: false, editoptions: { value: 'true:是;false:否' }, formatter: 'select'
            },
             {
                 name: 'IsRequired', index: 'IsRequired', width: 80, fixed: true, sortable: false, resize: false, editoptions: { value: 'true:是;false:否' }, formatter: 'select'
             },
             {
                 name: 'IsPushed', index: 'IsPushed', width: 80, fixed: true, sortable: false, resize: false, editoptions: { value: 'true:是;false:否' }, formatter: 'select'
             },
             {
                 name: 'Description', index: 'Description', width: 100, title: false, sortable: false
             },
             {
                 name: 'Options', index: 'Options', width: 100, title: false, sortable: false
             }
        ],
        pager: pager_selector,
        myOptions: {
            editUrl: "/Branches/AttributeGridEdit",
            deleteUrl: "/Branches/AttributeDelete"
        }
    });
    
    // 组删除
    $(".rowDelete", grid_selector).die();
    $(".rowDelete", grid_selector).live("click", function () {
        var rowId = $(this).attr("data-rowId");
        util.ask( "删除", "您确定要删除该组属性吗？<br/>删除将会把其他语言的同组属性一并删除", function () {
           
            $.ajax({
                url: "/Branches/DeleteAttributeByRowId",
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
                        util.err( "删除失败", data.Errors);
                    }
                }
            });

        });
    });

    $(".btnRefresh", widget_header).click(function () {

        jqGridUtil.doRefresh(grid_selector);
    });
    $('.btnAdd', widget_header).click(function () {
        loadContentByVName('BranchesAttributeAddEdit');
    });

    $('.btnEdit', widget_header).click(function () {

        var selRowId = grid_selector.jqGrid('getGridParam', 'selrow');
        var rowId = grid_selector.jqGrid('getRowData', selRowId).RowId;

        loadContentByVName('BranchesAttributeAddEdit', { data: { RowId: rowId } });
    });

    $("#searchForm_search", searchForm).click(function () {
        doSearch();
    });
    $("#searchForm_reset", searchForm).click(function () {
        searchForm[0].reset();
        doSearch();
    });

    function doSearch() {
        grid_selector.jqGrid('setGridParam', {
            search: true,
            datatype: 'json',
            postData: {
                name: $("#name", searchForm).val(),
                languageId: $("#Language", searchForm).val()
            },
            page: 1
        }).trigger("reloadGrid");
    }
    
});
