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

var accountmgr = (function () {
    function accountmgr() {
    }

    //account init
    accountmgr.init = function() {
        if ($("#divLoginError").text().trim() != "") {
            $("#divLoginError").show();
        }

        $("#myLoginForm").bind('ajax:complete', function() {
            alert("ajax complete...");  
            if ($("#hdnWebLogonUserTicket").value() != ""){
                alert("ticket is here");
            }
            else 
                alert("not ticket");
        });
    }

    accountmgr.ended = function(){
        alert("ended...");
        $("#myLoginForm").bind('ajax:complete', function() {
            alert("ajax complete...");  
            if ($("#hdnWebLogonUserTicket").value() != ""){
                alert("ticket is here");
            }
            else 
                alert("not ticket");
        });
    }

    //new user register
    accountmgr.register = function() {
        var errMsg = "";
        var userName = $("#txtUserName").val();
        var password = $("#txtPassword").val();
        var passwordConfirmed = $("#txtPasswordConfirmed").val();
        var email = $("#txtEmail").val();

        if (userName == undefined || userName.length == 0) {
            errMsg = "用户名称不能为空，请重新输入！";
        }
        else if (userName.length < 4) {
            errMsg = "用户名称长度不能低于4个字符，请重新输入！";
        }
        else if (password == undefined || passwordConfirmed == undefined || password != passwordConfirmed) {
            errMsg = "请确认两次输入的密码是否一致！";
        }
        else if (password.length < 6) {
            errMsg = "密码长度不能小于6位，请重新输入！";
        }
        else if (email == undefined || email.length == 0 || !isEMail(email)) {
            errMsg = "邮箱不能为空或输入错误格式，请重新输入！";
        }

        if (errMsg != "") {
            $("#divRegisterError").text(errMsg);
            $("#divRegisterError").show();
            return false;
        }

        var userData = {
            "UserName": userName,
            "Password": password,
            "EMail": email
        };

        ajaxPost(lsm.getApiUrl('Account/Register'),
            JSON.stringify(userData),
            function (result) {
                if (result.status == 1) {
                    window.location = "~/User/Profile";
                }
                else {
                    $("#divRegisterError").text(result.Message);
                    $("#divRegisterError").show();
                }
            });
    }

    //email validation
    function isEMail(str) {
        var reg = /^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3}){1,2})$/;
        return reg.test(str);
    }

    accountmgr.sendEMail = function(email) {
        var errMsg = "";
        if (email == undefined || email.length == 0 || !isEMail(email)) {
            errMsg = "邮箱不能为空或输入错误格式，请重新输入！";
        }

        if (errMsg != "") {
            $("#divSendEMailError").text(errMsg);
            $("#divSendEMailError").show();
            return false;
        }

        ajaxPost("/Account/SendEMail",
            JSON.stringify({ "EMail": email }),
            function (result) {
                if (result.status == 1) {
                    window.location = "/Account/Login";
                }
                else {
                    $("#divSendEMailError").text(result.Message);
                    $("#divSendEMailError").show();
                }
            });
    }

    //validate image for website project
    accountmgr.loadImage = function() {
        $.ajax({
            type: "GET",
            url: "/CaptchaImage/Index",
            contentType: "image/GF",
            success: function (img) {
                var d = new Date();
                $("#imgCaptcha").attr("src", "/CaptchaImage/Index?" + d.getTime());
            }
        });
    }

    //change password
    accountmgr.changePassword = function() {
        var errMsg = "";
        var user = lsm.getUserIdentity();
        var id = user.ID;
        var userName = user.UserName;
        var OldPassword = $("#txtOldPassword").val().trim();
        var newPassword = $("#txtNewPassword").val().trim();
        var newPasswordConfirmed = $("#txtNewPasswordConfirm").val().trim();

        if (newPassword == undefined || newPassword.length < 6 || newPassword != newPasswordConfirmed) {
            errMsg = "密码不符合要求，请重新输入新密码，密码必须为6个以上字母或数字的组合"
        }

        var userData = {
            "UserID": id,
            "UserName": userName,
            "OldPassword": OldPassword,
            "NewPassword": newPasswordConfirmed
        };

        ajaxPost("/Account/Password",
            JSON.stringify(userData),
            function (result) {
                if (result.ID) {
                    var userObj = { "ID": result.ID, "UserName": result.UserName, "Ticket": result.Ticket };
                    lsm.saveUserIdentity(userObj);
                    window.location = "/User/Profile";
                }
                else {
                    $("#divPasswordError").text(result.Message);
                    $("#divPasswordError").show();
                }
            });
    }

    return accountmgr;
})()
