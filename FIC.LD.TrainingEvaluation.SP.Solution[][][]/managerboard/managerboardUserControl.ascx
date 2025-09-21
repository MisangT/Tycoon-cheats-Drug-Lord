<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="managerboardUserControl.ascx.cs" Inherits="FIC.LD.TrainingEvaluation.SP.Solution.managerboard.managerboardUserControl" %>

<link href="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/css/managerdashboard.css" rel="stylesheet" />
<link href="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/css/jquery-ui.css" rel="stylesheet" />

<script src="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/scripts/managerdashboard.js"></script>
<script src="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/scripts/jquery-ui.js"></script>

<div id="ErrorMessageDiv" runat="server" visible="false" class="info"></div>
<br />

<div id="tabs" runat="server">

    <ul>
        <li><a href="#tabs-1">Individual Skill Development Approval Form</a></li>
        <li><a href="#tabs-2">Post Learning Action Plan</a></li>
        <li><a href="#tabs-3">Action Plan Evaluation</a></li>
    </ul>

    <div id="tabs-1">
        <fieldset id="ManagerEmployeesInterventionsFieldset" runat="server">
            <legend class="MainHeadingLegend">MANAGER EMPLOYEES DASHBOARD</legend>
            <div>                        
                <div>
                    <asp:GridView ID="ManagerEmployeesInterventionsGridView" runat="server" CssClass="Grid" AutoGenerateColumns="false" DataKeyNames="ID" AllowPaging="true" PageSize="15" EmptyDataText="No learning Interventions available." Width="100%">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="5">
                                <ItemTemplate>
                                    <asp:HiddenField ID="HiddenID" runat="server" Value='<%# Eval("ID")%>'></asp:HiddenField>
                                </ItemTemplate>

<ItemStyle Width="5px"></ItemStyle>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FullName" HeaderText="Full Name" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="NameOfLearning" HeaderText="Name Of Learning" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="DevelopmentNeeds" HeaderText="Development Needs" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="DiscussedWithManager" HeaderText="Discussed With Manager" ItemStyle-Width="180" >
<ItemStyle Width="180px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="EmployeeExpectations" HeaderText="Employee Expectations" ItemStyle-Width="500" >
<ItemStyle Width="500px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="DateCreated" HeaderText="Date Submitted" DataFormatString="{0:dd-MMMM-yyyy}" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField ItemStyle-Width="80">
                                <ItemTemplate>
                                    <asp:Button ID="EmployeeIntervention" runat="server" OnClick="EmployeeIntervention_Click" Text="Details" CssClass="buttons" />
                                </ItemTemplate>

<ItemStyle Width="80px"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle BackColor="#7D9BC0" ForeColor="White" />
                        <%--<RowStyle BackColor="#99CA79" />--%>
                    </asp:GridView>
                </div>
            </div>
        </fieldset>

    </div>

    <div id="tabs-2">
        <fieldset id="PostLearningActionPlanFieldset" runat="server">
            <legend class="MainHeadingLegend">MANAGER - Post Learning Action Plan</legend>
            <div>
                <div>
                    <asp:GridView ID="PostLearningActionPlanGridView" runat="server" CssClass="Grid" AutoGenerateColumns="false" DataKeyNames="InterventionID" EmptyDataText="No Post Learning Action Plans available." AllowPaging="true" PageSize="15" Width="100%">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="5">
                                <ItemTemplate>
                                    <asp:HiddenField ID="HiddenID" runat="server" Value='<%# Eval("InterventionID")%>'></asp:HiddenField>
                                </ItemTemplate>

<ItemStyle Width="5px"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="PostLearningActionPlanEvaluationLabel" runat="server" Text="Post Learning Action Plan" ItemStyle-Width="150" />
                                </ItemTemplate>

<ItemStyle Width="150px"></ItemStyle>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Username" HeaderText="Employee Name" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="StartDateOfTraining" HeaderText="Training Start Date" DataFormatString="{0:dd-MMMM-yyyy}" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="EndDateOfTraining" HeaderText="Training End Date" DataFormatString="{0:dd-MMMM-yyyy}" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="FirstReminderDate" HeaderText="First Reminder Date" DataFormatString="{0:dd-MMMM-yyyy}" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd-MMMM-yyyy}" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="150">
<ItemStyle Width="150px"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Details" ItemStyle-Width="60">
                                <ItemTemplate>
                                    <asp:Button ID="PostLearningActionPlanButton" runat="server" Text="Complete Post Learning Action Plan Evaluation" OnClick="PostLearningActionPlan_Click" CssClass="buttons" />
                                </ItemTemplate>

<ItemStyle Width="60px"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle BackColor="#7D9BC0" ForeColor="White" />
                        <%--<RowStyle BackColor="#99CA79" />--%>
                    </asp:GridView>
                </div>
            </div>
        </fieldset>
    </div>

    <div id="tabs-3">
    <fieldset id="ActionPlanEvaluationFieldset" runat="server">
        <legend class="MainHeadingLegend">MANAGER - Action Plan Evaluation</legend>
        <div>
            <div>
                <asp:GridView ID="ActionPlanEvaluationGridView" runat="server" CssClass="Grid" AutoGenerateColumns="false" DataKeyNames="InterventionID" EmptyDataText="No Action Plan Evaluations available." AllowPaging="true" PageSize="15" Width="100%">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="5">
                            <ItemTemplate>
                                <asp:HiddenField ID="HiddenID" runat="server" Value='<%# Eval("InterventionID")%>'></asp:HiddenField>
                            </ItemTemplate>

<ItemStyle Width="5px"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="150">
                            <ItemTemplate>
                                <asp:Label ID="ActionPlanEvaluationLabel" runat="server" Text="Action Plan Evaluation" ItemStyle-Width="150" />
                            </ItemTemplate>

<ItemStyle Width="150px"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Username" HeaderText="Employee Name" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="StartDateOfTraining" HeaderText="Training Start Date" DataFormatString="{0:dd-MMMM-yyyy}" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="EndDateOfTraining" HeaderText="Training End Date" DataFormatString="{0:dd-MMMM-yyyy}" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="FirstReminderDate" HeaderText="First Reminder Date" DataFormatString="{0:dd-MMMM-yyyy}" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd-MMMM-yyyy}" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="150">
<ItemStyle Width="150px"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Details" ItemStyle-Width="60">
                            <ItemTemplate>
                                <asp:Button ID="ActionPlanEvaluationButton" runat="server" Text="Complete Action Plan Evaluation" OnClick="ActionPlanEvaluation_Click" CssClass="buttons" />
                            </ItemTemplate>

<ItemStyle Width="60px"></ItemStyle>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle BackColor="#7D9BC0" ForeColor="White" />
                    <%--<RowStyle BackColor="#99CA79" />--%>
                </asp:GridView>
            </div>
        </div>
    </fieldset>

</div>

</div>