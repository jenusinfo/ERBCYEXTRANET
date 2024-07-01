<%@ Page Title="" Language="C#" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" AutoEventWireup="true" Theme="Default" CodeBehind="PollsList.aspx.cs" Inherits="CMSModules_Eurobank_PollsList" %>
<%@ Register src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" tagname="UniGrid" tagprefix="cms" %>
<%@ Register Namespace="CMS.UIControls.UniGridConfig" TagPrefix="ug" Assembly="CMS.UIControls" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
   <div class="form-horizontal form-filter" style="width:1000px !important">
     
  
       <asp:Button ID="btnNewPolls" runat="server" Text="New Polls" CssClass="btn btn-primary" OnClick="btnNewPolls_Click" />
       <br />
          <br />
  <cms:UniGrid ID="PollsList" runat="server"  Columns="ItemID,PollQuestion,PollOpenFrom,PollOpenTo,Isactive" >

      <GridActions>
          <ug:Action name="edit" caption="$General.Edit$" fonticonclass="icon-edit" fonticonstyle="allow" />
<ug:Action Name="deleteaction" Caption="$General.Delete$" Icon="Delete.png" Confirmation="$General.ConfirmDelete$"  />
          <ug:Action Name="report"  fonticonclass="icon-file" fonticonstyle="allow"  Caption="View"/>
</GridActions>
    <GridColumns>
        <ug:Column Source="PollQuestion" Caption="Question"   />
        <ug:Column Source="PollOpenFrom" Caption="Open From" ExternalSourceName="PollOpenFrom"  />
        <ug:Column Source="PollOpenTo" Caption="Open To" ExternalSourceName="PollOpenTo"  />
        <ug:Column Source="Isactive" Caption="Active"   />
    </GridColumns>
         
</cms:UniGrid>
       
          </div>
        
</asp:Content>
