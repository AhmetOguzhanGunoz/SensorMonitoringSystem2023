<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForgotPassword.aspx.cs" Inherits="ForgotPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="ClientStyle.css" rel="stylesheet" />
    <title>Retrieve Password From Sensor Monitoring System Site</title>
</head>
    <script type="text/javascript">
        function DisableCheckandSubmitButtonandShowLabeltobeReadTimer()
        {
            var CheckButton = document.getElementById("<%=Checkbtn.ClientID%>");         
            CheckButton.disabled = true;
            var CheckButtonTimeLeft = 10;
            var Checklbl = document.getElementById("<%=Checklbl.ClientID%>");
            var LabelMessageTimeLeft = 4;

            var InfoArea = document.getElementById('Infoarea');
            var NewPasswordlbl = document.getElementById("<%=NewPasswordlbl.ClientID%>");
            var NewPasswordtxt = document.getElementById("<%=NewPasswordtxt.ClientID%>");
            var NewPasswordRFV1 = document.getElementById("<%=NewPasswordRFV1.ClientID%>");
            var NewPasswordREV1 = document.getElementById("<%=NewPasswordREV1.ClientID%>");
            var NewPasswordREV2 = document.getElementById("<%=NewPasswordREV2.ClientID%>");

            var NewPasswordConfirmlbl = document.getElementById("<%=NewPasswordConfirmlbl.ClientID%>");
            var NewPasswordConfirmtxt = document.getElementById("<%=NewPasswordConfirmtxt.ClientID%>");
            var NewPasswordConfirmRFV1 = document.getElementById("<%=NewPasswordConfirmRFV1.ClientID%>");
            var NewPasswordConfirmREV1 = document.getElementById("<%=NewPasswordConfirmREV1.ClientID%>");
            var NewPasswordConfirmREV2 = document.getElementById("<%=NewPasswordConfirmREV2.ClientID%>");

            var SubmitButton = document.getElementById("<%=Submitbtn.ClientID%>");
            SubmitButton.disabled = true;
            var Successlbl = document.getElementById("<%=Successlbl.ClientID%>");

            Successlbl.enabled = false;
            NewPasswordConfirmREV2.enabled = false;
            NewPasswordConfirmREV1.enabled = false;
            NewPasswordConfirmRFV1.enabled = false;
            NewPasswordConfirmtxt.enabled = false;
            NewPasswordConfirmlbl.enabled = false;
            NewPasswordREV2.enabled = false;
            NewPasswordREV1.enabled = false;
            NewPasswordRFV1.enabled = false;
            NewPasswordtxt.enabled = false;
            NewPasswordlbl.enabled = false;
            InfoArea.enabled = false;
            InfoArea.style.visibility = "hidden";


            if (Checklbl.innerHTML !== "")
            {
                if (Checklbl.innerHTML == "User is found.")
                {
                    Successlbl.enabled = true;
                    NewPasswordConfirmREV2.enabled = true;
                    NewPasswordConfirmREV1.enabled = true;
                    NewPasswordConfirmRFV1.enabled = true;
                    NewPasswordConfirmtxt.enabled = true;
                    NewPasswordConfirmlbl.enabled = true;
                    NewPasswordREV2.enabled = true;
                    NewPasswordREV1.enabled = true;
                    NewPasswordRFV1.enabled = true;
                    NewPasswordtxt.enabled = true;
                    NewPasswordlbl.enabled = true;
                    InfoArea.enabled = true;
                    InfoArea.style.visibility = "visible";
                    var LabelMessageTimer = setInterval(function () {
                        if (LabelMessageTimeLeft <= 0) {
                            clearInterval(LabelMessageTimer);
                            var Checktimer = setInterval(function () {
                                Checklbl.innerHTML = "Please wait " + CheckButtonTimeLeft.toString() + " seconds for activation of check button.";
                                Successlbl.innerHTML = "Please wait " + CheckButtonTimeLeft.toString() + " seconds for activation of submit button.";
                                if (CheckButtonTimeLeft <= 0) {
                                    clearInterval(Checktimer);
                                    CheckButton.disabled = false;
                                    SubmitButton.disabled = false;
                                    Checklbl.innerHTML = "";
                                    Successlbl.innerHTML = "";
                                }
                                CheckButtonTimeLeft--;
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
                            var Checktimer = setInterval(function () {
                                Checklbl.innerHTML = "Please wait " + CheckButtonTimeLeft.toString() + " seconds for activation of check button.";
                                Successlbl.innerHTML = "Please wait " + CheckButtonTimeLeft.toString() + " seconds for activation of submit button.";
                                if (CheckButtonTimeLeft <= 0) {
                                    clearInterval(Checktimer);
                                    CheckButton.disabled = false;
                                    SubmitButton.disabled = false;
                                    Checklbl.innerHTML = "";
                                    Successlbl.innerHTML = "";
                                }
                                CheckButtonTimeLeft--;
                            }, 1000);
                        }
                        LabelMessageTimeLeft--;
                    }, 400);
                }
            }
            else
            {
                var Checktimer = setInterval(function () {
                    Checklbl.innerHTML = "Please wait " + CheckButtonTimeLeft.toString() + " seconds for activation of check button.";
                    Successlbl.innerHTML = "Please wait " + CheckButtonTimeLeft.toString() + " seconds for activation of submit button.";
                    if (CheckButtonTimeLeft <= 0) {
                        clearInterval(Checktimer);
                        CheckButton.disabled = false;
                        SubmitButton.disabled = false;
                        Checklbl.innerHTML = "";
                        Successlbl.innerHTML = "";
                    }
                    CheckButtonTimeLeft--;
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
<body onload="DisableCheckandSubmitButtonandShowLabeltobeReadTimer();">
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
                                <asp:TableCell Width="10%" ><asp:RequiredFieldValidator ID="CodeRFV" runat="server" ErrorMessage="Code is required" ControlToValidate="Codetxt"></asp:RequiredFieldValidator></asp:TableCell>
                                <asp:TableCell Width="15%" ><asp:RegularExpressionValidator ID="CodeREV1" runat="server" ErrorMessage="Code allows numbers only" Display="Dynamic" ControlToValidate="Codetxt" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator></asp:TableCell>
                                <asp:TableCell Width="25%" ><asp:RegularExpressionValidator ID="CodeREV2" runat="server" ErrorMessage="Code requires numbers in length 6" Display="Dynamic" ControlToValidate="Codetxt" ValidationExpression="^.{6,6}$"></asp:RegularExpressionValidator></asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <br />
                    <asp:Button ID="Checkbtn" CssClass="buttonclass" Width="20%" runat="server" Text="Check" OnClick="Checkbtn_Click"/>
                    <br />
                    <br />
                    <asp:Button ID="Backbtn" CssClass="buttonclass" Width="24%" runat="server" Text="Back to Welcome Page" OnClick="Backbtn_Click" OnClientClick="Backbtn_Click" />
                    <br />
                    <br />
                    <asp:Label ID="Checklbl" Width="100%" runat="server" Text=""></asp:Label>
                </div>

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