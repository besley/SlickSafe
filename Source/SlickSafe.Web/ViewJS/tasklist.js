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

var tasklist = (function () {
	function tasklist() {
	}

	//#region Task List
	function getTaskList() {
		somain.displayProgressBar(true);

		jshelper.ajaxGet('api/WfData/GetTaskList', null, function (result) {
			if (result.Status === 1) {
				var divTaskGrid = document.querySelector('#myTaskGrid');
				$(divTaskGrid).empty();

				var gridOptions = {
					columnDefs: [
						{ headerName: "ID", field: "ID", width: 40, cssClass: "bg-gray" },
						{ headerName: "应用名称", field: "AppName", width: 120, cssClass: "bg-gray" },
						{ headerName: "状态", field: "TaskState", width: 40, cssClass: "bg-gray" },
						{ headerName: "类型", field: "TaskType", width: 40, cssClass: "bg-gray" },
						{ headerName: "分配用户", field: "AssignedToUserName", width: 120, cssClass: "bg-gray" },
						{
							headerName: "创建日期", field: "CreatedDateTime", width: 120, cssClass: "bg-gray",							
						},
						{ headerName: "创建用户", field: "CreatedByUserName", width: 60, cssClass: "bg-gray" },
						{
							headerName: "完成日期", field: "EndedDateTime", width: 120, cssClass: "bg-gray",
						},
						{ headerName: "完成用户", field: "EndedByUserName", width: 60, cssClass: "bg-gray" },
					],
					rowSelection: 'single',
					onSelectionChanged: onSelectionChanged
				}

				new agGrid.Grid(divTaskGrid, gridOptions);
				gridOptions.api.setRowData(result.Entity);

				function onSelectionChanged() {
					var selectedRows = gridOptions.api.getSelectedRows();
					var selectedProcessID = 0;
					selectedRows.forEach(function (selectedRow, index) {
						userlist.pselectedTaskID = selectedRow.ID;
						userlist.pselectedTaskDataRow = selectedRow;
					});
				}

				somain.displayProgressBar(false);
			} else {
				$.msgBox({
					title: "Task / List",
					content: "读取任务记录失败！错误信息：" + result.Message,
					type: "error"
				});
			}
		});
	}

	return tasklist;
})()
