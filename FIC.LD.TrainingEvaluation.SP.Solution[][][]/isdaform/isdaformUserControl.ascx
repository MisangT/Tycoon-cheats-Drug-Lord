<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="isdaformUserControl.ascx.cs" Inherits="FIC.LD.TrainingEvaluation.SP.Solution.isdaform.isdaformUserControl" %>

<link href="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/css/main.css" rel="stylesheet" />
<link href="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/css/jquery-ui.css" rel="stylesheet" />
<link href="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/css/datepicker.css" rel="stylesheet" />
<!--<link href="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/css/bootstrap.min.css" rel="stylesheet" />-->

<script src="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/scripts/main.js"></script>
<script src="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/scripts/jquery-3.2.1.js"></script>
<script src="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/scripts/jquery-ui.js"></script>
<script src="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/scripts/bootstrap.min.js"></script>
<script src="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/scripts/bootstrap-datepicker.js"></script>


<style type="text/css">
    .auto-style1 {
        height: 24px;
    }

    .auto-style2 {
        border-radius: 10px;
        font-weight: bold;
    }

    .auto-style4 {
        width: 166px;
    }

    .auto-style7 {
        width: 166px;
        height: 18px;
    }

    .auto-style8 {
        height: 21px;
    }

    .auto-style9 {
        height: 9px;
    }

    .auto-style10 {
        width: 465px;
    }

    .auto-style11 {
        width: 118px;
    }

    .auto-style12 {
        width: 5px;
    }

    .auto-style13 {
        background-color: #F2F2F2;
        font-family: Calibri;
        font-size: small;
        font-weight: bold;
        color: black;
        border: 1px solid #5A7E92;
        padding: 10px;
        width: 447px;
    }

    .auto-style14 {
        background-color: #F2F2F2;
        font-family: Calibri;
        font-size: small;
        font-weight: bold;
        color: black;
        border: 1px solid #5A7E92;
        padding: 10px;
        width: 1048px;
    }
</style>


