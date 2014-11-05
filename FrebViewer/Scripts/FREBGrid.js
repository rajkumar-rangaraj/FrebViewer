$(function () {
    $("#grid").jqGrid({
        url: "/GetList",
        datatype: 'json',
        mtype: 'Get',
        postData: {
            SearchValue: function () { return $('#txtSearch').val(); }
        },
        colNames: ['FileName', 'URL', 'Verb', 'AppPoolName', 'StatusCode', 'TimeTaken'],
        colModel: [
            { key: false, name: 'FileName', index: 'FileName', width: 110, searchoptions: { sopt: ['eq', 'ne', 'cn'] } },
            { key: false, name: 'URL', index: 'URL', width: 220, searchoptions: { sopt: ['eq', 'ne', 'cn'] } },
            { key: false, name: 'Verb', index: 'Verb', width: 80, searchoptions: { sopt: ['eq', 'ne', 'cn'] } },
            { key: false, name: 'AppPoolName', index: 'AppPoolName', width: 200, searchoptions: { sopt: ['eq', 'ne', 'cn'] } },
            { key: false, name: 'StatusCode', index: 'StatusCode', width: 120, searchoptions: { sopt: ['eq', 'ne', 'ge', 'le'] } },
            { key: false, name: 'TimeTaken', index: 'TimeTaken', width: 120, searchoptions: { sopt: ['eq', 'ne', 'ge', 'le'] } }
        ],
        pager: jQuery('#pager'),
        rowNum: 10,
        rowList: [10, 20, 30, 40],
        height: '100%',
        width: '100%',
        shrinkToFit:true,
        viewrecords: true,
        gridview: true,
        caption: 'FREB Viewer',
        sortname: 'FileName',
        sortorder: 'asc',
        emptyrecords: 'No records to display',
        jsonReader: {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            id: "0"
        },
        autowidth: true,
        multiselect: false,
        onSelectRow: function (id) {
            var grid = $('#grid'),
            selRowId = grid.jqGrid('getGridParam', 'selrow'),
            FileName = grid.jqGrid('getCell', selRowId, 'FileName');
            //alert(cellValue);
                     //FileName = FileName.replace('.xml', '')
            var url = "/GetFile?filename=" + FileName;
                     //var url = "/GetFile?" + FileName;
            window.open(url);
        },
        loadComplete: function () {
            var grid = $("#grid");
            var ids = grid.getDataIDs();
            for (var i = 0; i < ids.length; i++) {
                grid.setRowData(ids[i], false, { height: 25});
            }
        }
    }).navGrid("#pager",
    {
        edit: false, add: false,
        search: true, del: false
    }, {}, {}, {},
    { sopt: ['eq', 'ne', 'cn'] });

    $("#btnSearch").click(function (e) {
        e.preventDefault();
        $("#grid").trigger('reloadGrid');
    });

    $("#btnReset").click(function (e) {
        e.preventDefault();
        $('#txtSearch').val('');
        $("#grid").trigger('reloadGrid');
    });

    $(window).bind('resize', function () {
        var wid = $("#container").width();
        $("#grid").jqGrid('setGridWidth', (wid));
    }).trigger('resize');

});

