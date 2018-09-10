// 打开窗口
function openWindowForm(title, id, width, height) {
    if (id.substring(0, 1) !== "#") {
        id = "#" + id;
    }
    width = width || "500px";
    height = height || "auto";
    window.layer.open({
        type: 1,
        title: title,
        area: [width, height],
        fix: false,
        maxmin: false,
        content: $(id)
    });
}

// 关闭窗口
function closeWindowForm(index) {
    if (index) {
        window.layer.close(index);
    } else {
        window.layer.closeAll();
    }
}

// 清空查询条件
function clearCondition() {
    $("input[type='text']").each(function () {
        if (this.id !== "Lower" && this.id !== "Upper") {
            $(this).val("");
        }
    });
}

// 操作函数
function operationAction(url, data, callback, error, beforeSend) {
    var load;
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "JSON",
        timeout: 50000,
        data: data,
        url: url,
        beforeSend: function () {
            if (beforeSend) {
            } else {
                load = window.layer.load(1);
            }
        },
        success: function (d) {
            if (callback) {
                callback(d);
            }
        },
        error: function (e) {
            if (error) {
                error(e);
            }
        },
        complete: function () {
            if (load) {
                window.layer.close(load);
            }
        }
    });
    return false;
}

// 确认函数
function confirmAction(title, sureAction, cancelAction) {
    window.layer.confirm(title, { icon: 3, title: "提示" }, function () {
        if (sureAction) {
            sureAction();
        }
    }, function () {
        if (cancelAction) {
            cancelAction();
        }
    });
}

// 成功默认提示函数
function defualtSuccessAction(d, callback) {
    if (d.IsSucceed) {
        if (callback) {
            callback();
        }
        $.layerAlert(d.Message, { icon: 1 });
    } else {
        $.layerAlert(d.Message, { icon: 2 });
    }
}

// 获取下拉列表的文本值
function getSelectText(id, firstText) {
    firstText = firstText || "请选择";
    var selectText = $.trim($("#" + id).find("option:selected").text());
    return selectText === firstText ? "" : selectText;
}

// 获取下拉列表的文本值
function getSelectVal(id, firstText) {
    firstText = firstText || "";
    var selectVal = $.trim($("#" + id).find("option:selected").val());
    return selectVal === firstText ? "" : selectVal;
}

//获取分页信息
function getPaging(pageindex, pagesize) {
    return { "PageSize": pagesize, "PageIndex": pageindex, "GetRowsCount": true };
}
