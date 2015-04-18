<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="MfiWebSuite.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>MFI Web Suite - dMatrix</title>
    <link href="/Bootstrap/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/login.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .temp
        {
            background-color: #F5F7FA;
            border: 1px solid #DEE5EF;
            border-radius: 3px 3px 3px 3px;
            float: left;
            margin-left: 49px;
            margin-top: 14px;
            padding: 5px;
            width: 268px;
        }
    </style>
</head>
<body>
    <form class="login-form" id="frmLogin" runat="server" action="login.aspx" autocomplete="off">
    <div class="login-wrap">
        <div class="login-box-bg">
            <div class="login-box">
                <div class="login-left">
                </div>
                <div class="login-right">
                    <div class="login-comp-head">
                        <div class="login-comp-logo">
                        </div>
                        <div class="login-comp-title">
                            MFI Web Suite</div>
                    </div>
                    <div class="login-right-contents">
                        <div class="login-glo-err" id="divLoginErr" runat="server">
                        </div>
                        <div class="login-cred-wrap">
                            <div class="ctrlwrap">
                                <div class="ctrl">
                                    <input type="text" id="txtEmailID" name="txtEmailID" runat="server" class="txt" placeholder="Email ID" />
                                </div>
                                <div class="err">
                                    <span id="errEmail"></span>
                                </div>
                            </div>
                            <div class="ctrlwrap">
                                <div class="ctrl">
                                    <input type="password" id="txtPassword" name="txtPassword" runat="server" class="txt"
                                        placeholder="Password" />
                                </div>
                                <div class="err">
                                    <span id="errPwd"></span>
                                </div>
                            </div>
                            
                            <div class="ctrlwrap">
                                <button type="submit" class="btn btn-success loginbtn" autocomplete="off" data-loading-text="<i class='icon-refresh icon-white'></i>&nbsp;Logging.."
                                    id="btnMwsLogin" runat="server" onclick="if (fnVal())" onserverclick="btnMwsLogin_Click">
                                    Login <i class="icon-chevron-right icon-white"></i>
                                </button>
                            </div>
                        </div>
                    </div>
                    <div class="temp">
                        <button class='close' data-dismiss='alert' type='button'>
                            ×</button>
                        <div>
                            Login simulations (only for testing!)
                        </div>
                        <%--<a href="#" id="testAcc">Accountant</a>&nbsp;&nbsp; <a href="#" id="testCeo">Ceo</a>&nbsp;&nbsp;--%>
                        <a href="#" id="testFE">FE1</a>&nbsp;&nbsp; 
                        <a href="#" id="testFE2">FE2</a>&nbsp;&nbsp; 
                        <a href="#" id="testDev">Developer</a>
                    </div>
                </div>
            </div>
        </div>
        <input type="hidden" id="hdnLUK" />
    </div>
    <div class="login-foot-wrap">
        <div class="login-footer">
            <%--<div class="left">
                &#169;2012 <a href="http://www.dmatrix.org" target="_blank"><strong>dMatrix Development
                    Foundation</strong> </a>
            </div>--%>
            <div class="right">
                Powered by <a href="http://www.epoweri.com" target="_blank"><strong>E power I</strong></a>
            </div>
        </div>
    </div>
    </form>    
    <script type="text/javascript" src="/JQuery/js/jquery-1.7.2.min.js"></script>
    <script src="/Bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="/Scripts/validate.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {

            //TEST-------------------------


            $("#testDev").click(function () {
                $("#txtEmailID").val("dev@me.com");
                $("#txtPassword").val("123456");
            });
                    
            $("#testFE").click(function () {
                $("#txtEmailID").val("fe1@dmatrix.com");
                $("#txtPassword").val("123456");
            });

            $("#testFE2").click(function () {
                $("#txtEmailID").val("fe2@dmatrix.com");
                $("#txtPassword").val("123456");
            });

            //TEST-------------------------

            $("#frmLogin").validate({
                ignore: "",
                rules: {
                    txtEmailID: {
                        required: true,
                        email: true
                    },
                    txtPassword: {
                        required: true
                    }
                }, //rules

                errorPlacement: function (error, element) {
                    if (element.attr("name") == "txtEmailID")
                        error.appendTo("#errEmail");
                    if (element.attr("name") == "txtPassword")
                        error.appendTo("#errPwd");
                },
                submitHandler: function (form) {

                },

                invalidHandler: function (form, validator) {

                }
            }); //validate()

        });   //doc ready

        function fnVal() {
            $("#divLoginErr").html("");

            if ($("#frmLogin").valid()) {
                $("#btnMwsLogin").button('loading');
                return true;
            }
            else {
                return false;
            }
        }
        
    </script>
</body>
</html>
