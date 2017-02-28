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


var userpermissionlist = (function () {
    function userpermissionlist() {
    }


    //#region User Resource Permission
	userpermissionlist.getUserList = function() {
		jshelper.ajaxGet('api/RoleData/GetUserAll', null, function (result) {
			if (result.Status === 1) {
				var divUserGrid = document.querySelector('#myuserpermissiongrid');
				$(divUserGrid).empty();

				var gridOptions = {
					columnDefs: [
					    { headerName: "ID", field: "ID", width: 40, cssClass: "bg-gray" },
						{ headerName: "用户名称", field: "UserName", width: 120, cssClass: "bg-gray" },
					],
					rowSelection: 'single',
					onRowClicked: rowClicked,
				}
				
				new agGrid.Grid(divUserGrid, gridOptions);
				gridOptions.api.setRowData(result.Entity);

                function rowClicked(params) {
                    var node = params.node;
                    userpermissionlist.pselectedUserID = node.data.ID;
                    userpermissionlist.pselectedUserDataRow = node.data;

                    //load user resource tree view
                    retrieveUserResourceTree(userpermissionlist.pselectedUserID);
                }
			} else {
				$.msgBox({
					title: "User / List",
					content: "读取用户记录失败！错误信息：" + result.Message,
					type: "error"
				});
			}
		});
	}

    function retrieveUserResourceTree(userID){
        displayTreeContainer(true);

        var query = {"UserID": userID};
        jshelper.ajaxPost('api/ResourceData/RetrieveUserResourceList', JSON.stringify(query), function (result) {
			if (result.Status === 1) {
				var zNodes = [
			         //{ id: 0, pId: -1, name: "权限列表", type: "root", open: true },
		        ];
                var permissionNode = null;
                var resourceList = result.Entity;
                $.each(resourceList, function(i, o){
                    permissionNode = {
                        id: o.ID,
                        pId: o.ParentID,
                        name: o.ResourceName,
                        isAllowInherited: o.IsAllowInherited,
                        isAllowSelf: o.IsAllowSelf,
                        isDenyInherited: o.IsDenyInherited,
                        isDenySelf: o.IsDenySelf,
                        open: true
                    };
                    zNodes.push(permissionNode);
                });

				//render zTree
				var t = $("#myuserresourcetree");
                userpermissionlist.pmztree = $.fn.zTree.init(t, getZTreeSetting(), zNodes);
			} else {
				$.msgBox({
					title: "UserResource / List",
					content: "读取用户资源权限数据失败！错误信息：" + result.Message,
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
        var chkAllowInherited = null, chkAllowSelf = null, chkDenyInherited = null, chkDenySelf = null;
        var chkPrefix = "chkBoxT0Z6_";

        var chkBoxIDAllowInherited = chkPrefix + "A_I_" + treeNode.id;
        var chkBoxIDAllowSelf = chkPrefix + "A_S_" + treeNode.id;

        var chkBoxIDDenyInherited = chkPrefix + "D_I_" + treeNode.id;
        var chkBoxIDDenySelf = chkPrefix + "D_S_" + treeNode.id;

        if ($("#" + chkBoxIDAllowInherited).length > 0 || $("#" + chkBoxIDDenyInherited).length > 0) return;
        
        chkStr += "<label><input type='checkbox' data-parent='" + treeNode.pId + "' class='checkBoxPermission checkBoxAllowed checkBoxAllowedInherited' id='" + chkBoxIDAllowInherited + "' title='允许(继承)' disabled ></label>";
        chkStr += "<label style='margin-left:10px;'><input type='checkbox' data-parent='" + treeNode.pId + "' class='checkBoxPermission checkBoxAllowed checkBoxAllowedSelf' id='" + chkBoxIDAllowSelf + "' title='允许(自有)' ></label>";
        chkStr += "<label style='margin-left:50px;'><input type='checkbox' data-parent='" + treeNode.pId + "' class='checkBoxPermission checkBoxDenied checkBoxDeniedInherited' id='" + chkBoxIDDenyInherited + "' title='拒绝(继承)' disabled ><label>";
        chkStr += "<label style='margin-left:10px;'><input type='checkbox' data-parent='" + treeNode.pId + "' class='checkBoxPermission checkBoxDenied checkBoxDeniedSelf' id='" + chkBoxIDDenySelf + "' title='拒绝(自有)' ><label>";
        chkStr = "<label style='margin-left:60px'>" + chkStr + "</label>";
    
        chkObj.after(chkStr);
        
        chkAllowInherited = $("#" + chkBoxIDAllowInherited);
        chkAllowSelf = $("#" + chkBoxIDAllowSelf);
        chkDenyInherited = $("#" + chkBoxIDDenyInherited);
        chkDenySelf = $("#" + chkBoxIDDenySelf);

        if (treeNode.isAllowInherited === 1)
            $(chkAllowInherited).attr("checked", true);

        if (treeNode.isAllowSelf === 1)
            $(chkAllowSelf).attr("checked", true);

        if (treeNode.isDenyInherited === 1)
            $(chkDenyInherited).attr("checked", true);

        if (treeNode.isDenySelf === 1)
            $(chkDenySelf).attr("checked", true);

        //bind event
        if (chkAllowSelf){
            chkAllowSelf.bind("change", function(){
                onCheckBoxAllowSelfChecked(chkAllowSelf, chkPrefix, treeNode, $(this).data("parent"));
            });
            //chkAllowSelf.addClass("socheckbox-self");
        }

        if (chkDenySelf){
            chkDenySelf.bind("change", function(){
                onCheckBoxDenySelfChecked(chkDenySelf, chkPrefix, treeNode, $(this).data("parent"));
            });
            //chkDenySelf.addClass("socheckbox-self");
        }
	};

    //allow checkbox status change event
    function onCheckBoxAllowSelfChecked(chkAllowSelf, prefix, treeNode, pId){
        if ($(chkAllowSelf).is(':checked') === false){
            return;     //needn't to verify upper level checkbox status
        }

        //when same level resource denied firstly, return
        var tId = treeNode.id;
        if (isSameLevelCheckBoxDenyChecked(prefix, tId)){
            $.msgBox({
				title: "UserResource / List",
				content: "已经选择了拒绝权限，请确认要重新授权许可吗？",
				type: "confirm",
                success: function(result){
                    if (result === "Yes"){
                        var chkbox = $("#" + prefix + "D_S_" + tId);
                        if (chkbox && ($(chkbox).is(':checked') === true)) {
                            $(chkbox).attr("checked", false);       //set denied checkbox uncheck
                        }
                    } else {
                         $(chkAllowSelf).attr("checked", false); 
                    }
                }
			});
            return false;
        }
        
        //when upper level resource denied firstly, return 
        if (isUpperLevelCheckBoxDenyChecked(prefix, pId)){
            $.msgBox({
				title: "UserResource / List",
				content: "已经有上级拒绝权限存在，不能再次选择！",
				type: "alert"
			});
            $(chkAllowSelf).attr("checked", false);
            return false;
        }

        //set upper level checked iterativly
        setUpperLevelCheckBoxAllowChecked(prefix, treeNode, pId);
    }

    function isSameLevelCheckBoxDenyChecked(prefix, tId){
        var chkbox = $("#" + prefix + "D_S_" + tId);
        if (chkbox && ($(chkbox).is(':checked') === true)) 
            return true;
        else
            return false;
    }

    function isUpperLevelCheckBoxDenyChecked(prefix, pId){
        var chkbox = $("#" + prefix + "D_S_" + pId);
        if (chkbox && ($(chkbox).is(':checked') === true)) {
            return true;
        }

        var parent = $(chkbox).data("parent");
        if (parent > 0) {
            return isUpperLevelCheckBoxDenyChecked(prefix, parent);
        } 
        return false;
    }

    function setUpperLevelCheckBoxAllowChecked(prefix, treeNode, pId){
        var chkbox = $("#" + prefix + "A_S_" + pId);
        if (chkbox && ($(chkbox).is(':checked') === false)) {
            $(chkbox).attr("checked", true);
        }

        var parent = $(chkbox).data("parent");
        if (parent > 0) setUpperLevelCheckBoxAllowChecked(prefix, treeNode.parent, parent);
    }

    //deny checkbox status change event
    function onCheckBoxDenySelfChecked(chkDenySelf, prefix, treeNode, parent){
        var tId = treeNode.id;
        if ($(chkDenySelf).is(':checked') === false){
            return;     //needn't to verify same level checkbox status
        }

        if (isSameLevelCheckBoxAllowChecked(prefix, tId) === true){
            $.msgBox({
			    title: "UserResource / List",
			    content: "当前权限为允许，请确认要修改为拒绝吗？",
			    type: "confirm",
                success: function(result){
                    if (result === "Yes"){
                        //when same level allow checked, then uncheck it and its children
                        setDownLevelCheckBoxChangedIterativly(prefix, treeNode);
                    } else {
                        $(chkDenySelf).attr("checked", false); 
                    }
                }
		    });
        } else {
            setDownLevelCheckBoxChangedIterativly(prefix, treeNode);
        }
    }

    function isSameLevelCheckBoxAllowChecked(prefix, tId){
        var chkbox = $("#" + prefix + "A_S_" + tId);
        if (chkbox && ($(chkbox).is(':checked') === true)) 
            return true;
        else
            return false;
    }

    function setDownLevelCheckBoxChangedIterativly(prefix, treeNode){
        var tId = treeNode.id;
        var chkboxA = $("#" + prefix + "A_S_" + tId);
        if (chkboxA) $(chkboxA).attr("checked", false);

        var chkboxD = $("#" + prefix + "D_S_" + tId);
        if (chkboxD) $(chkboxD).attr("checked", true);

        var children = treeNode.children;
        if (children && children.length > 0){
            $.each(children, function(i, child){
                setDownLevelCheckBoxChangedIterativly(prefix, child);
            });
        }
    }


    userpermissionlist.save = function(){
        var resourcepermissionlist = getCheckedNodesList(userpermissionlist.pselectedUserID);       
        if (resourcepermissionlist.length > 0){
            //save role resource list
            jshelper.ajaxPost('api/ResourceData/SaveUserResourceList', JSON.stringify(resourcepermissionlist), 
                function (result) {
                    if (result.Status === 1){
                    	$.msgBox({
					        title: "UserResoure / List",
					        content: "保存用户资源权限记录成功！",
					        type: "info"
				        });
                    } else {
                    	$.msgBox({
					        title: "UserResoure / List",
					        content: "保存用户资源权限记录失败！错误信息：" + result.Message,
					        type: "error"
				        });
                    }
            });
        } else {
            //clear role resource list
            var entity = {"UserID": userpermissionlist.pselectedUserID};
            jshelper.ajaxPost('api/ResourceData/ClearUserResourceList', JSON.stringify(entity), 
                function (result) {
                    if (result.Status === 1){
                    	$.msgBox({
					        title: "UserResoure / List",
					        content: "清除用户资源自有权限记录成功！",
					        type: "info"
				        });
                    } else {
                    	$.msgBox({
					        title: "RoleResoure / List",
					        content: "清除用户资源自有权限记录失败！错误信息：" + result.Message,
					        type: "error"
				        });
                    }
            });
        }    
    }

    function getCheckedNodesList(userID){
        var id = "";
        var node = null;
        var resourcepermissionlist = [];
        $.each($(".checkBoxPermission.checkBoxAllowed.checkBoxAllowedSelf"), function(i, o){
            id = o.id.substring(o.id.lastIndexOf("_") + 1);
            if ($(o).is(":checked") === true) {
                node = {
                    "UserID": userID,
                    "ResourceID": id,
                    "PermissionType": 1,
                    "IsInherited": -1 
                };
                resourcepermissionlist.push(node);
            } 
        });

        $.each($(".checkBoxPermission.checkBoxDenied.checkBoxDeniedSelf"), function(i, o){
            id = o.id.substring(o.id.lastIndexOf("_") + 1);
            if ($(o).is(":checked") === true){
                node = {
                    "UserID": userID,
                    "ResourceID": id,
                    "PermissionType": -1,
                    "IsInherited": -1 
                };
                resourcepermissionlist.push(node);
            }
        });
        return resourcepermissionlist;
    } 

    userpermissionlist.cancel = function(){
        displayTreeContainer(false);
    }

    function displayTreeContainer(visible){
        if (visible === true){
            $("#treeContainerUserResource").show(); 
        } else {
            $("#treeContainerUserResource").hide(); 
        }
    }
    //#endregion

    return userpermissionlist;
})()