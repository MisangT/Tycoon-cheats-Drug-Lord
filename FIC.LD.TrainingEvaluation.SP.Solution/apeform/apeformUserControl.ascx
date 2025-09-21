<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="apeformUserControl.ascx.cs" Inherits="FIC.LD.TrainingEvaluation.SP.Solution.apeform.apeformUserControl" %>


<link href="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/css/main.css" rel="stylesheet" />
<link href="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/css/jquery-ui.css" rel="stylesheet" />
<link href="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/css/datepicker.css" rel="stylesheet" />
<link href="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/css/bootstrap.min.css" rel="stylesheet" />

<script src="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/scripts/main.js"></script>
<script src="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/scripts/jquery-3.2.1.js"></script>
<script src="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/scripts/jquery-ui.js"></script>
<script src="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/scripts/bootstrap.min.js"></script>
<script src="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/scripts/bootstrap-datepicker.js"></script>

<div id="tblAPE" class="GeneralFormat">
    <fieldset class="FieldSetFormat">
        <legend class="MainHeadingLegend">
            <label class="SectionThreeHeader">ACTION PLAN EVALUATION FORM</label><br>
            <label class="BoldOrangeText">(To be completed & submitted to HR within 3 months post attendance learning)</label></legend>
        <div class="ContentsDIVFormat" runat="server" id="MainContent">
            <br />
            <fieldset class="FieldSetFormat">
                <legend class="MainHeadingLegend">Employee Details </legend>
                <div>
                    <table>
                        <tr>
                            <td>Your Full Name</td>
                            <td>
                                <asp:TextBox ID="txtEmpFullName" runat="server" CssClass="inputTextBox" disabled="disabled" />
                            </td>
                        </tr>
                        <tr>
                            <td>Designation</td>
                            <td>
                                <asp:TextBox ID="txtDesignation" runat="server" CssClass="inputTextBox" disabled="disabled" />
                            </td>
                        </tr>
                        <tr>
                            <td>Your Department</td>
                            <td>
                                <asp:TextBox ID="txtDepartment" runat="server" CssClass="inputTextBox" disabled="disabled" />
                            </td>
                        </tr>
                        <tr>
                            <td>Name of Learning Intervention</td>
                            <td>
                                <asp:TextBox ID="txtLearningIntervention" runat="server" CssClass="inputTextBox" disabled="disabled" />
                            </td>
                        </tr>
                        <tr>
                            <td>Date (s) Attended</td>
                            <td>
                                <asp:TextBox ID="txtTrainingDates" runat="server" CssClass="inputTextBox" disabled="disabled" />
                            </td>
                        </tr>
                        <tr>
                            <td>Name of Line Manager</td>
                            <td>
                                <asp:TextBox ID="txtManager" runat="server" CssClass="inputTextBox" disabled="disabled" />
                            </td>
                        </tr>
                    </table>
                    <br />
                </div>
            </fieldset>
            <br />
            <br />
            <div id="tblManagerAPE" class="GeneralFormat" runat="server">
                <fieldset class="FieldSetFormat">
                    <legend class="MainHeadingLegend">
                        <label class="SectionThreeHeader">SECTION 1: WHAT ARE THE TANGIBLE RESULTS GAINED FROM THE ATTENDED INTERVENTION?</label><br>
                        <label class="BoldOrangeText">(To be completed & submitted to HR within 3 months post attendance learning)</label></legend>
                    <fieldset class="FieldSetFormat">
                        <legend class="SubHeadingLegend">1.1	Did the employee demonstrate the skills/behaviour as outlined in the set learning objectives?</legend>
                        <div>
                            <table>
                                <tr>
                                    <td>
                                        <br />
                                        <asp:DropDownList ID="SkillsDemonstrationDropDownList" runat="server" Width="211px" required="true">
                                            <asp:ListItem Value="Select Option"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                            <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </fieldset>
                    <br />
                    <br />
                    <fieldset class="FieldSetFormat">
                        <legend class="SubHeadingLegend">1.2 How did the employee apply their newly acquired skills on the job?</legend>
                        <div>
                            <table>
                                <tr>
                                    <td>
                                        <br />
                                        <textarea id="txtManagerApplicationOfNewSkills" class="TextAreaFormat" runat="server"></textarea>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </fieldset>
                    <br />
                    <br />
                    <fieldset class="FieldSetFormat">
                        <legend class="SubHeadingLegend">
                            <label class="SectionThreeHeader">1.3	What changes in job performance/behaviour resulted from the learning attended?</label><br>
                            <label>(Capability to perform the newly acquired skills while on the job)</label></legend>
                        <div>
                            <table>
                                <tr>
                                    <td>
                                        <br />
                                        <textarea id="txtJobPerformanceChanges" class="TextAreaFormat" runat="server"></textarea>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </fieldset>
                    <br />
                    <br />
                    <fieldset class="FieldSetFormat">
                        <legend class="SubHeadingLegend">1.4 Any additional comments which confirm that the employee currently uses the acquired skills on the job.</legend>
                        <div>
                            <table>
                                <tr>
                                    <td>
                                        <br />
                                        <textarea id="txtAdditionalComments" class="TextAreaFormat" runat="server"></textarea>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </fieldset>
                     <br />
                    <br />
                    <div id="EmployeePerformanceImprovementDiv" runat="server" visible="false">
                          <fieldset class="FieldSetFormat">
                        <legend class="SubHeadingLegend">1.5	Do you notice any improvement on the employee&#39;s performance?</legend>
                        <div>
                            <table>
                                <tr>
                                    <td>
                                        <br />
                                        <asp:DropDownList ID="EmployeePerformanceImprovementDropDownList" runat="server" Width="211px" required="true">
                                            <asp:ListItem Value="Select Option"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                            <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </fieldset>
                    </div>                   
                </fieldset>
                <br />
            </div>
            <div id="tblEmployeeAPE" class="GeneralFormat"  runat="server">
                <br />
                <fieldset class="FieldSetFormat">
                    <legend class="MainHeadingLegend">
                        <label class="SectionThreeHeader">SECTION 2: ASSESSMENT OF OPPORTUNITY TO APPLY THE ACQUIRED SKILLS ON THE JOB</label><br>
                        <label class="BoldOrangeText">(This section to be completed by employee)</label></legend>
                    <fieldset class="FieldSetFormat">
                        <legend class="SubHeadingLegend">2.1	How would you rate the timing of this learning intervention?</legend>
                        <div>
                            <table>
                                <tr>
                                    <td>
                                        <br />
                                        <asp:DropDownList ID="LearningInterventionRatingDropDownList" runat="server" Width="211px" required="true">
                                            <asp:ListItem Value="Select Option"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Too early"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Just-in-time"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Too late"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </fieldset>
                    <br />
                    <br />
                    <fieldset class="FieldSetFormat">
                        <legend class="SubHeadingLegend">
                            <label class="SectionThreeHeader">2.2	Have you had an opportunity to apply your newly acquired skills?</label><br>
                            <label>(exposure in current work/projects to practice the acquired skills)</label></legend>
                        <div>
                            <table>
                                <tr>
                                    <td>
                                        <br />
                                        <asp:DropDownList ID="EmployeeApplicationOfNewSkillsChoiceDropDownList" runat="server" Width="211px" required="true">
                                            <asp:ListItem Value="Select Option"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                            <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                        </asp:DropDownList>
                                        <br />
                                        <br />
                                        <label class="BoldOrangeText">Please explain how?</label><br />
                                        <textarea id="txtEmployeeApplicationOfNewSkillsComments" class="TextAreaFormat" runat="server"></textarea>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </fieldset>
                    <br />
                    <br />
                    <fieldset class="FieldSetFormat">
                        <legend class="SubHeadingLegend">2.3	Did the line manager offer the required resources and support for effective application of the acquired skills over the last three (3) months?</legend>
                        <div>
                            <table>
                                <tr>
                                    <td>
                                        <br />
                                        <asp:DropDownList ID="RequiredResourcesAndSupportOfferedChoiceDropDownList" runat="server" Width="211px" required="true">
                                            <asp:ListItem Value="Select Option"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                            <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                        </asp:DropDownList>
                                        <br />
                                        <br />
                                        <label class="BoldOrangeText">Please explain how?</label><br />
                                        <textarea id="txtRequiredResourcesAndSupportOfferedComments" class="TextAreaFormat" runat="server"></textarea>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </fieldset>
                    <br />
                    <br />
                    <fieldset class="FieldSetFormat">
                        <legend class="SubHeadingLegend">2.4 What is that you had no knowledge of /could not do before that you now know/able to do as a result of the attended intervention?</legend>
                        <div>
                            <table>
                                <tr>
                                    <td>
                                        <br />
                                        <textarea id="txtEmployeeNewKnowledge" class="TextAreaFormat" runat="server"></textarea>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </fieldset>
                </fieldset>
                <br />
            </div>
            <br />

            <table>
                <tr>
                    <td class="AlignCenter">
                        <asp:Button ID="ManagerSubmit" AutoPostBack="true" OnClick="ManagerSubmit_Click" Text="Submit" runat="server" Width="121px" />
                        <asp:Button ID="EmployeeSubmit" AutoPostBack="true" OnClick="EmployeeSubmit_Click" Text="Submit" runat="server" Width="121px" />
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <div id="ErrorMessageDiv" runat="server" visible="false" class="info"></div>
        </div>
    </fieldset>
</div>
