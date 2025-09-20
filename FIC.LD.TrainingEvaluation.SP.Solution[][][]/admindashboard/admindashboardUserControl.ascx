<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="admindashboardUserControl.ascx.cs" Inherits="FIC.LD.TrainingEvaluation.SP.Solution.admindashboard.admindashboardUserControl" %>

<link href="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/css/admindashboard.css" rel="stylesheet" />
<script src="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/scripts/admindashboard.js"></script>

<fieldset id="ConfirmedInterventionsFieldset" runat="server">
    <legend class="MainHeadingLegend">EMPLOYEES INTERVENTIONS DASHBOARD</legend>

        <div>
        <table>
            <tr>           
                <td>
                    Filter by : 
                    <asp:DropDownList ID="EmpFullNameDropDownList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="EmpFullNameDropDownList_SelectedIndexChanged" Visible="true" Height="20px" Width="158px">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <asp:GridView ID="EmployeesGridView" runat="server" AutoGenerateColumns="false" DataKeyNames="EmployeeID" CssClass="Grid" AllowPaging="true" OnPageIndexChanging="EmployeesGridView_PageIndexChanging" PageSize="30" EmptyDataText="No employees available." Width="100%">
        <Columns>
            <asp:TemplateField ItemStyle-Width="0px">
                <ItemTemplate>
                    <asp:ImageButton ID="ShowHideEmployeeDashBoardFormsImageButton" runat="server" OnClick="ShowHideEmployeeDashBoardFormsImageButton_OnClick" ImageUrl="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/images/plus.png" CommandArgument="Show" />
                    <asp:Panel ID="LearningInterventionFormsPanel" runat="server" Visible="false" Style="position: relative">
                        <asp:GridView ID="LearningInterventionFormsGridView" runat="server" AutoGenerateColumns="false" PageSize="15" AllowPaging="true" CssClass="ChildGrid" DataKeyNames="InterventionID" Width="100%">
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="0px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ShowHideEmployeeFeedbackInterventionsImageButton" runat="server" OnClick="ShowHideEmployeeFeedbackInterventionsImageButton_OnClick" ImageUrl="../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/images/plus.png" CommandArgument="Show" />
                                        <asp:Panel ID="EmployeeFeedbackInterventionsPanel" runat="server" Visible="false" Style="position: relative">
                                            <asp:GridView ID="EmployeeFeedbackInterventionsGridView" runat="server" AutoGenerateColumns="false" PageSize="15" AllowPaging="true" OnRowDataBound="EmployeeFeedbackInterventions_RowDataBound" CssClass="ChildGrid" DataKeyNames="InterventionID" Width="100%">
                                                <Columns>
                                                    <asp:BoundField DataField="FormType" HeaderText="Form Name" ItemStyle-Width="120" />
                                                    <asp:BoundField DataField="EndDateOfTraining" HeaderText="Training End Date" DataFormatString="{0:dd-MMMM-yyyy}" ItemStyle-Width="120" />
                                                    <asp:BoundField DataField="FirstReminderDate" HeaderText="First Reminder Date" DataFormatString="{0:dd-MMMM-yyyy}" ItemStyle-Width="120" />
                                                    <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd-MMMM-yyyy}" ItemStyle-Width="150" />
                                                    <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="100" />
                                                    <asp:TemplateField HeaderText="Details" ItemStyle-Width="60">
                                                        <ItemTemplate>
                                                            <asp:Button ID="EmployeedFeedbackButton" runat="server" Text="Complete Post Learning Intervention Evaluation" OnClick="EmployeedFeedbackButton_Click" CssClass="buttons" />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="150px"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-Width="0">
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="EmployeeDashboardFormHiddenID" runat="server" Value='<%# Eval("InterventionID")%>'></asp:HiddenField>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                                            </asp:GridView>
                                        </asp:Panel>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="NameOfLearning" HeaderText="Intervention Name" ItemStyle-Width="200" />
                                <asp:BoundField DataField="DevelopmentNeeds" HeaderText="Development Needs" ItemStyle-Width="200" />
                                <asp:BoundField DataField="EmployeeExpectations" HeaderText="Employee Expectations" ItemStyle-Width="200" />
                                <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="100" />
                                <asp:TemplateField HeaderText="Details" ItemStyle-Width="60">
                                    <ItemTemplate>
                                        <asp:Button ID="ViewIntervention" runat="server" Text="Details" OnClick="ViewIntervention_Click" CssClass="buttons" />
                                    </ItemTemplate>
                                    <ItemStyle Width="60px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="0">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="HiddenInterventionID" runat="server" Value='<%# Eval("InterventionID")%>'></asp:HiddenField>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                        </asp:GridView>
                    </asp:Panel>
                </ItemTemplate>
            </asp:TemplateField>
        
            <asp:BoundField DataField="FullName" HeaderText="Full Name" ItemStyle-Width="150" />
            <asp:BoundField DataField="Designation" HeaderText="Designation" ItemStyle-Width="150" />
            <asp:BoundField DataField="Department" HeaderText="Department" ItemStyle-Width="150" />

            <asp:TemplateField ItemStyle-Width="0">
                <ItemTemplate>
                    <asp:HiddenField ID="HiddenEmployeeID" runat="server" Value='<%# Eval("EmployeeID")%>'></asp:HiddenField>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />

        <HeaderStyle BackColor="#7D9BC0" ForeColor="White" />
    </asp:GridView>    

    <div id="ErrorMessageDiv" runat="server" visible="false" class="info"></div>

</fieldset>
<input type="hidden" id="InterventionIDHidden" value="" runat="server" />