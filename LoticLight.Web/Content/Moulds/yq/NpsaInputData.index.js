$(function () {
    InitialPage();
    GetGrid();
    function InitialPage() {
        $(window).resize(function (e) {
            window.setTimeout(function () {
                $('#gridTable').setGridWidth(($('.gridPanel').width() - 40));
                $("#gridTable").setGridHeight($(window).height() - 300);
            }, 200);
            e.stopPropagation();
        });
        $('#btn-project-batch-change').click(function () {
            var dlg = new BootstrapDialog({
                title: 'Project批量修改',
                draggable: true,
                closeByBackdrop: false,
                message: function () {
                    var $div = $("<div/>");
                    $div.append($("#tpl-project-batch-change").html());
                    $div.find('#yzn-file-test').on('change', function () {
                        $div.find('#txt-file').val($(this).val());
                        console.log($(this).val());
                    });
                    dlg.open();
                    $div.find('#btn-download').click(function () {
                        var p = $div.find("#Project").val();
                        if (p == "") {
                            dialogAlert("必须选择一个Project后,才能下载!", -1 /* Error */);
                            return;
                        }
                        var oProject = { project: p };
                        downExcel('/NPSAInputData/DownLoadLastWeekForAProject?' + $.param(oProject));
                    });
                    $div.find('#btn-del-a-project').click(function () {
                        var $me = $(this);
                        Loading(true, "正在处理...");
                        var p = $div.find("#Project").val();
                        if (p == "") {
                            Loading(false);
                            dialogAlert("必须选择一个Project后,才能删除!", -1 /* Error */);
                            return;
                        }
                        $.post("/NPSAInputData/DelAProject", { project: p }, function (data) {
                            if (data.code) {
                                Loading(false);
                                dialogMsg(data.msg, 1 /* OK */);
                            }
                            else {
                                Loading(false);
                                dialogAlert(data.msg, -1 /* Error */);
                            }
                        });
                    });
                    $div.find("#frm-upload").ajaxForm({
                        success: function (data) {
                            Loading(false);
                            if (data.code) {
                                dialogAlert(data.msg, 1 /* OK */);
                            }
                            else {
                                dialogAlert(data.msg, -1 /* Error */);
                            }
                        },
                        resetForm: false,
                        dataType: 'json',
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            Loading(false);
                            dialogMsg(errorThrown, -1);
                        },
                        beforeSubmit: function () {
                            Loading(true, "正在上传文件数据并处理,请稍后...");
                        },
                        complete: function () {
                            Loading(false);
                        }
                    });
                    return $div;
                },
                buttons: [{
                        label: '关闭',
                        cssClass: 'btn-cancel',
                        action: function () {
                            dlg.close();
                        }
                    }],
                onshow: function () {
                    $('#Project', dlg.$modalBody).select2({
                        placeholder: "==请选择==",
                        height: "200px",
                    });
                },
                onshown: function () {
                    dlg.$modalBody.closest(".modal.bootstrap-dialog").removeAttr("tabindex");
                }
            });
            dlg.realize();
        });
        $('#btn-down-lead-data').click(function () {
            downExcel("/NPSAInputData/DownLeadData");
        });
    }
    //加载表格
    function GetGrid() {
        var selectedRowIndex = 0;
        var $gridTable = $('#gridTable');
        $gridTable.jqGrid({
            url: "/YQ/NPSAInputData/GridData",
            postData: $('#form-search').serializeObject(),
            datatype: "json",
            //method:'post',
            height: $(window).height() - 280,
            autowidth: true,
            colModel: [
                { label: "Id", name: "Id", index: "Id", hidden: true },
                { label: "Mode", name: "Mode", index: "Mode", width: 60, align: "left" },
                { label: "PN", name: "PN", index: "PN", width: 150, align: "left" },
                { label: "SBB Type", name: "SBBType", index: "SBBType", width: 100, align: "left" },
                { label: "PN Desp", name: "PNDesp", index: "PNDesp", width: 200, align: "left" },
                { label: "Project", name: "Project", index: "Project", width: 100, align: "left" },
                {
                    label: "API", name: "API", index: "API", width: 120, align: "left",
                    formatter: function (cellvalue, options, rowObject) {
                        try {
                            return cellvalue.toString().substr(0, 10);
                        }
                        catch (ex) {
                            return "";
                        }
                    }
                },
                { label: "Location", name: "Location", index: "Location", width: 100, align: "left" },
                { label: "Vendor", name: "Vendor", index: "Vendor", width: 100, align: "left" },
                { label: "Demand", name: "Demand", index: "Demand", width: 60, align: "left" },
                { label: "GSM", name: "GSM", index: "GSM", width: 100, align: "left" },
                { label: "NPSA", name: "NPSA", index: "NPSA", width: 100, align: "left" }
            ],
            viewrecords: true,
            rowNum: 30,
            rowList: [30, 50, 100],
            pager: "#gridPager",
            sortname: 'InputDate',
            sortorder: 'desc',
            rownumbers: true,
            shrinkToFit: false,
            gridview: true,
            onSelectRow: function () {
                selectedRowIndex = $("#" + this.id).getGridParam('selrow');
            },
            gridComplete: function () {
                $("#" + this.id).setSelection(selectedRowIndex, false);
            }
        });
        //    $gridTable.authorizeColModel();
        //查询事件
        $("#btn_Search").click(function () {
            $gridTable.jqGrid('setGridParam', {
                postData: $('#form-search').serializeObject(),
                page: 1
            }).trigger('reloadGrid');
        });
    }
});
//初始化页面
//新增
function btn_add() {
    dialogOpen({
        id: "Form",
        title: 'Add This Week Data',
        url: '/YQ/NPSAInputData/Form',
        width: "960px",
        height: "600px",
        callBack: function (iframeId) {
            top.frames[iframeId].AcceptClick();
        }
    });
}
;
//编辑
function btn_edit() {
    var keyValue = $("#gridTable").jqGridRowValue("Id");
    if (checkedRow(keyValue)) {
        dialogOpen({
            id: "Form",
            title: 'Change NPSAInputData',
            url: '/YQ/NPSAInputData/Form?keyValue=' + keyValue,
            width: "960px",
            height: "600px",
            callBack: function (iframeId) {
                top.frames[iframeId].AcceptClick();
            }
        });
    }
}
//删除
function btn_delete() {
    var keyValue = $("#gridTable").jqGridRowValue("Id");
    if (checkedRow(keyValue)) {
        $.RemoveForm({
            url: "/YQ/NPSAInputData/Delete",
            param: { keyValue: keyValue },
            success: function (data) {
                $("#gridTable").trigger("reloadGrid");
            }
        });
    }
    else {
        dialogMsg('请选择需要删除的记录！', 0);
    }
}
function downExcel(url) {
    var el = document.createElement("a");
    document.body.appendChild(el);
    el.href = url; //url 是你得到的连接
    el.target = '_blank'; //指定在新窗口打开
    el.click();
    document.body.removeChild(el);
}
function btn_export() {
    //var load = layer.load("提交中...", 3);
    downExcel("/YQ/NPSAInputData/export");
}
//本周录入数据的计算结果
function btn_cal_result() {
    downExcel("/YQ/NPSAInputData/downCalResult");
}
//btn_publishToGSM
function btn_publishToGSM() {
    var url = "/YQ/NPSAInputData/publishToGSM";
    Loading(true, "正在生成发布数据...");
    $.post(url, {}, function (data) {
        var data = $.parseJSON(data);
        if (data.type == "3") {
            dialogAlert(data.message, -1);
        }
        else {
            Loading(false);
            dialogMsg(data.message, 1);
            downExcel("/YQ/NPSAInputData/DownloadToGSM");
        }
        Loading(false);
    });
}
function btn_workToGSM() {
    dialogContent({
        id: 'worktogsm',
        title: '系统窗口',
        width: "100px",
        height: "100px",
        content: $('#div-work-to-gsm').html(),
        btn: null,
        callBack: null
    });
}
