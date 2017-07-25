$(function () {
    InitialPage();
    GetGrid();
    //初始化页面
    function InitialPage() {
        $(window).resize(function (e) {
            window.setTimeout(function () {
                $('#gridTable').setGridWidth(($('.gridPanel').width()));
                $("#gridTable").setGridHeight($(window).height() - 280);
            }, 200);
            e.stopPropagation();
        });
        ///#region btn
        $('#btn-add').click(function () {
            dialogOpen({
                id: "Form",
                title: 'Add PN',
                url: '/YQ/PN/Form',
                width: "600px",
                height: "500px",
                callBack: function (iframeId) {
                    top.frames[iframeId].AcceptClick();
                }
            });
        });
        $('#btn-edit').click(function () {
            var keyValue = $("#gridTable").jqGridRowValue("Id");
            if (checkedRow(keyValue)) {
                dialogOpen({
                    id: "Form",
                    title: 'Change PN',
                    url: '/YQ/PN/Form?keyValue=' + keyValue,
                    width: "600px",
                    height: "500px",
                    callBack: function (iframeId) {
                        top.frames[iframeId].AcceptClick();
                    }
                });
            }
        });
        $('#btn-delete').click(function () {
            var keyValue = $("#gridTable").jqGridRowValue("Id");
            if (checkedRow(keyValue)) {
                $.RemoveForm({
                    url: "/YQ/PN/Delete",
                    param: { keyValue: keyValue },
                    success: function (data) {
                        $("#gridTable").trigger("reloadGrid");
                    }
                });
            }
            else {
                dialogMsg('请选择需要删除的记录！', 0);
            }
        });
        $('#btn-upload').click(function () {
            dialogOpen({
                id: "Form",
                title: '上传基础资料',
                url: '/YQ/PN/Upload',
                width: "600px",
                height: "500px",
                callBack: function (iframeId) {
                    top.frames[iframeId].AcceptClick();
                }
            });
        });
        $('#btn-export').click(function () {
            //var load = layer.load("提交中...", 3);
            var url = "/YQ/PN/export";
            var el = document.createElement("a");
            document.body.appendChild(el);
            el.href = url; //url 是你得到的连接
            el.target = '_blank'; //指定在新窗口打开
            el.click();
            document.body.removeChild(el);
        });
        $('#btn-change-gsm').click(function () {
            var dlg = new BootstrapDialog({
                title: 'Replace GSM',
                draggable: true,
                closeByBackdrop: false,
                message: function () {
                    var $div = $('<div/>');
                    $div.append($('#div-change-gsm').html());
                    return $div;
                },
                buttons: [
                    {
                        label: 'OK',
                        cssClass: 'btn btn-ok',
                        action: function () {
                            var $frm = dlg.$modalContent.find("form");
                            if (!$frm.Validform()) {
                                return false;
                            }
                            //var postData = $frm.GetWebControls();
                            $.post("/YQ/PN/ChangeGSM", $frm.serialize(), function (data) {
                                if (data.code) {
                                    dlg.close();
                                    dialogMsg(data.msg, 1 /* OK */);
                                    $('#btn_Search').click();
                                }
                                else {
                                    dialogAlert(data.msg, -1 /* Error */);
                                }
                            });
                        }
                    },
                    {
                        label: 'Cancel',
                        cssClass: 'btn btn-cancel',
                        action: function () {
                            var me = this;
                            me.dialog.close();
                        }
                    },
                ],
                onshow: function () {
                    $('.gsm', dlg.$modalBody).select2({
                        placeholder: "==请选择==",
                        height: "200px",
                    });
                    // dlg.$modal.parent().removeAttr("tabindex");
                },
                onshown: function () {
                    dlg.$modalBody.closest(".modal.bootstrap-dialog").removeAttr("tabindex");
                },
                onhidden: function () {
                }
            });
            dlg.open();
        });
        ///#endregion
    }
    //加载表格
    function GetGrid() {
        var selectedRowIndex = 0;
        var $gridTable = $('#gridTable');
        $gridTable.jqGrid({
            url: "/YQ/PN/GridData",
            datatype: "json",
            height: $(window).height() - 280,
            autowidth: true,
            colModel: [
                { label: "Id", name: "Id", index: "Id", hidden: true },
                { label: "PN", name: "PN", index: "PN", width: 120, align: "left" },
                { label: "Desp", name: "Desp", index: "Desp", width: 300, align: "left" },
                { label: "GSM", name: "GSM", index: "GSM", width: 150, align: "left" },
                { label: "SBB Type", name: "SBBType", index: "SBBType", width: 80, align: "left" },
                { label: "Vendor", name: "Vendor", index: "Vendor", width: 150, align: "left" },
                { label: "Locations", name: "Locations", index: "Locations", width: 200, align: "left" }
            ],
            viewrecords: true,
            rowNum: 30,
            rownumWidth: 50,
            rowList: [30, 50, 100],
            pager: "#gridPager",
            sortname: 'PN',
            sortorder: 'asc',
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
                postData: {
                    keyword: $("#txt_Keyword").val()
                },
                page: 1
            }).trigger('reloadGrid');
        });
        //查询回车
        $('#txt_Keyword').bind('keypress', function (event) {
            if (event.keyCode.toString() == "13") {
                $('#btn_Search').trigger("click");
            }
        });
    }
});
