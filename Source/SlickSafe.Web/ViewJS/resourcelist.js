/*
*  SlickSafe 企业级权限快速开发框架遵循LGPL协议，也可联系作者商业授权并获取技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的商业版权纠纷。

The SlickSafe project.
Copyright (C) 2017  .NET Web Framwork Library

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, you can access the official
web page about lgpl: https://www.gnu.org/licenses/lgpl.html
*/

var resourcelist = (function (){
    function resourcelist(){

    }

    resourcelist.loadResource = function () {
        $(".resource-type").hide();

        if (somain.activeToolButtonType === "add") {
            $("#txtResourceName").val("");
            $("#ddlResourceTypeID").val("0");   
            $("#txtUrlAction").val("");
            $("#txtDataAction").val("");
            $("#txtStyleClass").val("");
            $("#txtOrderNum").val("");
        } else if(somain.activeToolButtonType === "edit"
            && resourcelist.pselectedResourceID !== "") {
            var entity = resourcelist.pselectedResourceDataRow;
            $("#txtResourceName").val(entity.ResourceName);
            $("#ddlResourceTypeID").val(entity.ResourceTypeID);
            $("#txtUrlAction").val(entity.UrlAction);
            $("#txtDataAction").val(entity.DataAction);
            $("#txtStyleClass").val(entity.StyleClass);
            $("#txtOrderNum").val(entity.OrderNum);
        }

        //set urlaction/dataction visible for different resource type record
        setFieldVisible(resourcelist.pselectedResourceTypeID, somain.activeToolButtonType);
    }

    function setFieldVisible(resourceTypeID, opType){
        if (opType === "edit"){
            if (resourceTypeID === 1)  $(".resource-type-system").show();
            if (resourceTypeID === 2)  $(".resource-type-page").show();
            if (resourceTypeID === 5)  $(".resource-type-action").show();
        } else {
            if (resourceTypeID === 0)  $(".resource-type-system").show();
            if (resourceTypeID === 1)  $(".resource-type-page").show();
            if (resourceTypeID === 2)  $(".resource-type-action").show();
        }
    }

    resourcelist.getResourceList = function(){
        jshelper.ajaxGet('api/ResourceData/GetResourceNodeAll', null, function (result) {
			if (result.Status === 1) {
				var gridOptions = {
					columnDefs: [
                        { headerName: "ID", field: "ID", width: 40, cssClass: "bg-gray" },
						{ headerName: "资源名称", field: "ResourceName", width: 160, cssClass: "bg-gray", 
                            cellRenderer: 'group',
                            cellRendererParams: {
                                innerRenderer: innerCellRenderer
                            }
                        },
                        { headerName: "类型", field: "ResourceTypeID", width: 40, cssClass: "bg-gray" },
                        { headerName: "页面URL", field: "UrlAction", width: 120, cssClass: "bg-gray" },
                        { headerName: "数据操作", field: "DataAction", width: 120, cssClass: "bg-gray" },
						{ headerName: "样式", field: "StyleClass", width: 200, cssClass: "bg-gray" },
                        { headerName: "排序", field: "OrderNum", width: 60, cssClass: "bg-gray" },
					],
					rowSelection: 'single',
                    enableColResize: true,
                    enableSorting: true,
                    animateRows: true,
                    rowHeight: 30,
                    getNodeChildDetails: function(node) {
                        if (node.group) {
                            return {
                                group: true,
                                name: node.ResourceName,
                                children: node.children,
                                expanded: node.ResourceTypeID < 3 ? "true" : "false"
                            };
                        } else {
                            return null;
                        }
                    },
                    onRowClicked: rowClicked,
				};

				var divResourceGrid = document.querySelector('#myresourcegrid');
				$(divResourceGrid).empty();

                var rowData = [];
                rowData.push(result.Entity);

				new agGrid.Grid(divResourceGrid, gridOptions);
                gridOptions.api.setRowData(rowData);

                function innerCellRenderer(params) {
                    return params.node.data.ResourceName;
                }

                function rowClicked(params) {
                    var node = params.node;
                    resourcelist.pselectedResourceID = node.data.ID;
                    resourcelist.pselectedResourceTypeID = node.data.ResourceTypeID;
                    resourcelist.pselectedResourceDataRow = node.data;
                }
			} else {
				$.msgBox({
					title: "Role / List",
					content: "读取资源记录失败！错误信息：" + result.Message,
					type: "error"
				});
			}
		});
    }

    resourcelist.saveResource = function () {
        if ($("#txtResourceName").val() == ""
            || $("#txtResourceCode").val() == "") {
            $.msgBox({
                title: "SlickSafe / Resource",
                content: "请输入资源基本信息！",
                type: "alert"
            });
            return false;
        }

        var entity = {
            "ID": "0",
            "ResourceName": $("#txtResourceName").val(),
            "ResourceTypeID": $("#ddlResourceTypeID").val(),
            "UrlAction": $("#txtUrlAction").val(),
            "StyleClass": $("#txtStyleClass").val(),
            "OrderNum": $("#txtOrderNum").val()
        };

        if (somain.activeToolButtonType === "edit") {
            entity.ID = resourcelist.pselectedResourceID;
            entity.ParentID = resourcelist.pselectedResourceDataRow.ParentID;
        } else {
            entity.ParentID = resourcelist.pselectedResourceID;
        }

        resourceapi.save(entity);
    }

    resourcelist.delete = function () {
        $.msgBox({
            title: "Are You Sure",
            content: "确实要删除资源记录吗? ",
            type: "confirm",
            buttons: [{ value: "Yes" }, { value: "Cancel" }],
            success: function (result) {
                if (result == "Yes") {
                    var entity = {
                        "ID": resourcelist.pselectedResourceID,
                    };
                    resourceapi.delete(entity);
                    return;
                }
            }
        });
    }

    return resourcelist;
})()

var resourceapi = (function () {
	function resourceapi() {
	}

    resourceapi.save = function (entity) {
		jshelper.ajaxPost('api/ResourceData/SaveResource',
            JSON.stringify(entity),
            function (result) {
            	if (result.Status == 1) {
            		$.msgBox({
            			title: "SlickSafe / Resource",
            			content: "已经成功保存资源数据！",
            			type: "info"
            		});

            		//refresh
            		resourcelist.getResourceList();

            	} else {
            		$.msgBox({
            			title: "SlickSafe / Resource",
            			content: result.Message,
            			type: "error",
            			buttons: [{ value: "Ok" }],
            		});
            	}
            });
	}

	resourceapi.delete = function (entity) {
		//delete the selected row
		jshelper.ajaxPost('api/ResourceData/DeleteResource',
            JSON.stringify(entity),
            function (result) {
            	if (result.Status == 1) {
            		$.msgBox({
            			title: "SlickSafe / Resource",
            			content: "资源记录已经删除！",
            			type: "info"
            		});

            		//refresh
                    resourcelist.getResourceList();
            	} else {
            		$.msgBox({
            			title: "Ooops",
            			content: result.Message,
            			type: "error",
            			buttons: [{ value: "Ok" }],
            		});
            	}
            });
	}

	return resourceapi;
})()