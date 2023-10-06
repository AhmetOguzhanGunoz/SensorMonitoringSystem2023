<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterPage.aspx.cs" Inherits="RegisterPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="ClientStyle.css" rel="stylesheet" />
    <title>Register to Sensor Monitoring System</title>   
</head>
    <script type="text/javascript">
        function DisableRegisterButtonandShowLabeltobeReadTimer()
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
<body onload="DisableRegisterButtonandShowLabeltobeReadTimer();">
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
                                <asp:TableCell Width="10%"><asp:Label ID="Namelbl"  Width="100%" runat="server" Text="Name:"></asp:Label></asp:TableCell>
                                <asp:TableCell Width="40%"><asp:TextBox ID="Nametxt" CssClass="inputclass" runat="server"></asp:TextBox></asp:TableCell>
                                <asp:TableCell Width="10%"><asp:RequiredFieldValidator ID="NameRFV" runat="server" ErrorMessage="Name is required" ControlToValidate="Nametxt"></asp:RequiredFieldValidator></asp:TableCell>
                                <asp:TableCell Width="15%"><asp:RegularExpressionValidator ID="NameREV1" runat="server" ErrorMessage="Name allows latin letters and two word combined only" Display="Dynamic" ControlToValidate="Nametxt" ValidationExpression="^[a-zA-ZğüşöçıİĞÜŞÖÇ]+(\s[a-zA-ZğüşöçıİĞÜŞÖÇ]+)?$"></asp:RegularExpressionValidator></asp:TableCell> <%-- Regex tek isim veya iki isim arada boşluklu ve Türkçe karakter alacak şekilde güncellendi.--%>
                                <asp:TableCell Width="25%"><asp:RegularExpressionValidator ID="NameREV2" runat="server" ErrorMessage="Name requires letters between 4-30 long" Display="Dynamic" ControlToValidate="Nametxt" ValidationExpression="^.{4,30}$"></asp:RegularExpressionValidator></asp:TableCell>
                            </asp:TableRow>                            
                            <asp:TableRow>
                                <asp:TableCell Width="10%"><asp:Label ID="Surnamelbl" Width="100%" runat="server" Text="Surname:"></asp:Label></asp:TableCell>
                                <asp:TableCell Width="40%"><asp:TextBox ID="Surnametxt" CssClass ="inputclass" runat="server"></asp:TextBox></asp:TableCell>
                                <asp:TableCell Width="10%" ><asp:RequiredFieldValidator ID="SurnameRFV" runat="server" ErrorMessage="Surname is required" ControlToValidate="Surnametxt"></asp:RequiredFieldValidator></asp:TableCell>
                                <asp:TableCell Width="15%" ><asp:RegularExpressionValidator ID="SurnameREV1" runat="server" ErrorMessage="Surname allows latin letters only" Display="Dynamic" ControlToValidate="Surnametxt" ValidationExpression="^[a-zA-ZğüşöçıİĞÜŞÖÇ]+$"></asp:RegularExpressionValidator></asp:TableCell> <%-- Türkçe karakter alacak şekilde güncellendi.--%>
                                <asp:TableCell Width="25%" ><asp:RegularExpressionValidator ID="SurnameREV2" runat="server" ErrorMessage="Surname requires letters between 4-30 long" Display="Dynamic" ControlToValidate="Surnametxt" ValidationExpression="^.{4,30}$"></asp:RegularExpressionValidator></asp:TableCell>
                                </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="10%"><asp:Label ID="UsernameRgslbl" Width="100%" runat="server" Text="Username:"></asp:Label></asp:TableCell>
                                <asp:TableCell Width="40%"><asp:TextBox ID="UsernameRgstxt" CssClass="inputclass"  runat="server"></asp:TextBox></asp:TableCell>
                                <asp:TableCell Width="10%" ><asp:RequiredFieldValidator ID="UsernameRFV" runat="server" ErrorMessage="Username is required" ControlToValidate="UsernameRgstxt"></asp:RequiredFieldValidator></asp:TableCell>
                                <asp:TableCell Width="15%" ><asp:RegularExpressionValidator ID="UsernameREV1" runat="server" ErrorMessage="Username allows english letters and numbers only" Display="Dynamic" ControlToValidate="UsernameRgstxt" ValidationExpression="^[a-zA-Z0-9]+$"></asp:RegularExpressionValidator></asp:TableCell>
                                <asp:TableCell Width="25%" ><asp:RegularExpressionValidator ID="UsernameREV2" runat="server" ErrorMessage="Username requires letters between 5-30 long" Display="Dynamic" ControlToValidate="UsernameRgstxt" ValidationExpression="^.{5,30}$"></asp:RegularExpressionValidator></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="10%"><asp:Label ID="PasswordRgslbl" Width="100%" runat="server" Text="Password:"></asp:Label></asp:TableCell>
                                <asp:TableCell Width="40%"><asp:TextBox ID="PasswordRgstxt" CssClass="inputclass" TextMode="Password" runat="server" oncopy="return false" onpaste="return false" oncut="return false" ondelete="return false" MaxLength="15"></asp:TextBox></asp:TableCell>
                                <asp:TableCell Width="10%" ><asp:RequiredFieldValidator ID="PasswordRFV" runat="server" ErrorMessage="Password is required" ControlToValidate="PasswordRgstxt"></asp:RequiredFieldValidator></asp:TableCell>
                                <asp:TableCell Width="15%" ><asp:RegularExpressionValidator ID="PasswordREV1" runat="server" ErrorMessage="Password requires at least one upper case English letter, at least one lower case English letter, at least one digit, at least one special character" Display="Dynamic" ControlToValidate="PasswordRgstxt" ValidationExpression="^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^*-.,]).{8,}$"></asp:RegularExpressionValidator></asp:TableCell>
                                <asp:TableCell Width="25%" ><asp:RegularExpressionValidator ID="PasswordREV2" runat="server" ErrorMessage="Password requires length 8-15 long" Display="Dynamic" ControlToValidate="PasswordRgstxt" ValidationExpression="^.{8,15}$"></asp:RegularExpressionValidator></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="10%"><asp:Label ID="Phonelbl" Width="100%" runat="server" Text="Phone Number:"></asp:Label></asp:TableCell>
                                <asp:TableCell Width="40%"><asp:TextBox ID="Phonetxt" CssClass="inputclass" runat="server" MaxLength="10"></asp:TextBox></asp:TableCell>
                                <asp:TableCell Width="10%" ><asp:RequiredFieldValidator ID="PhoneRFV" runat="server" ErrorMessage="Phone Number is required" ControlToValidate="Phonetxt"></asp:RequiredFieldValidator></asp:TableCell>
                                <asp:TableCell Width="15%" ><asp:RegularExpressionValidator ID="PhoneREV1" runat="server" ErrorMessage="Phone Number allows numbers only" Display="Dynamic" ControlToValidate="Phonetxt" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator></asp:TableCell>
                                <asp:TableCell Width="25%" ><asp:RegularExpressionValidator ID="PhoneREV2" runat="server" ErrorMessage="Phone Number requires numbers in length 10" Display="Dynamic" ControlToValidate="Phonetxt" ValidationExpression="^.{10,10}$"></asp:RegularExpressionValidator></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="10%"><asp:Label ID="Emaillbl" Width="100%" runat="server" Text="E-mail:"></asp:Label></asp:TableCell>
                                <asp:TableCell Width="40%"><asp:TextBox ID="Emailtxt" CssClass="inputclass" runat="server"></asp:TextBox></asp:TableCell>
                                <asp:TableCell Width="10%" ><asp:RequiredFieldValidator ID="EmailRFV" runat="server" ErrorMessage="E-mail is required" ControlToValidate="Emailtxt"></asp:RequiredFieldValidator></asp:TableCell>
                                <asp:TableCell Width="15%" ><asp:RegularExpressionValidator ID="EmailREV1" runat="server" ErrorMessage="E-mail is not proper" Display="Dynamic" ControlToValidate="Emailtxt" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"></asp:RegularExpressionValidator></asp:TableCell>
                                <asp:TableCell Width="25%" ><asp:RegularExpressionValidator ID="EmailREV2" runat="server" ErrorMessage="E-mail requires characters between 5-30 long" Display="Dynamic" ControlToValidate="Emailtxt" ValidationExpression="^.{5,30}$"></asp:RegularExpressionValidator></asp:TableCell>
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
                                <asp:TableCell Width="10%"><asp:Label ID="Addresslbl" Width="100%" runat="server" Text="Adress:"></asp:Label></asp:TableCell>
                                <asp:TableCell Width="40%"><asp:TextBox ID="Addresstxt" TextMode="MultiLine" Style="resize:none" CssClass="inputclass" runat="server"></asp:TextBox></asp:TableCell>
                                <asp:TableCell Width="10%" ><asp:RequiredFieldValidator ID="AddressRFV" runat="server" ErrorMessage="Address is required" ControlToValidate="Addresstxt"></asp:RequiredFieldValidator></asp:TableCell>
                                <asp:TableCell Width="15%" ><asp:RegularExpressionValidator ID="AddressREV1" runat="server" ErrorMessage="Address is not proper" Display="Dynamic" ControlToValidate="Addresstxt" ValidationExpression="^[a-zA-ZğüşöçıİĞÜŞÖÇ][a-zA-Z0-9ğüşöçıİĞÜŞÖÇ.:\-\/\s]*$"></asp:RegularExpressionValidator></asp:TableCell> <%-- Türkçe karakter alacak şekilde güncellendi.--%>
                                <asp:TableCell Width="25%" ><asp:RegularExpressionValidator ID="AddressREV2" runat="server" ErrorMessage="Address requires letters between 20-200 long" Display="Dynamic" ControlToValidate="Addresstxt" ValidationExpression="^.{20,200}$"></asp:RegularExpressionValidator></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="10%"><asp:Label ID="DOBlbl" Width="100%" runat="server" Text="Date of Birth:"></asp:Label></asp:TableCell>
                                <asp:TableCell Width="40%"><asp:TextBox ID="DOBtxt" TextMode="Date" CssClass="inputclass" Style="width:calc(21vw - 10.6vh);" runat="server"></asp:TextBox></asp:TableCell>
                                <asp:TableCell Width="50%" ><asp:RequiredFieldValidator ID="DOBRFV" runat="server" ErrorMessage="Date of Birth is required" ControlToValidate="DOBtxt"></asp:RequiredFieldValidator></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="10%"><asp:Label ID="Companylbl" Width="100%" runat="server" Text="Company:"></asp:Label></asp:TableCell>
                                <asp:TableCell Width="40%"><asp:DropDownList ID="Companyddl" CssClass="inputclass" Style="width:calc(21vw - 10vh);" runat="server"><asp:ListItem Text="Select Company" Value="0"></asp:ListItem></asp:DropDownList></asp:TableCell>
                                <asp:TableCell Width="50%" ><asp:RequiredFieldValidator ID="CompanyRFV" runat="server" ErrorMessage="Company is required" ControlToValidate="Companyddl" InitialValue="0"></asp:RequiredFieldValidator></asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                        <br />
                        <asp:Label ID="Successlbl" Width="100%" runat="server" Text=""></asp:Label>
                        <br />                        
                    </div>
                <br />
                <br />
                <asp:Button ID="Submitbtn" Width="20%" CssClass="buttonclass" runat="server" Text="Register" OnClick="Submitbtn_Click"/>
                <br />
                <br />
                <asp:Button ID="Backbtn" Width="24%" CssClass="buttonclass" runat="server" Text="Back to Welcome Page" OnClick="Backbtn_Click" OnClientClick="Backbtn_Click" />
            </div>
            <div id="footer">
                <p>TechExpert Information Technologies and Automation Engineering Trade Limited Company <br/> 
                    This site was made by Ahmet Oğuzhan Günöz - Copyright © 2017 - Modified 2023 - All Rights Reserved</p>
            </div>
    </form>
</body>
</html>
