<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="MfiWebSuite.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>MFI Web Suite - dMatrix</title>
    <link href="/Bootstrap/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/login.css" rel="stylesheet" type="text/css" />
    
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
    <script src="Scripts/login.js"></script>
</body>
</html>