<div id="tblISDA" class="GeneralFormat">
    <fieldset class="FieldSetFormat">
        <legend class="MainHeadingLegend">
            <label class="SectionThreeHeader">INDIVIDUAL SKILLS DEVELOPMENT APPROVAL FORM</label><br>
            <label class="BoldOrangeText">(To be completed by employee and line manager prior to attendance of learning)</label></legend>
        <div class="ContentsDIVFormat" runat="server" id="MainContent">

            <fieldset class="FieldSetFormat">
                <legend class="MainHeadingLegend">EMPLOYEE DETAILS</legend>
                <div>
                    <br />
                    <table>

                        <tr>
                            <td class="auto-style4">Employee Full Name&nbsp;</td>
                            <td>
                                <asp:TextBox ID="txtEmpFullName" runat="server" CssClass="inputTextBox" disabled="disabled" />
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style7"></td>

                        </tr>
                        <tr>
                            <td class="auto-style4">Designation</td>
                            <td>
                                <asp:TextBox ID="txtEmpDesignation" runat="server" CssClass="inputTextBox" disabled="disabled" />
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style4">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style4">Department\Business Unit&nbsp;</td>
                            <td>
                                <asp:TextBox ID="txtEmpDept" runat="server" CssClass="inputTextBox" disabled="disabled" />
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style4">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style4">Line Manager</td>
                            <td>
                                <SharePoint:PeopleEditor ID="ManagerEmailPeoplePicker" runat="server" MultiSelect="False" CssClass="inputTextbox" Width="402px" AllowTypeIn="true" AutoPostBack="false" SharePointGroup="FICLineManagers"></SharePoint:PeopleEditor>
                            </td>
                        </tr>
                    </table>
                    <br />
                </div>
            </fieldset>

            <br />
            <br />

            <fieldset class="DateLegendFormat">
                <legend class="MainHeadingLegend">DETAILS OF THE LEARNING INTERVENTION</legend>
                <br />
                <fieldset class="FieldSetFormat">
                    <legend class="SubHeadingLegend">Discussion with Line Manager</legend>
                    <div>
                        Please confirm if you have discussed the need to attend this learning intervention with your Line Manager.
                    </div>
                    <div>
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:RadioButtonList ID="DiscussedWithManagerRadioButtonList" runat="server" Width="211px">
                                        <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>

                    </div>
                </fieldset>

                <br />
                <br />

                <fieldset class="FieldSetFormat">
                    <legend class="SubHeadingLegend">Type of Training</legend>
                    <div id="TypeOfTrainingDIV">
                        <table>
                            <tr>
                                <td>(select where it is applicable)&nbsp;</td>
                                <td>
                                    <asp:DropDownList ID="drpdwnlstTypeOfTraining" runat="server" Width="211px" required="true" AutoPostBack="true" OnSelectedIndexChanged="drpdwnlstTypeOfTraining_SelectedIndexChanged">
                                        <asp:ListItem Value="Individual">Individual</asp:ListItem>
                                        <asp:ListItem Value="Generic" Text="Generic"></asp:ListItem>
                                        <asp:ListItem Value="Specialized" Text="Specialized"></asp:ListItem>
                                        <asp:ListItem Value="Professional" Text="Professional Membership"></asp:ListItem>
                                    </asp:DropDownList>

                                </td>
                            </tr>
                        </table>
                    </div>

                    <br />

                    <div id="IndividualDIV" runat="server">
                    </div>

                    <div id="GenericDIV" runat="server">
                    </div>

                    <table id="tblDisclaimer" visible="false" runat="server">
                        <tr>
                            <td>
                                <fieldset class="auto-style14">
                                    <legend class="SubHeadingLegend">Disclaimer</legend>
                                    <div id="DisclaimerDiv" runat="server">
                                        I accept that should my requested learning intervention cost the FIC an investment of R($$$) or more, I have an obligation to complete a Specialised Learning Contract and adhere to the prescribed conditions in line with the Specialised Learning Policy. Failure to do so may result in the cancellation of my learning request.
                                    </div>
                                    <br />

                                    <asp:RadioButtonList ID="DisclaimerRadioButtonList" AutoPostBack="true" runat="server" Width="211px" OnSelectedIndexChanged="DisclaimerRadioButtonList_SelectedIndexChanged">
                                        <asp:ListItem Value="1" Text="I Accept"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="I Decline"></asp:ListItem>
                                    </asp:RadioButtonList>

                                    <br />

                                    <table id="tblSpecializedContractDIV" visible="false" runat="server">
                                        <tr>
                                            <td>
                                                <fieldset class="auto-style13">
                                                    <legend class="SubHeadingLegend">Specialized Contract Required</legend>
                                                    <table>
                                                        <tr>
                                                            <td>Attach Specialized Contract &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:FileUpload ID="SpecializedContractFileUpload" runat="server" required />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
                                    </table>
                                    <div id="SpecializedContractDiv" runat="server" visible="false">
                                    </div>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                    </table>
                    <br />

                    <div id="ProfessionalDIV" runat="server">
                        <fieldset class="FieldSetFormat" id="ProfessionalSkillsForm" runat="server">
                            <legend class="SubHeadingLegend">Professional Skills Form Required</legend>
                            <div>
                                <table>
                                    <tr>
                                        <td>Attach Skills Form &nbsp;
                                        </td>
                                        <td>
                                            <asp:FileUpload ID="ProfessionalSkillFormFileUpload" runat="server" required />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <br />
                            <div id="ProfessionalSkillFormDiv" runat="server" visible="false">
                                <asp:LinkButton ID="LinkButton2" runat="server">LinkButton</asp:LinkButton>
                            </div>
                        </fieldset>
                    </div>

                </fieldset>

                <br />

                <fieldset class="FieldSetFormat">
                    <legend class="SubHeadingLegend">Development Need and Intervention</legend>
                    <div id="LDInterventionDIV">
                        <table>
                            <tr>
                                <td>What development need is intervention aimed to address? (select where it is applicable)&nbsp;</td>
                                <td>
                                    <asp:DropDownList ID="drpdwnlstDevelopmentNeeds" runat="server" Width="211px" required>
                                        <asp:ListItem Value="">Please Select Development Need</asp:ListItem>
                                        <asp:ListItem Value="Career Development">Career Development</asp:ListItem>
                                        <asp:ListItem Value="Job Enhancement">Job Enhancement</asp:ListItem>
                                        <asp:ListItem Value="Management Development">Management Development</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>Name of Learning Intervention</td>
                                <td>
                                    <asp:TextBox ID="txtLearningIntervention" runat="server" CssClass="inputTextBox" required />
                                </td>
                            </tr>
                        </table>
                    </div>

                </fieldset>

                <br />
                <br />

                <fieldset class="FieldSetFormat">
                    <legend class="SubHeadingLegend">
                        <label>Employee's Learning Expectations</label>
                    </legend>
                    <div>
                        <fieldset class="FieldSetFormat">
                            <legend class="SubHeadingLegend">Learning Objectives (Outline below)</legend>
                            <div>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:TextBox TextMode="MultiLine" ID="LearningObjectivesTextBox" CssClass="TextAreaFormat" runat="server" required></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>

                            </div>
                        </fieldset>
                        <br />
                    </div>
                </fieldset>

                <fieldset class="FieldSetFormat" id="WorkBackContract" runat="server" visible="false">
                    <legend class="SubHeadingLegend">
                        <label>Attach work-back contract here</label>
                    </legend>
                    <div>
                        <br />
                        <table>
                            <tr>
                                <td>Attached Work-back contract &nbsp;
                                </td>
                                <td>
                                    <asp:FileUpload ID="WorkBackFileUpload" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <div id="WorkbackContractDiv" runat="server" visible="false">
                    </div>
                </fieldset>

                <br />
                <br />

            </fieldset>

            <div id="ManagerStep2DIV" runat="server" visible="false">
                <fieldset class="FieldSetFormat">
                    <legend class="MainHeadingLegend">
                        <label>LINE MANANGER'S EXPECTATION</label><br />
                        <label>(This section should be completed by the Line Manager)</label>
                    </legend>
                    <div>
                        <fieldset class="FieldSetFormat">
                            <legend class="SubHeadingLegend">What performance improvement is expected from the employee after attending this learning intervention ?  (Outline below)</legend>
                            <div>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:TextBox TextMode="MultiLine" ID="txtPerformanceImprovements" CssClass="TextAreaFormat" runat="server" required></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>

                            </div>
                        </fieldset>
                        <br />
                    </div>
                </fieldset>

                <br />
                <br />

                <fieldset class="FieldSetFormat">
                    <legend class="MainHeadingLegend">
                        <label>LINE MANANGER'S APPROVAL</label><br />
                        <label>(This section should be completed by the Line Manager)</label>
                    </legend>
                    <div>
                        Was learning intervention identified on employee’s PDP?
                    </div>
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="FoundOnPDPDropDownList" runat="server" Width="211px" required>
                                        <asp:ListItem Value=""></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>

                        </table>
                        <br />
                    </div>
                    <div>
                        Please indicate if you approve or reject
                    </div>
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ManagerApproveRejectDropDownList" runat="server" Width="211px" required OnSelectedIndexChanged="ManagerApproveRejectDropDownList_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value=""></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Approve"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="Reject"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr id="ManagerRejectreasonTR" runat="server" visible="false">
                                <td>
                                    <table>
                                        <tr>
                                            <td style="vertical-align: top;">Reject Reason
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="ManagerRejectReasonTextbox" TextMode="MultiLine" runat="server" required class="TextAreaFormat"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </fieldset>

                <br />
                <br />
            </div>

            <div id="LDManagerStep3DIV" runat="server" visible="false">

                <fieldset class="FieldSetFormat">
                    <legend class="MainHeadingLegend">
                        <label>LEARNING AND DEVELOPMENT MANAGER'S APPROVAL</label><br />
                        <label>(This section should be completed by the Learning and Development Manager)</label>
                    </legend>
                    <div>
                        The FIC’s L&D policy and procedures have been adhered to ?
                    </div>
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="PolicyAdheredToDropDownList" runat="server" Width="211px" required>
                                        <asp:ListItem Value=""></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </div>
                    <div>
                        Funds are available ?
                    </div>
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="FundsAvailableDropDownList" runat="server" Width="211px" required>
                                        <asp:ListItem Value=""></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </div>
                    <div>
                        Please indicate if you approve or reject
                    </div>
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="LDManagerApproveRejectDropDownList" runat="server" Width="211px" required OnSelectedIndexChanged="LDManagerApproveRejectDropDownList_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value=""></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Approve"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="Reject"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr id="LDManagerRejectReasonTr" runat="server" visible="false">
                                <td>
                                    <table>
                                        <tr>
                                            <td style="vertical-align: top;">Reject Reason
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="LDManagerRejectReasonTextBox" TextMode="MultiLine" runat="server" required class="TextAreaFormat"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>

                    </div>
                </fieldset>

                <br />
                <br />
            </div>

            <div id="PreparationSourcingDIV" runat="server" visible="false">
                <fieldset class="FieldSetFormat">
                    <legend class="SubHeadingLegend">Action Preparation and Sourcing</legend>
                    <div>
                        <table>
                            <tr>
                                <td class="auto-style9"></td>
                                <td class="auto-style9">
                                    <asp:Button ID="btnActionSourcing" runat="server" Text="Start Sourcing Process" Width="158px" OnClick="btnActionSourcing_Click" CssClass="buttons" /></td>
                                <td class="auto-style9"></td>
                            </tr>
                        </table>
                    </div>
                </fieldset>
            </div>

            <div id="LDAdminStep4DIV" runat="server" visible="false">

                <fieldset class="FieldSetFormat">
                    <legend class="SubHeadingLegend">Name of Service Provider</legend>
                    <div>
                        <table>
                            <tr>
                                <td>Institution Name&nbsp;</td>
                                <td>
                                    <asp:TextBox ID="txtServiceProvider" runat="server" CssClass="inputTextBox" required />
                                </td>
                            </tr>
                        </table>

                    </div>
                </fieldset>

                <br />
                <br />

                <fieldset class="FieldSetFormat">
                    <legend class="SubHeadingLegend">Date(s) of Training</legend>
                    <div>
                        <table>
                            <tr>
                                <td class="auto-style1">From Date&nbsp;</td>
                                <td class="auto-style1">
                                    <input id="tbDateFrom" type="text" runat="server" required /></td>
                                <td class="auto-style1">&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                <td class="auto-style1">To Date:&nbsp;</td>
                                <td class="auto-style1">
                                    <input id="tbDateTo" type="text" runat="server" required />
                                </td>
                            </tr>
                        </table>
                    </div>

                </fieldset>

                <br />
                <br />

                <fieldset class="FieldSetFormat">
                    <legend class="SubHeadingLegend">Cost</legend>
                    <div class="BlackLabelsBold">
                        Training Costs Total (Incl. VAT)&nbsp;
                        <asp:TextBox ID="txtTrainingCost" runat="server" CssClass="auto-style2" required Width="103px" />
                    </div>

                </fieldset>

                <br />
                <br />

                <fieldset class="FieldSetFormat">
                    <legend class="SubHeadingLegend">Approve / Reject Learning Intervention</legend>
                    <div class="BlackLabelsBold">
                        Please indicate if you approve or reject                        
                    </div>
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="LDAdminApproveRejectDropDownList" runat="server" Width="211px" required OnSelectedIndexChanged="LDAdminApproveRejectDropDownList_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value=""></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Approve"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="Reject"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr id="LDAdminRejectReasonTr" runat="server" visible="false">
                                <td>
                                    <table>
                                        <tr>
                                            <td style="vertical-align: top;">Reject Reason
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="LDAdminRejectReasonTextBox" TextMode="MultiLine" runat="server" required class="TextAreaFormat"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>

                </fieldset>

                <br />
                <br />

            </div>

            <table>
                <tr>
                    <td class="AlignCenter">
                        <asp:Button ID="EmployeeSubmit" AutoPostBack="true" OnClick="EmployeeSubmit_Click" Text="Save & Submit" runat="server" CssClass="buttons" />
                        <asp:Button ID="LineManagerApproveButton" AutoPostBack="true" Text="Update & Submit" runat="server" OnClick="LineManagerApproveButton_Click" Visible="false" CssClass="buttons" />
                        <asp:Button ID="LDManagerApproveButton" AutoPostBack="true" Text="Update & Submit" runat="server" OnClick="LDManagerApproveButton_Click" Visible="false" CssClass="buttons" />
                        <asp:Button ID="LDAdminSubmitButton" AutoPostBack="true" Text="Update & Submit" runat="server" OnClick="LDAdminSubmitButton_Click" Visible="false" CssClass="buttons" />
                        <input type="button" value="Reject Intervention" id="ShowRejectButton" runat="server" visible="false" />
                    </td>
                </tr>
            </table>
        </div>

        <fieldset class="FieldSetFormat" runat="server" visible="false">
            <legend class="SubHeadingLegend">Type of Intervention</legend>
            <div class="BlackLabelsBold">
                Select Intervention Type&nbsp;
                        <asp:DropDownList ID="TypeOfInterventionDropDownList" runat="server" Width="211px" required>
                            <asp:ListItem Value=""></asp:ListItem>
                            <asp:ListItem>Standard</asp:ListItem>
                            <asp:ListItem>Specialised</asp:ListItem>
                        </asp:DropDownList>
            </div>

        </fieldset>

        <br />
        <div id="ErrorMessageDiv" runat="server" visible="false" class="info"></div>

        <div id="RejectDiv" style="display: none;">

            <table>
                <tr>
                    <td style="vertical-align: top">Reject reason</td>
                    <td>
                        <asp:TextBox ID="RejectReasonTextbox" runat="server" CssClass="TextAreaFormat" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
            </table>

        </div>
    </fieldset>
</div>

<input type="hidden" id="InterventionIDHidden" value="" runat="server" />
<input type="hidden" id="InterventionStatusHidden" value="" runat="server" />
