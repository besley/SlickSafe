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


var loglist = (function(){
    function loglist(){
    }

    //#region log List
	loglist.getExceptionLogList = function() {
		jshelper.ajaxGet('api/LogData/GetExceptionList', null, function (result) {
			if (result.Status === 1) {
                var divLogGrid = document.querySelector('#myloggrid');
				$(divLogGrid).empty();

				var gridOptions = {
					columnDefs: [
						{ headerName: 'ID', field: 'ID', width: 50 },
						{ headerName: '类型', field: 'EventTypeID', width: 60 },
						{ headerName: '优先级', field: 'Priority', width: 60 },
						{ headerName: '紧急', field: 'Severity', width: 60 },
						{ headerName: '标题', field: 'Title', width: 160 },
						{ headerName: '信息', field: 'Message', width: 160 },
						{ headerName: '创建日期', field: 'Timestamp', width: 120 }
					],
					rowSelection: 'single',
					onSelectionChanged: onSelectionChanged,
				};

				new agGrid.Grid(divLogGrid, gridOptions);
				gridOptions.api.setRowData(result.Entity);

				function onSelectionChanged() {
					var selectedRows = gridOptions.api.getSelectedRows();
					selectedRows.forEach(function (selectedRow, index) {
					    loglist.pselectedExceptionLogID = selectedRow.ID;
						loglist.pselecteExceptionLogDataRow = selectedRow;
					});
				}
            } else {
            	$.msgBox({
            		title: "Log / List",
            		content: "读取异常日志记录失败！错误信息：" + result.Message,
            		type: "error"
            	});
            }
		});
	}

    loglist.getUserLogList = function() {
		jshelper.ajaxGet('api/LogData/GetUserLogPaged100', null, function (result) {
			if (result.Status === 1) {
                var divLogGrid = document.querySelector('#myuserloggrid');
				$(divLogGrid).empty();

				var gridOptions = {
					columnDefs: [
						{ headerName: 'ID', field: 'ID', width: 50 },
						{ headerName: '用户ID', field: 'UserID', width: 60 },
						{ headerName: '登录名称', field: 'LoginName', width: 60 },
						{ headerName: '登录时间', field: 'LoginTime', width: 160 },
						{ headerName: '退出时间', field: 'LogoutTime', width: 160 },
						{ headerName: 'IP地址', field: 'IPAddress', width: 120}
					],
					rowSelection: 'single',
					onSelectionChanged: onSelectionChanged,
				};

				new agGrid.Grid(divLogGrid, gridOptions);
				gridOptions.api.setRowData(result.Entity);

				function onSelectionChanged() {
					var selectedRows = gridOptions.api.getSelectedRows();
					selectedRows.forEach(function (selectedRow, index) {
						loglist.pselectedUserLogID = selectedRow.ID;
						loglist.pselecteUserLogDataRow = selectedRow;
					});
				}
            } else {
            	$.msgBox({
            		title: "Log / List",
            		content: "读取用户登录记录失败！错误信息：" + result.Message,
            		type: "error"
            	});
            }
		});
	}
	//#endregion

    return loglist;
})()