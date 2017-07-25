/// <reference path="../../jq/jquery-1.8.3.min.js" />
/// <reference path="../../lib/jqform/jquery.form.min.js" />
/// <reference path="../../lib/utils/JS/learun-ui.js" />

$(document).ready(function () {
    $('#frm-warehouse').ajaxForm({
        url: '/YQ/Warehouse/Upload',
        success: function (data) {
            if (data.type == "3") {
                dialogAlert(data.message, -1);
            } else {
                Loading(false);
                dialogMsg(data.message, 1);
            }
            Loading(false);
        },
        resetForm: false,
        dataType: 'json',
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            Loading(false);
            dialogMsg(errorThrown, -1);
        },
        beforeSubmit: function () {
            Loading(true, "正在上传文件数据...");
        },
        complete: function () {
            Loading(false);
        }
    });

});


