<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="dashboardUserControl.ascx.cs" Inherits="FIC.LD.TrainingEvaluation.SP.Solution.dashboard.dashboardUserControl" %>

<link href="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/css/employeedashboard.css" rel="stylesheet" />
<script src="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/scripts/employeedashboard.js"></script>



<fieldset>
    <legend class="MainHeadingLegend">EMPLOYEE INTERVENTIONS</legend>

    <asp:GridView ID="EmployeeInterventionsGridView" runat="server" AutoGenerateColumns="false" DataKeyNames="InterventionID" CssClass="Grid" AllowPaging="true" PageSize="15" EmptyDataText="No learning Interventions available." Width="100%">
        <Columns>

            <asp:TemplateField ItemStyle-Width="0px">
                <ItemTemplate>
                    <asp:ImageButton ID="ShowHideEmployeeDashBoardFormsImageButton" runat="server" OnClick="ShowHideEmployeeDashBoardFormsImageButton_OnClick" ImageUrl="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/images/plus.png" CommandArgument="Show" />
                    <asp:Panel ID="EmployeeFormsPanel" runat="server" Visible="false" Style="position:relative">
                        <asp:GridView ID="EmployeeFormsGridView" runat="server" AutoGenerateColumns="false" PageSize="15" AllowPaging="true" OnRowDataBound="EmployeeFormsGridView_RowDataBound" CssClass="ChildGrid" DataKeyNames="InterventionID" Width="100%">
                            <Columns>                               
                                <asp:BoundField DataField="FormType" HeaderText="Form Type" ItemStyle-Width="120" />
                                <asp:BoundField DataField="EndDateOfTraining" HeaderText="Training End Date" DataFormatString="{0:dd-MMMM-yyyy}" ItemStyle-Width="120" />
                                <asp:BoundField DataField="FirstReminderDate" HeaderText="First Reminder Date" DataFormatString="{0:dd-MMMM-yyyy}" ItemStyle-Width="120" />
                                <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd-MMMM-yyyy}" ItemStyle-Width="150" />
                                <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="100" />
                                <asp:TemplateField HeaderText="Details" ItemStyle-Width="60">
                                    <ItemTemplate>
                                        <asp:Button ID="PostLearningInterventionEvaluationButton" runat="server" Text="Complete Post Learning Intervention Evaluation" OnClick="PostLearningInterventionEvaluationButton_Click" CssClass="buttons" />
                                    </ItemTemplate>
                                    <ItemStyle Width="150px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="0">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="EmployeeDashboardFormHiddenID" runat="server" Value='<%# Eval("InterventionID")%>'></asp:HiddenField>
                                    </ItemTemplate>                                    
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" CssClass="GridPager"  />
                        </asp:GridView>
                    </asp:Panel>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="NameOfLearning" HeaderText="Intervention Name" ItemStyle-Width="150" />
            <asp:BoundField DataField="DevelopmentNeeds" HeaderText="Development Needs" ItemStyle-Width="150" />
            <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="150" />
            <asp:TemplateField HeaderText="Details" ItemStyle-Width="60">
                <ItemTemplate>
                    <asp:Button ID="ViewIntervention" runat="server" Text="Details" OnClick="ViewIntervention_Click" CssClass="buttons"/>
                </ItemTemplate>
                <ItemStyle Width="60px"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="0">
                <ItemTemplate>
                    <asp:HiddenField ID="HiddenID" runat="server" Value='<%# Eval("InterventionID")%>'></asp:HiddenField>
                </ItemTemplate>                
            </asp:TemplateField>
        </Columns>
        <PagerStyle HorizontalAlign="Center" CssClass="GridPager"  />
    </asp:GridView>
    <br />
    <div id="ErrorMessageDiv" runat="server" visible="false" class="info"></div>

</fieldset>
