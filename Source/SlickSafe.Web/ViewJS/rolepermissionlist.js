/*
* SlickSafe 企业级权限快速开发框架遵循LGPL协议，也可联系作者获取商业授权
* 和技术支持服务；除此之外的使用，则视为不正当使用，请您务必避免由此带来的
* 商业版权纠纷。
*
The SlickSafe Product.
Copyright (C) 2017  .NET Authorization Framework Software

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

var rolepermissionlist = (function () {
    function rolepermissionlist() {
    }

    //#region Role Resource List
    rolepermissionlist.getRoleList = function(){
        jshelper.ajaxGet('api/RoleData/GetRoleAll', null, function (result) {
			if (result.Status === 1) {
				var divRolePermissionGrid = document.querySelector('#myrolepermissiongrid');
				$(divRolePermissionGrid).empty();

				var gridOptions = {
					columnDefs: [
						{ headerName: "ID", field: "ID", width: 40, cssClass: "bg-gray" },
						{ headerName: "角色名称", field: "RoleName", width: 140, cssClass: "bg-gray" },
						{ headerName: "角色代码", field: "RoleCode", width: 140, cssClass: "bg-gray" }
					],
					rowSelection: 'single',
                    onRowClicked: rowClicked,
				}

				new agGrid.Grid(divRolePermissionGrid, gridOptions);
				gridOptions.api.setRowData(result.Entity);

                function rowClicked(params) {
                    var node = params.node;
                    rolepermissionlist.pselectedRoleID = node.data.ID;
                    rolepermissionlist.pselectedRoleDataRow = node.data;

                    //load role resource tree view
                    getRoleResourceTree(rolepermissionlist.pselectedRoleID);
                }

			} else {
				$.msgBox({
					title: "Permission / List",
					content: "读取角色记录失败！错误信息：" + result.Message,
					type: "error"
				});
			}
		});
    }

    function getRoleResourceTree(roleID){  
        displayTreeContainer(true);
    
        var query = {"RoleID": roleID};
        jshelper.ajaxPost('api/ResourceData/GetRoleResourceList', JSON.stringify(query), function (result) {
			if (result.Status === 1) {
				var zNodes = [
			         //{ id: 0, pId: -1, name: "权限列表", type: "root", open: true },
		        ];
                var permissionNode = null;
                var resourceList = result.Entity;

                //push ztree nodelist
                $.each(resourceList, function(i, o){
                    permissionNode = {
                        id: o.ID,
                        pId: o.ParentID,
                        name: o.ResourceName,
                        permission: o.PermissionType,
                        open: true
                    };
                    zNodes.push(permissionNode);
                });

				//render zTree
				var t = $("#myroleresourcetree");
                rolepermissionlist.pmztree = $.fn.zTree.init(t, getZTreeSetting(), zNodes);
			} else {
				$.msgBox({
					title: "Permission / List",
					content: "读取角色资源权限数据失败！错误信息：" + result.Message,
					type: "error"
				});
			}
		});
    }

    function getZTreeSetting() {
		var setting = {
			check: {
				enable: false
			},
			view: {
				addDiyDom : addDiyDom,
				dblClickExpand: false,
				showLine: true,
				selectedMulti: false
			},
			data: {
				simpleData: {
					enable: true,
					idKey: "id",
					pIdKey: "pId",
					rootPId: ""
				}
			}
		};
		return setting;
	}

    function addDiyDom(treeId, treeNode) {
        if (treeNode.type === "root") return;

        var chkStr = "";
        var chkObj = $("#" + treeNode.tId + "_span");
        var chkAllow = null, chkDeny = null;
        var chkPrefix = "chkBoxR9Y7_";

        var chkBoxIDAllow = chkPrefix + "A_" + treeNode.id;
        var chkBoxIDDeny = chkPrefix + "D_" + treeNode.id;
        if ($("#" + chkBoxIDAllow).length > 0 || $("#" + chkBoxIDDeny).length > 0) return;
        
        chkStr += "<label><input type='checkbox' data-parent='" + treeNode.pId + "' class='checkBoxPermission checkBoxAllowed' id='" + chkBoxIDAllow + "' title='允许' ></label>";
        chkStr += "<label style='margin-left:25px;'><input type='checkbox' data-parent='" + treeNode.pId + "' class='checkBoxPermission checkBoxDenied' id='" + chkBoxIDDeny + "' title='拒绝' ><label>";
        chkStr = "<label style='margin-left:60px'>" + chkStr + "</label>";
    
        chkObj.after(chkStr);
        chkAllow = $("#" + chkBoxIDAllow);
        if (chkAllow){
            chkAllow.bind("change", function(){
                onCheckBoxAllowChecked(chkAllow, chkPrefix, treeNode.id, $(this).data("parent"));
            });
        }

        chkDeny = $("#" + chkBoxIDDeny);
        if (chkDeny){
            chkDeny.bind("change", function(){
                onCheckBoxDenyChecked(chkDeny, chkPrefix, treeNode);
            });
        }

        //set check status on each znode
        if (treeNode.permission === 1){
            $(chkAllow).attr("checked", true);
        } else if(treeNode.permission === -1) {
            $(chkDeny).attr("checked", true);
        }
	};

    function onCheckBoxAllowChecked(chkAllow, prefix, tId, pId){
        if ($(chkAllow).is(':checked') === false){
            return;     //needn't to verify upper level checkbox status
        }
        
        //when same level resource denied firstly, set it uncheck
        if (isSameLevelCheckBoxDenyChecked(prefix, tId)){
            $.msgBox({
				title: "RoleResource / List",
				content: "已经选择了拒绝权限，请确认要重新授权许可吗？",
				type: "confirm",
                success: function(result){
                    if (result === "Yes"){
                        var chkbox = $("#" + prefix + "D_" + tId);
                        if (chkbox && ($(chkbox).is(':checked') === true)) {
                            $(chkbox).attr("checked", false);       //set denied checkbox uncheck
                        }
                    } else {
                         $(chkAllow).attr("checked", false); 
                    }
                }
			});
            return false;
        }
        
        //when upper level resource denied firstly, return 
        if (isUpperLevelCheckBoxDenyChecked(prefix, pId)){
            $.msgBox({
				title: "RoleResource / List",
				content: "已经有上级拒绝权限存在，不能再次选择！",
				type: "alert"
			});
            $(chkAllow).attr("checked", false);
            return false;
        }

        //set upper level checked iterativly
        setUpperLevelCheckBoxAllowChecked(prefix, pId);
    }

    function isSameLevelCheckBoxDenyChecked(prefix, tId){
        var chkbox = $("#" + prefix + "D_" + tId);
        if (chkbox && ($(chkbox).is(':checked') === true)) 
            return true;
        else
            return false;
    }

    function isUpperLevelCheckBoxDenyChecked(prefix, pId){
        var chkbox = $("#" + prefix + "D_" + pId);
        if (chkbox && ($(chkbox).is(':checked') === true)) {
            return true;
        }

        var parent = $(chkbox).data("parent");
        if (parent > 0) {
            return isUpperLevelCheckBoxDenyChecked(prefix, parent);
        } 
        return false;
    }

    function setUpperLevelCheckBoxAllowChecked(prefix, pId){
        var chkbox = $("#" + prefix + "A_" + pId);
        if (chkbox && ($(chkbox).is(':checked') === false)) {
            $(chkbox).attr("checked", true);
        }

        var parent = $(chkbox).data("parent");
        if (parent > 0) setUpperLevelCheckBoxAllowChecked(prefix, parent);
    }

    function onCheckBoxDenyChecked(chkDeny, prefix, treeNode){
        var tId = treeNode.id;
        if ($(chkDeny).is(':checked') === false){
            return;     //needn't to verify same level checkbox status
        }

        if (isSameLevelCheckBoxAllowChecked(prefix, tId) === true){
            $.msgBox({
			    title: "RoleResource / List",
			    content: "当前权限为允许，请确认要修改为拒绝吗？",
			    type: "confirm",
                success: function(result){
                    if (result === "Yes"){
                        //when same level allow checked, then uncheck it and its children
                        setDownLevelCheckBoxChangedIterativly(prefix, treeNode);
                    } else {
                        $(chkDeny).attr("checked", false); 
                    }
                }
		    });
        } else {
            setDownLevelCheckBoxChangedIterativly(prefix, treeNode);
        }
    }

    function isSameLevelCheckBoxAllowChecked(prefix, tId){
        var chkbox = $("#" + prefix + "A_" + tId);
        if (chkbox && ($(chkbox).is(':checked') === true)) 
            return true;
        else
            return false;
    }

    function setDownLevelCheckBoxChangedIterativly(prefix, treeNode){
        var tId = treeNode.id;
        var chkboxA = $("#" + prefix + "A_" + tId);
        if (chkboxA) $(chkboxA).attr("checked", false);

        var chkboxD = $("#" + prefix + "D_" + tId);
        if (chkboxD) $(chkboxD).attr("checked", true);

        var children = treeNode.children;
        if (children && children.length > 0){
            $.each(children, function(i, child){
                setDownLevelCheckBoxChangedIterativly(prefix, child);
            });
        }
    }

    rolepermissionlist.save = function(){
        var resourcepermissionlist = getCheckedNodesList(rolepermissionlist.pselectedRoleID);       
        if (resourcepermissionlist.length > 0){
            //save role resource list
            jshelper.ajaxPost('api/ResourceData/SaveRoleResourceList', JSON.stringify(resourcepermissionlist), 
                function (result) {
                    if (result.Status === 1){
                    	$.msgBox({
					        title: "RoleResoure / List",
					        content: "保存角色资源权限记录成功！",
					        type: "info"
				        });
                    } else {
                    	$.msgBox({
					        title: "RoleResoure / List",
					        content: "保存角色资源权限记录失败！错误信息：" + result.Message,
					        type: "error"
				        });
                    }
            });
        } else {
            //clear role resource list
            var entity = {"RoleID": rolepermissionlist.pselectedRoleID};
            jshelper.ajaxPost('api/ResourceData/ClearRoleResourceList', JSON.stringify(entity), 
                function (result) {
                    if (result.Status === 1){
                    	$.msgBox({
					        title: "RoleResoure / List",
					        content: "清除角色资源权限记录成功！",
					        type: "info"
				        });
                    } else {
                    	$.msgBox({
					        title: "RoleResoure / List",
					        content: "清除角色资源权限记录失败！错误信息：" + result.Message,
					        type: "error"
				        });
                    }
            });
        }               
    }

    function getCheckedNodesList(roleID){
        var id = "";
        var node = null;
        var resourcepermissionlist = [];
        $.each($(".checkBoxPermission.checkBoxAllowed"), function(i, o){
            if ($(o).is(":checked") === true){
                id = o.id.substring(o.id.lastIndexOf("_") + 1);
                node = { 
                    "RoleID": roleID,
                    "ResourceID" : id,
                    "PermissionType" : 1
                }
                resourcepermissionlist.push(node);
            }
        });

        $.each($(".checkBoxPermission.checkBoxDenied"), function(i, o){
            if ($(o).is(":checked") === true){
                id = o.id.substring(o.id.lastIndexOf("_") + 1);
                node = { 
                    "RoleID": roleID,
                    "ResourceID" : id,
                    "PermissionType" : -1
                }
                resourcepermissionlist.push(node);
            }
        });
        return resourcepermissionlist;
    } 

    rolepermissionlist.cancel = function(){
        displayTreeContainer(false);
    }

    function displayTreeContainer(visible){
        if (visible === true){
            $("#treeContainerRoleResource").show(); 
        } else {
            $("#treeContainerRoleResource").hide(); 
        }
    }
    //#endregion

    return rolepermissionlist;
})()