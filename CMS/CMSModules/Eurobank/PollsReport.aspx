<%@ Page Title="" Language="C#" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" AutoEventWireup="true" Theme="Default" CodeBehind="PollsReport.aspx.cs" Inherits="CMSModules_Eurobank_PollsReport" %>

<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<%@ Register Namespace="CMS.UIControls.UniGridConfig" TagPrefix="ug" Assembly="CMS.UIControls" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <div class="form-horizontal form-filter" style="width: 1000px !important">
        <h4><asp:Label ID="lblDisplayname" runat="server"/></h4>
        <br />
         
        <h5><asp:Label ID="lblQuestion" runat="server"/></h5>
                <br />
        <asp:Repeater ID="RepterReportDetails" runat="server">
            <ItemTemplate>
                <span><%# Eval("AnswerText")%></span>

                <div class="progress">
                    <div class="progress-bar" role="progressbar" style="<%# "width:"+CheckVote(Eval("AnswerPollID"),Eval("ItemID"))+"%" %>" aria-valuenow="<%# CheckVote(Eval("AnswerPollID"),Eval("ItemID"))%>" aria-valuemin="0" aria-valuemax="100"></div>
                    <span><%# CheckVote(Eval("AnswerPollID"),Eval("ItemID"))+"%"%></span>
                </div>

            </ItemTemplate>
        </asp:Repeater>
    </div>

</asp:Content>
