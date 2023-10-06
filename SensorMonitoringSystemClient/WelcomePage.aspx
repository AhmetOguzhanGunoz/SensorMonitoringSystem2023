<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WelcomePage.aspx.cs" Inherits="WelcomePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="ClientStyle.css" rel="stylesheet" />
    <title>Welcome to Sensor Monitoring System Site</title>   
</head>
    <script type="text/javascript">
        function DisableLoginButtonandShowLabeltobeReadTimer() {
            var LoginButtonTimeLeft = 10;
            var LabelMessageTimeLeft = 4;

            var Loginbtn = document.getElementById("<%=Loginbtn.ClientID%>");
            Loginbtn.disabled = true;

            var Checklbl = document.getElementById("<%=Checklbl.ClientID%>");

            if (Checklbl.innerHTML !== "") {
                var LabelMessageTimer = setInterval(function () {
                    if (LabelMessageTimeLeft <= 0) {
                        clearInterval(LabelMessageTimer);
                        var Logintimer = setInterval(function () {
                            Checklbl.innerHTML = "Please wait " + LoginButtonTimeLeft.toString() + " seconds for activation of login button.";
                            if (LoginButtonTimeLeft <= 0) {
                                clearInterval(Logintimer);
                                Loginbtn.disabled = false;
                                Checklbl.innerHTML = "";
                            }
                            LoginButtonTimeLeft--;
                        }, 1000);
                    }
                    LabelMessageTimeLeft--;
                }, 400);
            }
            else
            {
                var LabelMessageTimer = setInterval(function () {
                    if (LabelMessageTimeLeft <= 0) {
                        clearInterval(LabelMessageTimer);
                        var Logintimer = setInterval(function () {
                            Checklbl.innerHTML = "Please wait " + LoginButtonTimeLeft.toString() + " seconds for activation of login button.";
                            if (LoginButtonTimeLeft <= 0) {
                                clearInterval(Logintimer);
                                Loginbtn.disabled = false;
                                Checklbl.innerHTML = "";
                            }
                            LoginButtonTimeLeft--;
                        }, 1000);
                    }
                    LabelMessageTimeLeft--;
                }, 400);
            }
        }
        function LoadingScreen()
        {
            var Loading = document.getElementById('loading');
            var Banner = document.getElementById('banner');
            var Content = document.getElementById('content');
            var Footer = document.getElementById('footer');
            Banner.style.opacity = 0.5;
            Content.style.opacity = 0.5;
            Footer.style.opacity = 0.5;
            Loading.style.display = "block";
            setTimeout(
                function ()
                {
                    Banner.style.opacity = 1;
                    Content.style.opacity = 1;
                    Footer.style.opacity = 1;
                    Loading.style.display = "none";
                }, 20000
            );
        }
    </script>
<body onload="DisableLoginButtonandShowLabeltobeReadTimer();">
    <form id="form1" runat="server" onsubmit="LoadingScreen();">
        <div id="loading" style="display:none"></div>
            <div id="banner">
                <div id="bannerpart1">
                    <img src="logo.png" alt="techexpertlogo"/>
                </div>
                <div id="bannerpart2">
                    <h3>Sensor Monitoring System</h3>
                    <p>depends to trustworthy sensor datas</p>
                </div>
            </div>

            <div id="content">
                <div id="contentmiddle">
                        <asp:Table Width="100%" CellPadding="0" CellSpacing="10" ID="InputTable" runat="server">
                            <asp:TableRow>
                                <asp:TableCell Width="30%"><asp:Label ID="Usernamelbl" Width="100%" runat="server" Text="Username:"></asp:Label></asp:TableCell>
                                <asp:TableCell Width="70%"><asp:TextBox ID="Usernametxt" CssClass="inputclass" Width="100%" runat="server" MaxLength="30"></asp:TextBox></asp:TableCell>

                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell Width="30%"><asp:Label ID="Passwordlbl" Width="100%" runat="server" Text="Password:"></asp:Label></asp:TableCell>
                                <asp:TableCell Width="70%"><asp:TextBox ID="Passwordtxt" CssClass="inputclass" Width="100%" Textmode="Password" runat="server" oncopy="return false" onpaste="return false" oncut="return false" ondelete="return false" MaxLength="15"></asp:TextBox></asp:TableCell>
                           
                            </asp:TableRow>  
                         </asp:Table>
                    
                        <asp:Table CssClass="contentmargin" Width ="100%" CellPadding="0" CellSpacing="5" ID="ButtonTable" runat="server">
                            <asp:TableRow>
                                <asp:TableCell Width="50%"><asp:Button ID="Registerbtn" CssClass="buttonclass" Width="100%" runat="server" Text="Register" OnClick="Registerbtn_Click" /></asp:TableCell>

                                <asp:TableCell Width="50%"><asp:Button ID="Loginbtn" CssClass="buttonclass"  Width="100%" runat="server" Text="Login" OnClick="Loginbtn_Click"/></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="50%"><asp:Button ID="Forgotpassbtn" CssClass="buttonclass" Width="100%"  runat="server" Text="Forgot Password" OnClick="Forgotpassbtn_Click"/></asp:TableCell>

                                <asp:TableCell Width="50%"><asp:Button ID="Activationbtn" CssClass="buttonclass" Width="100%" runat="server" Text="Activate Account" OnClick="Activationbtn_Click" /></asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                                        
                    <br />
                    <br />
                    <asp:Label ID="Checklbl" Width="100%" runat="server" Text=""></asp:Label>
                </div>
            </div>
            <div id="footer">
                <p>TechExpert Information Technologies and Automation Engineering Trade Limited Company <br/> 
                    This site was made by Ahmet Oğuzhan Günöz - Copyright © 2017 - Modified 2023 - All Rights Reserved</p>
            </div>
    </form>
</body>
</html>
