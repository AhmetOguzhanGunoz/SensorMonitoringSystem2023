<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SensorAddPage.aspx.cs" Inherits="SensorAddPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="ClientStyle.css" rel="stylesheet" />
    <title>Add Sensor to Sensor Monitoring System</title>   
</head>
        <script type="text/javascript">
        function DisableAddSensorButtonandShowLabeltobeReadTimer()
        {
            var Button = document.getElementById("<%=Submitbtn.ClientID%>");
            Button.disabled = true;
            var Successlbl = document.getElementById("<%=Successlbl.ClientID%>");
            var RegisterButtonTimeLeft = 10;
            var LabelMessageTimeLeft = 6;

            if (Successlbl.innerHTML !== "")
            {
                var LabelMessageTimer = setInterval(function () {
                    if (LabelMessageTimeLeft <= 0) {
                        clearInterval(LabelMessageTimer);
                        var RegisterTimer = setInterval(function () {
                            Successlbl.innerHTML = "Please wait " + RegisterButtonTimeLeft.toString() + " seconds for activation of register button.";
                            if (RegisterButtonTimeLeft <= 0) {
                                clearInterval(RegisterTimer);
                                Button.disabled = false;
                                Successlbl.innerHTML = "";
                            }
                            RegisterButtonTimeLeft--;
                        }, 1000);
                    }
                    LabelMessageTimeLeft--;
                }, 600);
            }
            else
            {
                var RegisterTimer = setInterval(function () {
                    Successlbl.innerHTML = "Please wait " + RegisterButtonTimeLeft.toString() + " seconds for activation of register button.";
                    if (RegisterButtonTimeLeft <= 0) {
                        clearInterval(RegisterTimer);
                        Button.disabled = false;
                        Successlbl.innerHTML = "";
                    }
                    RegisterButtonTimeLeft--;
                }, 1000);
            }
            }

            function SetMaxLength()
            {
                var MinValueText = document.getElementById("<%=MinValuetxt.ClientID%>");
                var MaxValueText = document.getElementById("<%=MaxValuetxt.ClientID%>");
                var LowCriticValueText = document.getElementById("<%=LowestCriticalValuetxt.ClientID%>");
                var HighCriticValueText = document.getElementById("<%=HighestCriticalValuetxt.ClientID%>");

                var minus = "-";

                MinValueText.onkeyup = function () {
                    if (MinValueText.value.indexOf(minus) != -1) { MinValueText.maxLength = 10; }
                }
                MaxValueText.onkeyup = function () {
                    if (MaxValueText.value.indexOf(minus) != -1) { MaxValueText.maxLength = 10; }
                }
                LowCriticValueText.onkeyup = function () {
                    if (LowCriticValueText.value.indexOf(minus) != -1) { LowCriticValueText.maxLength = 9; }
                }
                HighCriticValueText.onkeyup = function () {
                    if (HighCriticValueText.value.indexOf(minus) != -1) { HighCriticValueText.maxLength = 9; }
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
<body onload="DisableAddSensorButtonandShowLabeltobeReadTimer();">
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
                    <div id="signuparea">
                        <asp:Table CellPadding="0" CellSpacing="10" ID="RegisterTable" Height="100%" runat="server" >
                            <asp:TableRow>
                                <asp:TableCell Width="10%"><asp:Label ID="SensorDescriptionlbl"  Width="100%" runat="server" Text="Sensor Description:"></asp:Label></asp:TableCell>
                                <asp:TableCell Width="40%"><asp:TextBox ID="SensorDescriptiontxt" CssClass="inputclass" runat="server"></asp:TextBox></asp:TableCell>
                                <asp:TableCell Width="10%"><asp:RequiredFieldValidator ID="SensorDescriptionRFV" runat="server" ErrorMessage="Sensor description is required" ControlToValidate="SensorDescriptiontxt"></asp:RequiredFieldValidator></asp:TableCell>
                                <asp:TableCell Width="15%"><asp:RegularExpressionValidator ID="SensorDescriptionREV1" runat="server" ErrorMessage="Sensor description allows latin letters only" Display="Dynamic" ControlToValidate="SensorDescriptiontxt" ValidationExpression="^[a-zA-ZğüşöçıİĞÜŞÖÇ][a-zA-Z0-9ğüşöçıİĞÜŞÖÇ.:\-\/\s]*$"></asp:RegularExpressionValidator></asp:TableCell>
                                <asp:TableCell Width="25%"><asp:RegularExpressionValidator ID="SensorDescriptionREV2" runat="server" ErrorMessage="Sensor description requires letters between 4-30 long" Display="Dynamic" ControlToValidate="SensorDescriptiontxt" ValidationExpression="^.{4,30}$"></asp:RegularExpressionValidator></asp:TableCell>
                            </asp:TableRow> 
                            <asp:TableRow>
                                <asp:TableCell Width="10%"><asp:Label ID="Countrylbl" Width="100%" runat="server" Text="Country:"></asp:Label></asp:TableCell>
                                <asp:TableCell Width="40%"><asp:DropDownList ID="Countryddl" CssClass="inputclass" Style="width:calc(21vw - 10vh);" runat="server" OnSelectedIndexChanged="Countryddl_SelectedIndexChanged" AutoPostBack="true"><asp:ListItem Text="Select Country" Value="0"></asp:ListItem></asp:DropDownList></asp:TableCell>
                                <asp:TableCell Width="50%" ><asp:RequiredFieldValidator ID="CountryRFV" runat="server" ErrorMessage="Country is required" ControlToValidate="Countryddl" InitialValue="0"></asp:RequiredFieldValidator></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="10%"><asp:Label ID="Citylbl" Width="100%" runat="server" Text="City:"></asp:Label></asp:TableCell>
                                <asp:TableCell Width="40%"><asp:DropDownList ID="Cityddl" CssClass="inputclass" Style="width:calc(21vw - 10vh);" runat="server" OnSelectedIndexChanged="Cityddl_SelectedIndexChanged" AutoPostBack="true"><asp:ListItem Text="Select City" Value="0"></asp:ListItem></asp:DropDownList></asp:TableCell>
                                <asp:TableCell Width="50%" ><asp:RequiredFieldValidator ID="CityRFV" runat="server" ErrorMessage="City is required" ControlToValidate="Cityddl" InitialValue="0"></asp:RequiredFieldValidator></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="10%"><asp:Label ID="Districtlbl" Width="100%" runat="server" Text="District:"></asp:Label></asp:TableCell>
                                <asp:TableCell Width="40%"><asp:DropDownList ID="Districtddl" CssClass="inputclass" Style="width:calc(21vw - 10vh);" runat="server"><asp:ListItem Text="Select District" Value="0"></asp:ListItem></asp:DropDownList></asp:TableCell>
                                <asp:TableCell Width="50%" ><asp:RequiredFieldValidator ID="DistrictRFV" runat="server" ErrorMessage="District is required" ControlToValidate="Districtddl" InitialValue="0"></asp:RequiredFieldValidator></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="10%"><asp:Label ID="SensorAddresslbl" Width="100%" runat="server" Text="Sensor Adress:"></asp:Label></asp:TableCell>
                                <asp:TableCell Width="40%"><asp:TextBox ID="SensorAddresstxt" TextMode="MultiLine" Style="resize:none" CssClass="inputclass" runat="server"></asp:TextBox></asp:TableCell>
                                <asp:TableCell Width="10%" ><asp:RequiredFieldValidator ID="SensorAddressRFV" runat="server" ErrorMessage="Sensor address is required" ControlToValidate="SensorAddresstxt"></asp:RequiredFieldValidator></asp:TableCell>
                                <asp:TableCell Width="15%" ><asp:RegularExpressionValidator ID="SensorAddressREV1" runat="server" ErrorMessage="Sensor address is not proper" Display="Dynamic" ControlToValidate="SensorAddresstxt" ValidationExpression="^[a-zA-ZğüşöçıİĞÜŞÖÇ][a-zA-Z0-9ğüşöçıİĞÜŞÖÇ.:\-\/\s]*$"></asp:RegularExpressionValidator></asp:TableCell>
                                <asp:TableCell Width="25%" ><asp:RegularExpressionValidator ID="SensorAddressREV2" runat="server" ErrorMessage="Sensor address requires letters between 4-200 long" Display="Dynamic" ControlToValidate="SensorAddresstxt" ValidationExpression="^.{4,200}$"></asp:RegularExpressionValidator></asp:TableCell>
                            </asp:TableRow>                            
                            <asp:TableRow>
                                <asp:TableCell Width="10%"><asp:Label ID="MinValuelbl" Width="100%" runat="server" Text="Minimum Value:"></asp:Label></asp:TableCell>
                                <asp:TableCell Width="40%"><asp:TextBox ID="MinValuetxt" CssClass="inputclass" runat="server" MaxLength="9" onKeyUp="SetMaxLength()"></asp:TextBox></asp:TableCell>
                                <asp:TableCell Width="10%" ><asp:RequiredFieldValidator ID="MinValueRFV" runat="server" ErrorMessage="Minimum value is required" ControlToValidate="MinValuetxt"></asp:RequiredFieldValidator></asp:TableCell>
                                <asp:TableCell Width="40%" ><asp:RegularExpressionValidator ID="MinValueREV1" runat="server" ErrorMessage="Minimum value allows numbers only" Display="Dynamic" ControlToValidate="MinValuetxt" ValidationExpression="^[-]?[0-9]+$"></asp:RegularExpressionValidator></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="10%"><asp:Label ID="MaxValuelbl" Width="100%" runat="server" Text="Maximum Value:"></asp:Label></asp:TableCell>
                                <asp:TableCell Width="40%"><asp:TextBox ID="MaxValuetxt" CssClass="inputclass" runat="server" MaxLength="9" onKeyUp="SetMaxLength()"></asp:TextBox></asp:TableCell>
                                <asp:TableCell Width="10%" ><asp:RequiredFieldValidator ID="MaxValueRFV" runat="server" ErrorMessage="Maximum value is required" ControlToValidate="MaxValuetxt"></asp:RequiredFieldValidator></asp:TableCell>
                                <asp:TableCell Width="40%" ><asp:RegularExpressionValidator ID="MaxValueREV1" runat="server" ErrorMessage="Maximum value allows numbers only" Display="Dynamic" ControlToValidate="MaxValuetxt" ValidationExpression="^[-]?[0-9]+$"></asp:RegularExpressionValidator></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="10%"><asp:Label ID="LowestCriticalValuelbl" Width="100%" runat="server" Text="Lowest Critical Value (As Format integer ({5}) & decimal ({5},{2})):"></asp:Label></asp:TableCell>
                                <asp:TableCell Width="40%"><asp:TextBox ID="LowestCriticalValuetxt" CssClass="inputclass" runat="server" MaxLength="8" onKeyUp="SetMaxLength()"></asp:TextBox></asp:TableCell>
                                <asp:TableCell Width="10%" ><asp:RequiredFieldValidator ID="LowestCriticalValueRFV" runat="server" ErrorMessage="Lowest critical value is required" ControlToValidate="LowestCriticalValuetxt"></asp:RequiredFieldValidator></asp:TableCell>
                                <asp:TableCell Width="15%" ><asp:RegularExpressionValidator ID="LowestCriticalValuetxtREV1" runat="server" ErrorMessage="Lowest critical value allows numbers and decimals only" Display="Dynamic" ControlToValidate="LowestCriticalValuetxt" ValidationExpression="^[-]?[0-9,]+$"></asp:RegularExpressionValidator></asp:TableCell>
                                <asp:TableCell Width="25%" ><asp:RegularExpressionValidator ID="LowestCriticalValuetxtREV2" runat="server" ErrorMessage="Lowest critical value is not proper" Display="Dynamic" ControlToValidate="LowestCriticalValuetxt" ValidationExpression="^[-]?[0-9]{1,5}(?:[,][0-9]?[0-9])?$"></asp:RegularExpressionValidator></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="10%"><asp:Label ID="HighestCriticalValuelbl" Width="100%" runat="server" Text="Highest Critical Value (As Format integer ({5}) & decimal ({5},{2})):"></asp:Label></asp:TableCell>
                                <asp:TableCell Width="40%"><asp:TextBox ID="HighestCriticalValuetxt" CssClass="inputclass" runat="server" MaxLength="8" onKeyUp="SetMaxLength()"></asp:TextBox></asp:TableCell>
                                <asp:TableCell Width="10%" ><asp:RequiredFieldValidator ID="HighestCriticalValueRFV1" runat="server" ErrorMessage="Highest critical value is required" ControlToValidate="HighestCriticalValuetxt"></asp:RequiredFieldValidator></asp:TableCell>
                                <asp:TableCell Width="15%" ><asp:RegularExpressionValidator ID="HighestCriticalValueREV1" runat="server" ErrorMessage="Highest critical value allows numbers and decimals only" Display="Dynamic" ControlToValidate="HighestCriticalValuetxt" ValidationExpression="^[-]?[0-9,]+$"></asp:RegularExpressionValidator></asp:TableCell>
                                <asp:TableCell Width="25%" ><asp:RegularExpressionValidator ID="HighestCriticalValueREV2" runat="server" ErrorMessage="Highest critical value is not proper" Display="Dynamic" ControlToValidate="HighestCriticalValuetxt" ValidationExpression="^[-]?[0-9]{1,5}(?:[,][0-9]?[1-9])?$"></asp:RegularExpressionValidator></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="10%"><asp:Label ID="SensorUnitlbl" Width="100%" runat="server" Text="Sensor Unit:"></asp:Label></asp:TableCell>
                                <asp:TableCell Width="40%"><asp:TextBox ID="SensorUnittxt" CssClass="inputclass" runat="server" MaxLength="10"></asp:TextBox></asp:TableCell>
                                <asp:TableCell Width="10%" ><asp:RequiredFieldValidator ID="SensorUnitRFV" runat="server" ErrorMessage="Sensor unit is required" ControlToValidate="SensorUnittxt"></asp:RequiredFieldValidator></asp:TableCell>
                                <asp:TableCell Width="40%" ><asp:RegularExpressionValidator ID="SensorUnitREV1" runat="server" ErrorMessage="Sensor unit is not proper" Display="Dynamic" ControlToValidate="SensorUnittxt" ValidationExpression="^[^\n\r\t\0\s\b\f0-9]+$"></asp:RegularExpressionValidator></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="10%"><asp:Label ID="SensorSpecificlbl" Width="100%" runat="server" Text="Is this specific?:"></asp:Label></asp:TableCell>
                                <asp:TableCell Width="40%"><asp:RadioButtonList ID="SensorSpecificbtnlist" runat="server" style="text-align:left; width:calc(10vw - 6vh); margin-left:5%;" RepeatDirection="Horizontal"><asp:ListItem>Yes</asp:ListItem><asp:ListItem Selected="True">No</asp:ListItem></asp:RadioButtonList></asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                        <br />
                        <asp:Label ID="Successlbl" Width="100%" runat="server" Text=""></asp:Label>
                        <br />                        
                    </div>
                <br />
                <br />
                <asp:Button ID="Submitbtn" Width="20%" CssClass="buttonclass" runat="server" Text="Add Sensor" OnClick="Submitbtn_Click"/>
                <br />
                <br />
                <asp:Button ID="Backbtn" Width="24%" CssClass="buttonclass" runat="server" Text="Back to Profile Page" OnClick="Backbtn_Click" OnClientClick="Backbtn_Click" />
            </div>
            <div id="footer">
                <p>TechExpert Information Technologies and Automation Engineering Trade Limited Company <br/> 
                    This site was made by Ahmet Oğuzhan Günöz - Copyright © 2017 - Modified 2023 - All Rights Reserved</p>
            </div>
    </form>
</body>
</html>
