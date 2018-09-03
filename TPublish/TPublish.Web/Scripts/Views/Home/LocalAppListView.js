$(function () {
    var index = 10;
    $("#tableId").treetable({
        expandable: true,
        onNodeExpand: function () {
            //var node = this;
            //var childSize = $("#tableId").find("[data-tt-parent-id='" + node.id + "']").length;
            //console.log("childSize = ", childSize)

            //var rows = ' <tr data-tt-id="' + index + '"  data-tt-parent-id="' + node.id + '"><td>ParentA</td><td>ParentB</td><td>ParentC</td></tr>'
            //index++;
            //$("#tableId").treetable("loadBranch", node, rows);// 插入子节点
            //$("#tableId").treetable("expandNode", node.id);// 展开子节点
        }
    });

    $("#tableId").treetable("expandAll", "");
});

function ExpandOrCollapsById(id) {
    var isCollapsed = $("#" + id).hasClass("collapsed");
    if (isCollapsed === true) {
        $("#tableId").treetable("expandNode", id);
    } else {
        $("#tableId").treetable("collapseNode", id);
    }
}

function ExpandOrCollaps(obj) {
    var isCollapsed = obj.classList.contains("collapsed");
    if (isCollapsed === true) {
        $("#tableId").treetable("expandNode", obj.id);
    } else {
        $("#tableId").treetable("collapseNode", obj.id);
    }
}