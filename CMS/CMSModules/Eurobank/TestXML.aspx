<%@ Page Title="" Language="C#" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" AutoEventWireup="true" CodeBehind="TestXML.aspx.cs" Inherits="CMSModules_Eurobank_TestXML"  Theme="Default" %>

<%@ Register src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" tagname="UniGrid" tagprefix="cms" %>
<%@ Register Namespace="CMS.UIControls.UniGridConfig" TagPrefix="ug" Assembly="CMS.UIControls" %>

<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <div class="form-group row">
        <div>
        <asp:TextBox ID="txtApplicationNumber" runat="server" placeholder="Enter Application Number" CssClass="form-control "></asp:TextBox>
    <asp:RequiredFieldValidator ID="rflavel" ControlToValidate="txtApplicationNumber" runat="server" ErrorMessage="Please enter Application Number !"></asp:RequiredFieldValidator> 

            </div>
        <asp:Button ID="btnXML" runat="server" Text="Get xml" OnClick="btnXML_Click"  CssClass="btn btn-primary" />
        <br /><asp:Label ID="lblMessage" runat="server" Text="message" ForeColor="Red"></asp:Label>
        <br /><asp:Label ID="lblcode" runat="server" Text="code" ForeColor="Red"></asp:Label>
        <br /><asp:TextBox ID="txtarea" runat="server" TextMode="MultiLine" Height="500px" Width="1500"></asp:TextBox>
    </div>
    
        
</asp:Content>
