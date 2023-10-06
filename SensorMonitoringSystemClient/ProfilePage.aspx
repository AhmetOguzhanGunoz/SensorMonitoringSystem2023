<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProfilePage.aspx.cs" Inherits="ProfilePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="ClientStyle.css" rel="stylesheet" />
    <title>Choose Sensor to Analyze over Sensor Monitoring System Site</title>   
</head>
    <script type="text/javascript">
        function DisableRequestButtonsandShowLabeltobeReadTimer()
        {
            var ChangeCodeButtonTimeLeft = 4;
            var LabelMessageTimeLeft = 4;

            var ChangeCodeButton = document.getElementById("<%=Changecodebtn.ClientID%>");
            var Changelbl = document.getElementById("<%=Changelbl.ClientID%>");

            var AnalyzeButton = document.getElementById("<%=Analyzebtn.ClientID%>");
            var DeleteSensorButton = document.getElementById("<%=DeleteSensorbtn.ClientID%>");
            var AddSensorButton = document.getElementById("<%=AddSensorbtn.ClientID%>");
            var SensorAddresslbl = document.getElementById("<%=SensorAddresslbl.ClientID%>");

            var Checklist = document.getElementById("<%=RegisteredUsersChklist.ClientID%>");
            var ApproveButton = document.getElementById("<%=ApproveBtn.ClientID%>");
            var RefuseButton = document.getElementById("<%=RefuseBtn.ClientID%>");
            var DeleteUserButton = document.getElementById("<%=DeleteUserBtn.ClientID%>");
            var RegisteredUserslbl = document.getElementById("<%=RegisteredUserslbl.ClientID%>");

            var Sensorsddl = document.getElementById("<%=Sensorsddl.ClientID%>");

            var Profilearea2 = document.getElementById("<%=Profilearea2.ClientID%>");
            var Profilearea3 = document.getElementById("<%=Profilearea3.ClientID%>");

            AnalyzeButton.disabled = true;
            DeleteSensorButton.disabled = true;            
            DeleteUserButton.disabled = true;
            ChangeCodeButton.disabled = true;
            ApproveButton.disabled = true;
            RefuseButton.disabled = true;

            if (DeleteUserButton.value == "In Deleting Process (Click to Return Approve)")
            {
                RefuseButton.value = "Delete User/s";
            }

            if (Changelbl.innerHTML !== "")
            {
                var LabelMessageTimer = setInterval(function () {
                    if (LabelMessageTimeLeft <= 0) {
                        clearInterval(LabelMessageTimer);
                        var ChangeCodetimer = setInterval(function () {
                            Changelbl.innerHTML = "Please wait " + ChangeCodeButtonTimeLeft.toString() + " seconds for activation of change code button.";
                            if (Profilearea2.style.display != "none" && !Sensorsddl.disabled)
                            {
                                if (AddSensorButton.disabled)
                                {
                                    SensorAddresslbl.innerHTML = "Please wait " + ChangeCodeButtonTimeLeft.toString() + " seconds for activation of analyze button.";
                                }
                                else
                                {
                                    SensorAddresslbl.innerHTML = "Please wait " + ChangeCodeButtonTimeLeft.toString() + " seconds for activation of analyze and delete sensor button.";
                                }
                            }
                            if (RefuseButton.value == "Delete User/s")
                            {
                                RegisteredUserslbl.innerHTML = "Please wait " + ChangeCodeButtonTimeLeft.toString() + " seconds for activation of return approve and delete user/s button.";
                            }
                            else if (Profilearea3.style.display != "none")
                            {
                                if (Checklist.innerText != "No Record of Activated User")
                                {
                                    RegisteredUserslbl.innerHTML = "Please wait " + ChangeCodeButtonTimeLeft.toString() + " seconds for activation of obtain, approve and refuse code button.";
                                }
                                else
                                {
                                    RegisteredUserslbl.innerHTML = "Please wait " + ChangeCodeButtonTimeLeft.toString() + " seconds for activation of obtain button.";
                                }
                            }
                            if (ChangeCodeButtonTimeLeft <= 0) {
                                clearInterval(ChangeCodetimer);
                                ChangeCodeButton.disabled = false;
                                if (Profilearea2.style.display != "none" && !Sensorsddl.disabled)
                                {
                                    if (AddSensorButton.disabled)
                                    {
                                        AnalyzeButton.disabled = false;
                                    }
                                    else
                                    {
                                        DeleteSensorButton.disabled = false;
                                        AnalyzeButton.disabled = false;
                                    }
                                }
                                if (DeleteUserButton.value == "In Deleting Process (Click to Return Approve)") // Changing value code-behind if not changed
                                {
                                    RefuseButton.disabled = false;
                                    DeleteUserButton.disabled = false;
                                }
                                else if (Profilearea3.style.display != "none") //Checklist cannot detected for non HR&Company Owner Users, so passed with 2 if clauses but running at server didnt tried for chklistdivision
                                {                                               
                                    if (Checklist.innerText != "No Record of Activated User")
                                    {
                                        DeleteUserButton.disabled = false;
                                        ApproveButton.disabled = false;
                                        RefuseButton.disabled = false;
                                    }
                                    else
                                    {
                                        DeleteUserButton.disabled = false;
                                    }
                                }
                                Changelbl.innerHTML = "";
                                RegisteredUserslbl.innerHTML = "";
                                SensorAddresslbl.innerHTML = "";
                            }
                            ChangeCodeButtonTimeLeft--;
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
                        var ChangeCodetimer = setInterval(function () {
                            Changelbl.innerHTML = "Please wait " + ChangeCodeButtonTimeLeft.toString() + " seconds for activation of change code button.";
                            if (Profilearea2.style.display != "none" && !Sensorsddl.disabled)
                            {
                                if (AddSensorButton.disabled)
                                {
                                    SensorAddresslbl.innerHTML = "Please wait " + ChangeCodeButtonTimeLeft.toString() + " seconds for activation of analyze button.";
                                }
                                else
                                {
                                    SensorAddresslbl.innerHTML = "Please wait " + ChangeCodeButtonTimeLeft.toString() + " seconds for activation of analyze and delete sensor button.";
                                }
                            }
                            if (RefuseButton.value == "Delete User/s")
                            {
                                RegisteredUserslbl.innerHTML = "Please wait " + ChangeCodeButtonTimeLeft.toString() + " seconds for activation of return approve and delete user/s button.";
                            }
                            else if (Profilearea3.style.display != 'none')
                            {
                                if (Checklist.innerText != 'No Record of Activated User')
                                {
                                    RegisteredUserslbl.innerHTML = "Please wait " + ChangeCodeButtonTimeLeft.toString() + " seconds for activation of obtain, approve and refuse code button.";
                                }
                                else
                                {
                                    RegisteredUserslbl.innerHTML = "Please wait " + ChangeCodeButtonTimeLeft.toString() + " seconds for activation of obtain button.";
                                }
                            }
                            if (ChangeCodeButtonTimeLeft <= 0) {
                                clearInterval(ChangeCodetimer);
                                ChangeCodeButton.disabled = false;
                                if (Profilearea2.style.display != "none" && !Sensorsddl.disabled)
                                {
                                    if (AddSensorButton.disabled)
                                    {
                                        AnalyzeButton.disabled = false;
                                    }
                                    else
                                    {
                                        DeleteSensorButton.disabled = false;
                                        AnalyzeButton.disabled = false;
                                    }
                                }
                                if (DeleteUserButton.value == "In Deleting Process (Click to Return Approve)")
                                {
                                    RefuseButton.disabled = false;
                                    DeleteUserButton.disabled = false;
                                }
                                else if (Profilearea3.style.display != "none")
                                {
                                    if (Checklist.innerText != "No Record of Activated User")
                                    {
                                        DeleteUserButton.disabled = false;
                                        ApproveButton.disabled = false;
                                        RefuseButton.disabled = false;
                                    }
                                    else
                                    {
                                        DeleteUserButton.disabled = false;
                                    }
                                }
                                Changelbl.innerHTML = "";
                                RegisteredUserslbl.innerHTML = "";
                                SensorAddresslbl.innerHTML = "";
                            }
                            ChangeCodeButtonTimeLeft--;
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
        function confirmation()
        {
            return confirm("Are you sure to make this process?");
        }
</script>
<body onload="DisableRequestButtonsandShowLabeltobeReadTimer();">
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
                <br />
                <br />
                <br />
                <div id="Profilearea1" class="Profileareaclass" style="display:contents;">
                    <asp:Table CellPadding="0" CellSpacing="10" ID="ProfileTable1" runat="server">
                        <asp:TableRow>
                            <asp:TableCell Width="100%"><asp:Label ID="Dearlbl" runat="server" Text=""></asp:Label></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell Width="46%"><asp:Label ID="Infolbl" Width="100%" runat="server" Text="You can change your account activation code or your account password by clicking related buttons."></asp:Label></asp:TableCell>
                            <asp:TableCell Width="18%"><asp:Button ID="Changecodebtn" CssClass="buttonclass" style="font-size: calc(0.6vw + 0.4vh);" Width="100%" runat="server" Text="Change activation code" OnClick="Changecodebtn_Click" OnClientClick="return confirmation();"/></asp:TableCell>
                            <asp:TableCell Width="18%"><asp:Button ID="Changepasswordbtn" CssClass="buttonclass" style="font-size: calc(0.6vw + 0.4vh);" Width="100%" runat="server" Text="Change password" OnClick="Changepasswordbtn_Click" OnClientClick="Changepasswordbtn_Click"/></asp:TableCell>  
                            <asp:TableCell Width="18%"><asp:Button ID="Logoutbtn" CssClass="buttonclass" style="font-size: calc(0.6vw + 0.4vh);" Width="100%" runat="server" Text="Log Out" OnClick="Logoutbtn_Click" OnClientClick="Logoutbtn_Click"/></asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <br />
                    <asp:Label ID="Changelbl" runat="server" Text=""></asp:Label>
                </div>
                <br />
                <div id="Profilearea2" runat="server" class="Profileareaclass">
                    <asp:Label ID="Chooselbl" runat="server" Text="Choose Sensor to Analyze Sensor Datas"></asp:Label>
                    <br /> 
                    <asp:Table Width="100%" CellPadding="0" CellSpacing="10" ID="ProfileTable2" runat="server">
                        <asp:TableRow>
                            <asp:TableCell Width="25%"><asp:DropDownList ID="Sensorsddl" Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Sensorsddl_SelectedIndexChanged"></asp:DropDownList></asp:TableCell>
                            <asp:TableCell Width="25%"><asp:Button ID="Analyzebtn" CssClass="buttonclass" style="font-size: calc(0.6vw + 0.4vh);" Width="100%" runat="server" Text="Analyze" OnClick="Analyzebtn_Click"/></asp:TableCell>
                            <asp:TableCell Width="25%"><asp:Button ID="DeleteSensorbtn" CssClass="buttonclass" style="font-size: calc(0.6vw + 0.4vh);" Width="100%" runat="server" Text="Delete Sensor" OnClick="DeleteSensorbtn_Click" OnClientClick="return confirmation();"/></asp:TableCell>
                            <asp:TableCell Width="25%"><asp:Button ID="AddSensorbtn" CssClass="buttonclass" style="font-size: calc(0.6vw + 0.4vh);" Width="100%" runat="server" Text="Add Sensor" OnClick="AddSensorbtn_Click" OnClientClick="AddSensorbtn_Click" /></asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <br />
                    <asp:Label ID="SensorAddresslbl" runat="server" Text=""></asp:Label>
                    <br />                   
                </div>
                <br />
                <div id="Profilearea3" runat="server" class="Profileareaclass">
                    <div id="UserLabelArea" runat="server">
                        <asp:Table ID="UserLblTable1" Width="100%" runat="server">
                            <asp:TableRow>
                                <asp:TableCell Width="50%"><asp:Label ID="Userslbl" runat="server">Registered and Activated Users</asp:Label></asp:TableCell>
                                <asp:TableCell Width="50%"><asp:Label ID="UserTypeslbl" runat="server">User Types</asp:Label></asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>                       
                    </div>
                    <div id="UsersArea">
                        <asp:CheckBoxList ID="RegisteredUsersChklist" runat="server" style="text-align:left;">
                        </asp:CheckBoxList>
                    </div>
                    <div id="UsersTypeArea">
                        <asp:RadioButtonList ID="UserTypeRadlist" runat="server" style="text-align:left;">
                        </asp:RadioButtonList>
                    </div>
                    <div id="SelectionArea">
                        <asp:Table Width="100%" CellPadding="0" CellSpacing="10" ID="ProfileTable3" runat="server" style="align-self:center;">
                            <asp:TableRow>
                                <asp:TableCell Width="30%"><asp:Button ID="DeleteUserBtn" CssClass="buttonclass" style="font-size: calc(0.6vw + 0.4vh);" Width="100%" runat="server" Text="Obtain Approved Users For Delete" OnClick="DeleteUserBtn_Click"/></asp:TableCell>
                                <asp:TableCell Width="30%"><asp:Button ID="ApproveBtn" CssClass="buttonclass" style="font-size: calc(0.6vw + 0.4vh);" Width="100%" runat="server" Text="Approve Selected" OnClick="ApproveBtn_Click" OnClientClick="return confirmation();"/></asp:TableCell>
                                <asp:TableCell Width="30%"><asp:Button ID="RefuseBtn" CssClass="buttonclass" style="font-size: calc(0.6vw + 0.4vh);" Width="100%" runat="server" Text="Refuse Selected" OnClick="RefuseBtn_Click" OnClientClick="return confirmation();"/></asp:TableCell>  
                            </asp:TableRow>
                        </asp:Table>                        
                    </div>
                    <asp:Label ID="RegisteredUserslbl" runat="server" Text=""></asp:Label>
                    <br />
                    <br />
                    <br />
                </div>
            </div>
            <div id="footer">
                <p>TechExpert Information Technologies and Automation Engineering Trade Limited Company <br/> 
                    This site was made by Ahmet Oğuzhan Günöz - Copyright © 2017 - Modified 2023 - All Rights Reserved</p>
            </div>
    </form>
</body>
</html>
