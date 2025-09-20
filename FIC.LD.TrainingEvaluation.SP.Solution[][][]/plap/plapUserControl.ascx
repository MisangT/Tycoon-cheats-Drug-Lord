<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="plapUserControl.ascx.cs" Inherits="FIC.LD.TrainingEvaluation.SP.Solution.plap.plapUserControl" %>


<link href="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/css/main.css" rel="stylesheet" />
<link href="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/css/jquery-ui.css" rel="stylesheet" />
<link href="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/css/datepicker.css" rel="stylesheet" />
<link href="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/css/bootstrap.min.css" rel="stylesheet" />

<script src="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/scripts/main.js"></script>
<script src="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/scripts/jquery-3.2.1.js"></script>
<script src="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/scripts/jquery-ui.js"></script>
<script src="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/scripts/bootstrap.min.js"></script>
<script src="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/scripts/bootstrap-datepicker.js"></script>

<div id="plapDiv" class="GeneralFormat">
    <fieldset class="FieldSetFormat">
        <legend class="MainHeadingLegend">
            <label class="SectionThreeHeader">POST LEARNING ACTION PLAN</label><br>
            <label class="BoldOrangeText">(To be completed by the learner and line manager and submitted to HR within 2 weeks post attendance of learning)</label></legend>
        <div class="ContentsDIVFormat" runat="server" id="MainContent">

            <fieldset class="FieldSetFormat">
                <legend class="MainHeadingLegend">Employee Details </legend>
                <div>
                    <table>
                        <tr>
                            <td>Full Name</td>
                            <td>
                                <asp:TextBox ID="FullNameTextBox" runat="server" CssClass="inputTextBox" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>Designation</td>
                            <td>
                                <asp:TextBox ID="DesignationTextBox" runat="server" CssClass="inputTextBox" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>Department</td>
                            <td>
                                <asp:TextBox ID="DepartmentTextBox" runat="server" CssClass="inputTextBox" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>Name of Learning Intervention</td>
                            <td>
                                <asp:TextBox ID="NameofLearningInterventionTextBox" runat="server" CssClass="inputTextBox" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>Date of Attendance</td>
                            <td>
                                <asp:TextBox ID="DateofAttendanceTextBox" runat="server" CssClass="inputTextBox" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>Manager</td>
                            <td>
                                <asp:TextBox ID="ManagerNameTextBox" runat="server" CssClass="inputTextBox" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <br />
                </div>
            </fieldset>

            <br />
            <br />

            <fieldset class="FieldSetFormat">
                <legend class="SubHeadingLegend">Post Learning Action Plans</legend>
                <div>
                    <table style="width: 100%;" id="ActionPlanTable">
                        <tr>
                            <td>INITIAL LEARNING OBJECTIVES<br>
                                <label class="BoldOrangeText" style="font-size: 12px; color: #FF0000;">NB: Ensure that this section matches your initial outlined learning objectives)</label>
                            </td>
                            <td>ACTION PLAN ITEM/S
                            </td>
                            <td>HOW TO IMPLEMENT THE ITEM/S
                            </td>
                            <td>WHEN IT SHOULD BE DONE
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input type="text" id="InitialObjectiveTextBox1" class="inputTextBox" required runat="server" />
                            </td>
                            <td>
                                <input type="text" id="ActionPlanTextBox1" class="inputTextBox" required runat="server" />
                            </td>
                            <td>
                                <input type="text" id="HowToImplementTextBox1" class="inputTextBox" required runat="server" />
                            </td>
                            <td>
                                <input type="text" id="WhenItShouldBeDoneTextBox1" class="inputTextBox" required runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input type="text" id="InitialObjectiveTextBox2" class="inputTextBox" runat="server" />
                            </td>
                            <td>
                                <input type="text" id="ActionPlanTextBox2" class="inputTextBox" runat="server" />
                            </td>
                            <td>
                                <input type="text" id="HowToImplementTextBox2" class="inputTextBox" runat="server" />
                            </td>
                            <td>
                                <input type="text" id="WhenItShouldBeDoneTextBox2" class="inputTextBox" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input type="text" id="InitialObjectiveTextBox3" class="inputTextBox" runat="server" />
                            </td>
                            <td>
                                <input type="text" id="ActionPlanTextBox3" class="inputTextBox" runat="server" />
                            </td>
                            <td>
                                <input type="text" id="HowToImplementTextBox3" class="inputTextBox" runat="server" />
                            </td>
                            <td>
                                <input type="text" id="WhenItShouldBeDoneTextBox3" class="inputTextBox" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input type="text" id="InitialObjectiveTextBox4" class="inputTextBox" runat="server" />
                            </td>
                            <td>
                                <input type="text" id="ActionPlanTextBox4" class="inputTextBox" runat="server" />
                            </td>
                            <td>
                                <input type="text" id="HowToImplementTextBox4" class="inputTextBox" runat="server" />
                            </td>
                            <td>
                                <input type="text" id="WhenItShouldBeDoneTextBox4" class="inputTextBox" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>                       
                    </table>
                    <br />
                    <input type="button" value="Add Row" id="addRowButton" runat="server" visible="false" /><input type="button" value="Remove Row" id="removeRowButton" runat="server" visible="false" />
                    <br />
                </div>
            </fieldset>

            <br />
            <br />

            <fieldset class="FieldSetFormat">
                <legend class="SubHeadingLegend">Learner to outline what resources/ support is required from Line Manager to achieve the set action plan and how progress will be measured</legend>
                <asp:TextBox TextMode="MultiLine" ID="LearnerMotivationTextBox" CssClass="TextAreaFormat" runat="server" required></asp:TextBox>
            </fieldset>

            <br />
            <br />

            <fieldset class="FieldSetFormat" id="ApproveRejectFieldSet" runat="server" visible="false">
                <legend class="MainHeadingLegend">Please indicate if you approve or reject - Employee Post learning Action Plan</legend>
                <div>
                </div>
                <div>
                    <table>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ManagerApproveOrRejectDropDownList" runat="server" Width="211px" required>
                                    <asp:ListItem Value=""></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                </div>
            </fieldset>       
            
            <br />

            <table>
                <tr>
                    <td class="AlignCenter">
                        <asp:Button ID="EmployeeSubmit" Text="Save & Submit" runat="server" OnClick="EmployeeSubmit_Click" />
                        <asp:Button ID="LineManagerApproveButton" Text="Save & Submit" runat="server" Visible="false" OnClick="LineManagerApproveButton_Click" />
                    </td>
                </tr>
            </table>

        </div>

         <br />

        <div id="ErrorMessageDiv" runat="server" visible="false" class="info"></div>

    </fieldset>
</div>

<input type="hidden" runat="server" id="RowsAddedHidden" />

<input type="hidden" runat="server" id="RowsSavedCount" />