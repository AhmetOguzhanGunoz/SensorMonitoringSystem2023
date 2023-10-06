<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="ChangePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="ClientStyle.css" rel="stylesheet" />
    <title>Change Password From Sensor Monitoring System Site</title>
</head>
    <script type="text/javascript">
        function DisableSubmitButtonandShowLabeltobeReadTimer()
        {
            var SubmitButtonTimeLeft = 4;
            var LabelMessageTimeLeft = 4;
            var SubmitButton = document.getElementById("<%=Submitbtn.ClientID%>");
            var Successlbl = document.getElementById("<%=Successlbl.ClientID%>");
            SubmitButton.disabled = true;

            if (Successlbl.innerHTML !== "")
            {
                var LabelMessageTimer = setInterval(function () {
                    if (LabelMessageTimeLeft <= 0) {
                        clearInterval(LabelMessageTimer);
                        var SubmitTimer = setInterval(function () {
                            Successlbl.innerHTML = "Please wait " + SubmitButtonTimeLeft.toString() + " seconds for activation of submit button.";
                            if (SubmitButtonTimeLeft <= 0) {
                                clearInterval(SubmitTimer);
                                SubmitButton.disabled = false;
                                Successlbl.innerHTML = "";
                            }
                            SubmitButtonTimeLeft--;
                        }, 1000);
                    }
                    LabelMessageTimeLeft--;
                }, 400);              
            }
            else
            {
                var SubmitTimer = setInterval(function () {
                    Successlbl.innerHTML = "Please wait " + SubmitButtonTimeLeft.toString() + " seconds for activation of submit button.";
                    if (SubmitButtonTimeLeft <= 0) {
                        clearInterval(SubmitTimer);
                        SubmitButton.disabled = false;
                        Successlbl.innerHTML = "";
                    }
                    SubmitButtonTimeLeft--;
                }, 1000);
            }
        }
        function LoadingScreen() {
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
<body onload="DisableSubmitButtonandShowLabeltobeReadTimer();">
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
                <div id="Infoarea" runat="server">
                    <asp:Table Width="70%" CellPadding="0" CellSpacing="10" ID="NewInfoTable" runat="server">
                        <asp:TableRow>
                                <asp:TableCell Width="10%"><asp:Label ID="NewPasswordlbl" Width="100%" runat="server" Text="New Password:"></asp:Label></asp:TableCell>
                                <asp:TableCell Width="40%"><asp:TextBox ID="NewPasswordtxt" CssClass="inputclass" TextMode="Password" runat="server" oncopy="return false" onpaste="return false" oncut="return false" ondelete="return false" MaxLength="15"></asp:TextBox></asp:TableCell>
                                <asp:TableCell Width="10%" ><asp:RequiredFieldValidator ID="NewPasswordRFV1" runat="server" ErrorMessage="Password is required" ControlToValidate="NewPasswordtxt"></asp:RequiredFieldValidator></asp:TableCell>
                                <asp:TableCell Width="15%" ><asp:RegularExpressionValidator ID="NewPasswordREV1" runat="server" ErrorMessage="Password requires at least one upper case English letter, at least one lower case English letter, at least one digit, at least one special character" Display="Dynamic" ControlToValidate="NewPasswordtxt" ValidationExpression="^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^*-.,]).{8,}$"></asp:RegularExpressionValidator></asp:TableCell>
                                <asp:TableCell Width="25%" ><asp:RegularExpressionValidator ID="NewPasswordREV2" runat="server" ErrorMessage="Password requires letters 8-15 long" Display="Dynamic" ControlToValidate="NewPasswordtxt" ValidationExpression="^.{8,15}$"></asp:RegularExpressionValidator></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                                <asp:TableCell Width="10%"><asp:Label ID="NewPasswordConfirmlbl" Width="100%" runat="server" Text="New Password Confirm:"></asp:Label></asp:TableCell>
                                <asp:TableCell Width="40%"><asp:TextBox ID="NewPasswordConfirmtxt" CssClass="inputclass" TextMode="Password" runat="server" oncopy="return false" onpaste="return false" oncut="return false" ondelete="return false" MaxLength="15"></asp:TextBox></asp:TableCell>
                                <asp:TableCell Width="10%" ><asp:RequiredFieldValidator ID="NewPasswordConfirmRFV1" runat="server" ErrorMessage="Password is required" ControlToValidate="NewPasswordConfirmtxt"></asp:RequiredFieldValidator></asp:TableCell>
                                <asp:TableCell Width="15%" ><asp:RegularExpressionValidator ID="NewPasswordConfirmREV1" runat="server" ErrorMessage="Password requires at least one upper case English letter, at least one lower case English letter, at least one digit, at least one special character" Display="Dynamic" ControlToValidate="NewPasswordConfirmtxt" ValidationExpression="^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^*-.,]).{8,}$"></asp:RegularExpressionValidator></asp:TableCell>
                                <asp:TableCell Width="25%" ><asp:RegularExpressionValidator ID="NewPasswordConfirmREV2" runat="server" ErrorMessage="Password requires length 8-15 long" Display="Dynamic" ControlToValidate="NewPasswordConfirmtxt" ValidationExpression="^.{8,15}$"></asp:RegularExpressionValidator></asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <br />
                    <asp:Button ID="Submitbtn" CssClass="buttonclass" Width="20%" runat="server" Text="Submit" OnClick="Submitbtn_Click"/>
                    <br />
                    <br />
                    <asp:Button ID="Backbtn" CssClass="buttonclass" Width="24%" runat="server" Text="Back to Profile Page" OnClick="Backbtn_Click" OnClientClick="Backbtn_Click" />     
                    <br />
                    <br />
                    <asp:Label ID="Successlbl" Width="100%" runat="server" Text=""></asp:Label>
              
                </div>                                                                                                                                                                           
            </div>
            <div id="footer">
                <p>TechExpert Information Technologies and Automation Engineering Trade Limited Company <br/> 
                    This site was made by Ahmet Oğuzhan Günöz - Copyright © 2017 - Modified 2023 - All Rights Reserved</p>
            </div>
    </form>
</body>
</html>
