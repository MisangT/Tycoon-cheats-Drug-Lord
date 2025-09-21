hbh<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="plie_formUserControl.ascx.cs" Inherits="FIC.LD.TrainingEvaluation.SP.Solution.plie_form.plie_formUserControl" %>


<link href="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/css/main.css" rel="stylesheet" />


<div id="tblPLIE" class="GeneralFormat">
    <fieldset class="FieldSetFormat">
        <legend class="MainHeadingLegend">
            <label class="SectionThreeHeader">POST LEARNING INTERVENTION EVALUATION FORM</label><br>
            <label class="BoldOrangeText">(To be completed by the learner and submitted to HR within 1 week post attendance of learning)</label></legend>
        <div class="ContentsDIVFormat"  runat="server" id="MainContent">

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
                            <td>Name of Service Provider</td>
                            <td>
                                <asp:TextBox ID="txtServiceProvider" runat="server" CssClass="inputTextBox" disabled="disabled" />
                            </td>
                        </tr>
                        <tr>
                            <td>Date (s) Attended</td>
                            <td>
                                <asp:TextBox ID="txtTrainingDates" runat="server" CssClass="inputTextBox" disabled="disabled" />
                            </td>
                        </tr>
                    </table>
                    <br />
                </div>
            </fieldset>
            <br />
            <br />
            <fieldset class="FieldSetFormat">
                <legend class="MainHeadingLegend">Post Learning Intervention Evaluation Survey
                </legend>
                <fieldset class="FieldSetFormat">
                    <legend class="SubHeadingLegend">1.	The course was worth my time and met my learning expectations?</legend>
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <br />
                                    <asp:DropDownList ID="CourseWorthTimeandExpectationsDropDownList" runat="server" Width="211px" required="true">
                                        <asp:ListItem Value="Select Option"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="Strongly Agree"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Agree"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Neutral"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Disagree"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Strongly Disagree"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </fieldset>
                <br />
                <br />
                <fieldset class="FieldSetFormat">
                    <legend class="SubHeadingLegend">2.	I will be able to apply the knowledge/skills acquired?</legend>
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <br />
                                    <asp:DropDownList ID="ApplySkillsLearnedDropDownList" runat="server" Width="211px" required="true">
                                        <asp:ListItem Value="Select Option"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="Strongly Agree"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Agree"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Neutral"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Disagree"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Strongly Disagree"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </fieldset>
                <br />
                <br />
                <fieldset class="FieldSetFormat">
                    <legend class="SubHeadingLegend">3.	How was the quality of the presenter/facilitator?</legend>
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <br />
                                    <asp:DropDownList ID="QualityOfPresenterDropDownList" runat="server" Width="211px" required="true">
                                        <asp:ListItem Value="Select Option"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="Excellent "></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Very Good"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Good"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Somewhat Good"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Poor"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </fieldset>
                <br />
                <br />
                <fieldset class="FieldSetFormat">
                    <legend class="SubHeadingLegend">4.	Was the learning intervention well-coordinated by the L&D team? </legend>
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <br />
                                    <asp:DropDownList ID="LearningInterventionCoordinationDropDownList" runat="server" Width="211px" required="true">
                                        <asp:ListItem Value="Select Option"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="Strongly Agree"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Agree"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Neutral"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Disagree"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Strongly Disagree"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </fieldset>
                <br />
                <br />
                 <fieldset class="FieldSetFormat">
                            <legend class="SubHeadingLegend">5.	What were the highlights of the training or areas of improvement?</legend>
                            <div>
                                <table>
                                    <tr>
                                        <td>
                                            <textarea id="txtHighlightsOfTraining" class="TextAreaFormat" runat="server"></textarea>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </div>
                        </fieldset>
            </fieldset>
            <br />
            <br />           
            <table>
                <tr>
                    <td class="AlignCenter">
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
