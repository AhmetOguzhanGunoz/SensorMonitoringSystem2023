<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ActivationPage.aspx.cs" Inherits="ActivationPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="ClientStyle.css" rel="stylesheet" />
    <title>Activate Account over Sensor Monitoring System Site</title>
</head>
    <script type="text/javascript">
        function DisableResendandActivationButtonandShowLabeltobeReadTimer()
        {
            var ResendandActivationButtonTimeLeft = 10;
            var LabelMessageTimeLeft = 4;

            var ResendButton = document.getElementById("<%=Resendbtn.ClientID%>");         
            ResendButton.disabled = true;
            var ActivationButton = document.getElementById("<%=Activationbtn.ClientID%>");
            ActivationButton.disabled = true;

            var Activationlbl = document.getElementById("<%=Activationlbl.ClientID%>");

            if (Activationlbl.innerHTML !== "")
            {
                var LabelMessageTimer = setInterval(function () {
                    if (LabelMessageTimeLeft <= 0) {
                        clearInterval(LabelMessageTimer);
                        var ResendandActivationtimer = setInterval(function () {
                            Activationlbl.innerHTML = "Please wait " + ResendandActivationButtonTimeLeft.toString() + " seconds for enablement of resend and activation button.";
                            if (ResendandActivationButtonTimeLeft <= 0) {
                                clearInterval(ResendandActivationtimer);
                                ResendButton.disabled = false;
                                ActivationButton.disabled = false;
                                Activationlbl.innerHTML = "";
                            }
                            ResendandActivationButtonTimeLeft--;
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
                        var ResendandActivationtimer = setInterval(function () {
                            Activationlbl.innerHTML = "Please wait " + ResendandActivationButtonTimeLeft.toString() + " seconds for enablement of resend and activation button.";
                            if (ResendandActivationButtonTimeLeft <= 0) {
                                clearInterval(ResendandActivationtimer);
                                ResendButton.disabled = false;
                                ActivationButton.disabled = false;
                                Activationlbl.innerHTML = "";
                            }
                            ResendandActivationButtonTimeLeft--;
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
                function () {
                    Banner.style.opacity = 1;
                    Content.style.opacity = 1;
                    Footer.style.opacity = 1;
                    Loading.style.display = "none";
                }, 20000
            );
        }
    </script>
<body onload="DisableResendandActivationButtonandShowLabeltobeReadTimer();">
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
                <div id="Checkarea">
                    <asp:Table Width="70%" CellPadding="0" CellSpacing="10" ID="CheckInfoTable" runat="server">
                        <asp:TableRow>
                                <asp:TableCell Width="10%"><asp:Label ID="UsernameRgslbl" Width="100%" runat="server" Text="Username:"></asp:Label></asp:TableCell>
                                <asp:TableCell Width="40%"><asp:TextBox ID="UsernameRgstxt" CssClass="inputclass"  runat="server"></asp:TextBox></asp:TableCell>
                                <asp:TableCell Width="10%" ><asp:RequiredFieldValidator ID="UsernameRFV" runat="server" ErrorMessage="Username is required" ControlToValidate="UsernameRgstxt"></asp:RequiredFieldValidator></asp:TableCell>
                                <asp:TableCell Width="15%" ><asp:RegularExpressionValidator ID="UsernameREV1" runat="server" ErrorMessage="Username allows english letters and numbers only" Display="Dynamic" ControlToValidate="UsernameRgstxt" ValidationExpression="^[a-zA-Z0-9]+$"></asp:RegularExpressionValidator></asp:TableCell>
                                <asp:TableCell Width="25%" ><asp:RegularExpressionValidator ID="UsernameREV2" runat="server" ErrorMessage="Username requires letters between 5-30 long" Display="Dynamic" ControlToValidate="UsernameRgstxt" ValidationExpression="^.{5,30}$"></asp:RegularExpressionValidator></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                                <asp:TableCell Width="10%"><asp:Label ID="Codelbl" Width="100%" runat="server" Text="Activation Code:"></asp:Label></asp:TableCell>
                                <asp:TableCell Width="40%"><asp:TextBox ID="Codetxt" CssClass="inputclass" runat="server" MaxLength="6"></asp:TextBox></asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <br />
                    <asp:Button ID="Resendbtn" CssClass="buttonclass" Width="20%" runat="server" Text="Re-send Code" OnClick="Resendbtn_Click1"/>                    
                    <br />
                    <br />
                    <asp:Button ID="Activationbtn" CssClass="buttonclass" Width="20%" runat="server" Text="Activate" OnClick="Activationbtn_Click"/>
                    <br />
                    <br />
                    <asp:Button ID="Backbtn" Width="24%" CssClass="buttonclass" runat="server" Text="Back to Welcome Page" OnClick="Backbtn_Click" OnClientClick="Backbtn_Click" />
                    <br />
                    <br />
                    <asp:Label ID="Activationlbl" Width="100%" runat="server" Text=""></asp:Label>
                </div>
                                                                                                                                                                                           
            </div>
            <div id="footer">
                <p>TechExpert Information Technologies and Automation Engineering Trade Limited Company <br/> 
                    This site was made by Ahmet Oğuzhan Günöz - Copyright © 2017 - Modified 2023 - All Rights Reserved</p>
            </div>
    </form>
</body>
</html>
