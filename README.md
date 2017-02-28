# SlickSafe
===========

SlickSafe 企业级权限快速开发框架，技术体系描述如下：Bootstrap3/Mvc(WebApi)Dapper，AG-Grid/zTree优秀开源组件，Dapper针对MSSQL, MySQL, Oracle等多数据库的实现，丰富代码示例。


# SlickSafe V1.2.0 版本说明
===========

功能说明

1. 角色用户基本数据维护;
2. 角色授权/用户授权同时支持;
3. 用户登录身份信息，权限授权验证;
4. 用户菜单，页面权限信息检验和受控实现;
5. WebApi Ticket(票据)信息验证;


技术说明

基于MVC, BootStrap3, WebApi, Dapper的3层分布式架构开发框架，其技术特点是：

1.  采用Dapper微ORMapping框架，性能接近原生SQL；
2.  采用Repository模式；
3.  采用面向Interface接口编程规范；
4.  采用WebApi实现服务总线;
5.  前端Bootstrap3框架布局在线演示Demo实现；

* SlickSafe框架在线DEMO：

http://demo.slickflow.com/ssweb/

用户名密码：admin/123456, jack/123456

* QQ交流群：

331928998

* 快速入门指南：

http://www.cnblogs.com/slickflow/p/6478887.html



SlickSafe is a web based user authentication system. There are some new features have been 
implemenmted in the solution. 

1. user and role management module.
2. role and user permisison assignment both.
3. user login management and ticket write read feature.
4. left side menu and button authorization management.
5. webapi user ticket authentication feature.

The system  is designed for 3-tier distributed system, SOA based system, Repository pattern, POCO entity pattern, asp.net mvc/web api architecture. Some details can be described here:

1. The 3-tier distributed layer include: data access layer, buisiness logic layer and web presentation layer.
2. Using micro-ORMapping framework Dapper/DapperExtension for database operation.
3. Using Generic repository pattern to implement data access feature.
4. Using Asp.net MVC WebAPI to implement service layer, webapi is a restful style service, we make it
   to match different client include winform, web and mobile.
5. The IRepository class can be used to implement EF, NHerbinate framework which the user prefered to them.
6. MSSQL, MySQL, Oracle and other database supported by Dapper.
 
The SlickSafe.Web project would give you a full tutorial how to use the SlickSafe library and webapi to create a rich mvc web
application. Similarily, there are serveral key features to describe here:

1. Bootstrap3/Mvc(WebApi)/Dapper.
2. AG-Grid/zTree/Bootstrap-Dialog.
3. NavBar in top and left side.
4. Rich page demos in solution.


