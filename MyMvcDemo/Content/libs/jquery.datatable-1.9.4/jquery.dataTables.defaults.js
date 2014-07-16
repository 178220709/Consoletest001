$(function () {
    $.fn.dataTableExt.oPagination.input = {
        "fnInit": function (oSettings, nPaging, fnCallbackDraw) {
            var divFirst = document.createElement('div');
            var divLast = document.createElement('div');
            var nFirst = document.createElement('button');
            var nPrevious = document.createElement('button');
            var nNext = document.createElement('button');
            var nLast = document.createElement('button');
            var nInput = document.createElement('input');
            var nPage = document.createElement('span');
            var nOf = document.createElement('span');

            nFirst.innerHTML = oSettings.oLanguage.oPaginate.sFirst;
            nPrevious.innerHTML = oSettings.oLanguage.oPaginate.sPrevious;
            nNext.innerHTML = oSettings.oLanguage.oPaginate.sNext;
            nLast.innerHTML = oSettings.oLanguage.oPaginate.sLast;

            divFirst.className = "btn-group firstGroup";
            divLast.className = "btn-group lastGroup";
            nFirst.className = "paginate_button first btn";
            nPrevious.className = "paginate_button previous btn";
            nNext.className = "paginate_button next btn";
            nLast.className = "paginate_button last btn";
            nOf.className = "paginate_of";
            nPage.className = "paginate_page";

            if (oSettings.sTableId !== '') {
                nPaging.setAttribute('id', oSettings.sTableId + '_paginate');
                nPrevious.setAttribute('id', oSettings.sTableId + '_previous');
                nPrevious.setAttribute('id', oSettings.sTableId + '_previous');
                nNext.setAttribute('id', oSettings.sTableId + '_next');
                nLast.setAttribute('id', oSettings.sTableId + '_last');
            }

            nInput.type = "text";
            nInput.style.width = "30px";
            nInput.style.display = "inline";
            nPage.innerHTML = "第 ";

            divFirst.appendChild(nFirst);
            divFirst.appendChild(nPrevious);
            nPaging.appendChild(divFirst);
            divLast.appendChild(nNext);
            divLast.appendChild(nLast);
            nPaging.appendChild(nPage);
            nPaging.appendChild(nInput);
            nPaging.appendChild(nOf);
            nPaging.appendChild(divLast);

            $(nFirst).click(function () {
                oSettings.oApi._fnPageChange(oSettings, "first");
                fnCallbackDraw(oSettings);
            });

            $(nPrevious).click(function () {
                oSettings.oApi._fnPageChange(oSettings, "previous");
                fnCallbackDraw(oSettings);
            });

            $(nNext).click(function () {
                oSettings.oApi._fnPageChange(oSettings, "next");
                fnCallbackDraw(oSettings);
            });

            $(nLast).click(function () {
                oSettings.oApi._fnPageChange(oSettings, "last");
                fnCallbackDraw(oSettings);
            });

            $(nInput).keyup(function (e) {

                if (e.which == 38 || e.which == 39) {
                    this.value++;
                }
                else if ((e.which == 37 || e.which == 40) && this.value > 1) {
                    this.value--;
                }

                if (this.value == "" || this.value.match(/[^0-9]/)) {
                    /* Nothing entered or non-numeric character */
                    return;
                }

                var iNewStart = oSettings._iDisplayLength * (this.value - 1);
                if (iNewStart > oSettings.fnRecordsDisplay()) {
                    /* Display overrun */
                    oSettings._iDisplayStart = (Math.ceil((oSettings.fnRecordsDisplay() - 1) /
                        oSettings._iDisplayLength) - 1) * oSettings._iDisplayLength;
                    fnCallbackDraw(oSettings);
                    return;
                }

                oSettings._iDisplayStart = iNewStart;
                fnCallbackDraw(oSettings);
            });

            /* Take the brutal approach to cancelling text selection */
            $('span', nPaging).bind('mousedown', function () { return false; });
            $('span', nPaging).bind('selectstart', function () { return false; });
        },


        "fnUpdate": function (oSettings, fnCallbackDraw) {
            if (!oSettings.aanFeatures.p) {
                return;
            }
            var iPages = Math.ceil((oSettings.fnRecordsDisplay()) / oSettings._iDisplayLength);
            var iCurrentPage = Math.ceil(oSettings._iDisplayStart / oSettings._iDisplayLength) + 1;

            /* Loop over each instance of the pager */
            var an = oSettings.aanFeatures.p;
            for (var i = 0, iLen = an.length ; i < iLen ; i++) {
                var spans = an[i].getElementsByTagName('span');
                var inputs = an[i].getElementsByTagName('input');
                var buttons = an[i].getElementsByTagName('button');
                spans[1].innerHTML = " 页, 共 " + iPages + " 页";
                inputs[0].value = iCurrentPage;

                if (iPages == 1) {
                    $(inputs[0]).attr("disabled", "disabled");
                }
                else if (iPages > 1) {
                    $(inputs[0]).removeAttr("disabled");
                }

                if (iCurrentPage == 1) {
                    $(buttons[0]).attr("disabled", "disabled");
                    $(buttons[1]).attr("disabled", "disabled");
                }
                else if (iCurrentPage > 1) {
                    $(buttons[0]).removeAttr("disabled");
                    $(buttons[1]).removeAttr("disabled");
                }

                if (iCurrentPage == iPages || iPages == 0) {
                    $(buttons[2]).attr("disabled", "disabled");
                    $(buttons[3]).attr("disabled", "disabled");
                }
                else if (iCurrentPage < iPages) {
                    $(buttons[2]).removeAttr("disabled");
                    $(buttons[3]).removeAttr("disabled");
                }
            }
        }
    };

    //==datatable defaults==//
    $.extend($.fn.dataTable.defaults, {
        bJQueryUI: true,
        sPaginationType: "input",
        bServerSide: true,
        sServerMethod: "POST",
        fnDrawCallback: function (oSettings) {
            var that = this;
            this.closest(".dataTables_wrapper").find("select").select2();
            this.$('tbody tr')
                .bind('mouseenter', function () { $(this).addClass("datatablerowhighlight"); })
                .bind('mouseleave', function () { $(this).removeClass("datatablerowhighlight"); })
                .on("click", function () {
                    if (that.fnSettings().oInit.singleSelect) {
                        $(this).siblings("tr").removeClass("checked");
                    }
                    $(this).toggleClass("checked");
                    that.trigger("rowClicked", [this, that]);
                });

            //grouping
            var groupCol = this.fnSettings().oInit.groupingCol;
            if (!_.isEmpty(groupCol)) {
                if (oSettings.aiDisplay.length == 0) {
                    return;
                }

                var nTrs = $("#" + oSettings.sTableId).find('tbody tr');
                var iColspan = nTrs[0].getElementsByTagName('td').length;
                var sLastGroup = "";
                for (var i = 0 ; i < nTrs.length ; i++) {
                    //var iDisplayIndex = oSettings._iDisplayStart + i;
                    var iDisplayIndex = i;
                    var sGroup = oSettings.aoData[oSettings.aiDisplay[iDisplayIndex]]._aData[groupCol];
                    if (sGroup != sLastGroup) {
                        var nGroup = document.createElement('tr');
                        var nCell = document.createElement('td');
                        nCell.colSpan = iColspan;
                        nCell.className = "group";
                        nCell.innerHTML = _.template("<span class='groupingTxt'><strong><%=txt%></strong>", { txt: sGroup });
                        nGroup.appendChild(nCell);
                        nTrs[i].parentNode.insertBefore(nGroup, nTrs[i]);
                        sLastGroup = sGroup;
                    }
                }
            }
        },
        fnServerData: function (sSource, aoData, fnCallback, oSettings) {
            var me = this;
            util.block($(this).closest(".widget-box"));
            var obj = {};
            _.each(aoData, function (item) {
                obj[item.name] = item.value;
            });

            var sortingIdx = [],
                sortingDirs = [],
                sortingEnables = [];

            _.each(aoData, function (item) {
                if (_.str.startsWith(item.name, "iSortCol_")) {
                    var sortIndex = item.name.split("_")[1];
                    sortingIdx.push(
                        {
                            sortIndex: sortIndex,
                            value: item.value
                        });
                }

                if (_.str.startsWith(item.name, "sSortDir_")) {
                    var sortIndex = item.name.split("_")[1];
                    sortingDirs.push(
                        {
                            sortIndex: sortIndex,
                            value: item.value
                        });
                }

                if (_.str.startsWith(item.name, "bSortable_")) {
                    var sortIndex = item.name.split("_")[1];
                    sortingEnables.push(
                        {
                            sortIndex: sortIndex,
                            value: item.value
                        });
                }
            });

            sortingIdx = _.sortBy(sortingIdx, function (item) {
                return item.sortIndex;
            });

            sortingDirs = _.sortBy(sortingDirs, function (item) {
                return item.sortIndex;
            });

            sortingEnables = _.sortBy(sortingEnables, function (item) {
                return item.sortIndex;
            });

            var colNames = obj.sColumns.split(",");

            var sortings = [];
            _.each(sortingIdx, function (item, i) {
                var enabled = sortingEnables[item.value];

                if (enabled.value) {
                    sortings.push(
                    {
                        sort: colNames[item.value],
                        dir: sortingDirs[i].value
                    });
                }
            });

            obj.sortings = sortings;

            oSettings.jqXHR = $.ajax({
                dataType: 'json',
                contentType: "application/json;charset=UTF-8",
                type: oSettings.sServerMethod,
                url: sSource,
                data: JSON.stringify(obj),
                complete: function(jqXHR, textStatus)
                {
                    util.unblock($(me).closest(".widget-box"));
                },
                success: function (data, textStatus, jqXHR) {
                    fnCallback(data, textStatus, jqXHR);
                }
            });
        }
    });

    $.fn.dataTableExt.oApi.fntoggleSelectAll = function (oSettings) {
        var nodes = this.fnGetNodes();
        $(nodes).trigger("click");

        return nodes;
    }

    $.fn.dataTableExt.oApi.fnUnSelectAll = function (oSettings) {
        var nodes = this.fnGetNodes();
        $(nodes).removeClass("checked");

        return nodes;
    }

    $.fn.dataTableExt.oApi.fngetSelectedNodes = function (oSettings) {
        var nodes = this.fnGetNodes();
        var selected = _.chain(nodes).filter(function (item) {
            return $(item).hasClass("checked");
        }).value();

        return selected;
    }
});