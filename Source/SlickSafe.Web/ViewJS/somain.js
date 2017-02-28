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


var somain = (function () {
    function somain() {
	}

    somain.tablist = [];
	somain.activeTabName = "";
	somain.activeToolButtonType = "";
    somain.userPermissionTreeView = null;
    
    somain.init = function(){
        var trigger = $('.hamburger'),
                isClosed = false;

        trigger.click(function () {
            hamburger_cross();
        });

        function hamburger_cross() {
            if (isClosed == true) {
                trigger.removeClass('is-open');
                trigger.addClass('is-closed');
                isClosed = false;
            } else {
                trigger.removeClass('is-closed');
                trigger.addClass('is-open');
                isClosed = true;
            }
        }

        $('[data-toggle="offcanvas"]').click(function () {
            $('#wrapper').toggleClass('toggled');
        });

        //load left menu tree
        loadLeftMenuList();
    }

    function loadLeftMenuList(){
         //get userid from cookie
         var logonUserID = lsm.getWebLogonUserID();
         jshelper.ajaxGet('api/ResourceData/GetLeftMenuList/' + logonUserID, null, function (result) {
			if (result.Status === 1) {
                var menuData = result.Entity;

                //create left side bar menu dynamically
                somain.userPermissionTreeView = menuData;
                createLeftSideBarMenuDynamiclly(menuData);
            } else {
				$.msgBox({
					title: "Menu / List",
					content: "读取菜单记录失败！错误信息：" + result.Message,
					type: "error"
				});
            }
        });
    }

    function createLeftSideBarMenuDynamiclly(menuData){
        var first = true;
        var firstChild = true;
        var menuClassCollapsed = "collapsed";
        var menuClassActive = " active";
        var menuClass = "";

        //add dynamic page from database
        $.each(menuData, function(i, item){
            var styleClass = item.StyleClass;
            var title = item.ResourceName;
            var code = "RESX06Z9-" + item.ID;
            
            if (first) 
                menuClass = menuClassCollapsed + menuClassActive;
            else
                menuClass = menuClassCollapsed;
            
            //sub system level menu
            $('<li data-toggle="collapse" data-target="#' + code + '" class="' + menuClass + '">'
                + '<a href="#"><i class="' + styleClass + '"></i> ' + title + ' <span class="arrow"></span></a>'
                +'</li>').appendTo($('#menu-content'));

            //module level menu
            var subMenu = $('<ul class="sub-menu collapse" id="' + code + '"></ul>').appendTo($('#menu-content'));
            $.each(item.children, function(i, child){
                var pageObj = { 
                    name: "RESX06Z9-" + child.ID,
                    title: child.ResourceName,
                    url: child.UrlAction      //form or page url from database
                };

                var classStyle = "";
                if (first && firstChild)
                    classStyle = 'class="' + "active" + '"';

                $('<li><a href="#" ' + classStyle + "data-page='" + JSON.stringify(pageObj) + "'" + ' onclick="somain.showTab(this)">'
                    + child.ResourceName + '</a></li>')
                 .appendTo($(subMenu));

                firstChild = false;
            });

            first = false;
        });

        //add profile page reserved
        var pageProfile = { 
            name: "myprofile", 
            title: "个人资料",
            url: "profile/index"
        };

        $('<li data-target="#' + pageProfile.name + '"><a href="#"' + "data-page='" + JSON.stringify(pageProfile) + "'" 
            + ' onclick="somain.showTab(this)"><i class="fa fa-user fa-lg"></i> Profile</a></li>')
        .appendTo($('#menu-content'));
    }

    somain.showTab = function (link) {
        var pageObj = $(link).data("page");
        var pageName = pageObj.name;
        var pageTitle = pageObj.title;
        var pageUrl = pageObj.url;
        
		//make the current tab actived
		somain.activeTabName = pageName;

        if (pageName === "mydashboard") {
            $('#myTab a:first').tab('show');
            return;
        }

        var tabName = pageName + "Tab_";
		var tab = $("a[href='#" + tabName + "'");

		if (tab.length === 0) {
            //create new tab
			$('#myTab').append(
			    $('<li><a href="#' + tabName + '">' +
			    pageTitle +
			    '<button class="close" type="button" ' +
			    'title="Remove this page"> ×</button>' +
			    '</a></li>'));

			var newTabContent = $('<div class="tab-pane" style="height:700px;width:100%;margin-top:10px;" id="' + tabName 
                    + '" data-page="' + pageUrl + '"></div>')
				.appendTo("#divTabContentContainer");

            somain.activeTabContent = newTabContent;

            //load page
            refreshTabContent(somain.activeTabContent);

			$('#myTab a:last').tab('show');
		} else {
            somain.activeTabContent = $("#" + tabName);

			//tab already exist
			tab.tab('show');
		}
	}

    //refresh page
    function refreshTabContent(tabContent){
        somain.displayProgressBar(true);
        var url = $(tabContent).data("page");       //page url from database

        //reload the page
        $(tabContent).empty();
        $(tabContent).load(url, function(){
            somain.displayProgressBar(false);
        });
    }

	//#region init, button
	somain.initPage = function () {
		//show tab
		$("#myTab").on("click", "a", function (e) {
			e.preventDefault();
			$(this).tab('show');
		});

		//remove tab
		$('#myTab').on('click', ' li a .close', function () {
			var tabId = $(this).parents('li').children('a').attr('href');

			$(this).parents('li').remove('li');
			$('#myTab a:first').tab('show');
		});

		$("#myTab").tab();
	}
	//#endregion

   
	function datetimeFormatter(row, cell, value, columnDef, dataContext) {
		if (value != null && value != "") {
			return value.substring(0, 10);
		}
	}
	//#endregion

	//#region toolbutton
	somain.addrecord = function (pageUrl) {
		somain.activeToolButtonType = "add";
		openDialogForm(somain.activeToolButtonType, pageUrl);
	}

	somain.editrecord = function (pageUrl) {
		somain.activeToolButtonType = "edit";
		openDialogForm(somain.activeToolButtonType, pageUrl);
	}

	somain.deleterecord = function (action) {
		somain.activeToolButtonType = "delete";
        executeCustomCommand(somain.activeToolButtonType, action);
	}

    somain.refreshrecord = function (){
        somain.activeToolButtonType = "refresh";
        refreshTabContent(somain.activeTabContent);
    }

	function openDialogForm(buttonType, url) {
        if (url === ""){
            $.msgBox({
				title: "SlickSafe / Page",
				content: "指向页面URL字段不存在，请再次确认！",
				type: "alert"
			});
            return false;
        }

        $.ajax({
            type: 'HEAD',
            url: url,
            statusCode: {
                404: function() {
                  	$.msgBox({
					    title: "SlickSafe / Page",
					    content: "页面不存在！Error:404 Page Not Found.",
					    type: "alert"
				    });
                }
            },
            success: function() {
                // page exists
                somain.displayProgressBar(true);
			    BootstrapDialog.show({
				    title: buttonType.toUpperCase(),
				    message: $('<div></div>').load(url, function(){
                        somain.displayProgressBar(false);
                    })
			    });
            },
            error: function (){
                ;
            }
        });
	}

    function executeCustomCommand(buttonType, action){
        if (action === undefined || action === ""){
            var errorMsg = "您没有权限操作，或者程序方法[{0})]未定义，请联系管理员确认！";
            $.msgBox({
				title: "SlickSafe / Page",
				content: errorMsg.formatUnicorn(buttonType),
				type: "alert"
			});
            return false;
        }

        jshelper.executeFunctionByName(action, window);
    }
	//#endregion

    somain.displayProgressBar = function(visible){
        if (visible === true)
            $('#loading-indicator').show();
        else 
            $('#loading-indicator').hide();
    }

    //#region user permission check
    somain.checkUserPermission = function(){
        //get page action resource collection
        var resourceID = somain.activeTabName.substring(somain.activeTabName.lastIndexOf("-") + 1).toLowerCase();   
        var actionResource = getPageActionResourceIterativly(somain.userPermissionTreeView, resourceID); 

        //find action controls    
        var tabName = somain.activeTabName + "Tab_";       
        var controls = $("div#" + tabName).find("[data-action]");

        $.each(controls, function(i, item){
            var action = $(item).data("action");
            var isValid = checkActionValid(action, actionResource);
            if (isValid === false) {
                 $(item).removeClass("btn-info")
                        .addClass("btn-warning")
                        .attr("onclick","")
                        //.addClass("disableHref")
                        .bind("click", function(e){
                            e.preventDefault(); 
                            $.msgBox({
				                title: "SlickSafe / Page",
				                content: "您当前没有权限操作此功能！",
				                type: "alert"
			                });
                            //$(this).addClass("disabled");
                            return false; 
                        });
                        
            } 
        });
    }

    function getPageActionResourceIterativly(menuData, resourceID){
         var children = null;
         $.each(menuData, function(i, item){
            if (item.ID === parseInt(resourceID)){
                children = item.children;
                if (children !== null) return false;                            
            }

            if (item.children && item.children.length > 0){
                children = getPageActionResourceIterativly(item.children, resourceID);
                if (children !== null) return false;
            }

            if (children !== null) return false;
        });
        return children;
    }

    function checkActionValid(action, actionResource){
        var isValid = false;
        $.each(actionResource, function(i, item){
            if (item.DataAction === action){
                isValid = true;
                return false;
            }
        });
        return isValid;
    }
    //#endregion

	return somain;
})();