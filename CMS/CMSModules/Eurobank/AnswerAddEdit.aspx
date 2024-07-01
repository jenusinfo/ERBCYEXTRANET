<%@ Page Title="" Language="C#" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" AutoEventWireup="true" CodeBehind="AnswerAddEdit.aspx.cs" Theme="Default" Inherits="CMSModules_Eurobank_AnswerAddEdit" %>

<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<%@ Register Namespace="CMS.UIControls.UniGridConfig" TagPrefix="ug" Assembly="CMS.UIControls" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
        <div class="form-horizontal form-filter">
            <h4>New Answers</h4>
    <asp:HiddenField ID="hdnPollsID" runat="server" />
    <asp:HiddenField ID="hdnAnsID" runat="server" />
              <div class="form-group row">
        <div class="col-sm-2">
            <asp:Button ID="btnSubmitAnswer" CssClass="btn btn-primary" runat="server" Text="Save" OnClick="btnSubmitAnswer_Click" />
        </div>
                   <div class="col-sm-4">
            <asp:Button ID="Button1" CssClass="btn btn-primary" runat="server" Text="New Answer" />
        </div>
    </div>
    <div class="form-group row">
        <label for="inputEmail3" class="col-sm-4 col-form-label">Answer:*</label>
        <div class="col-sm-8">
            <asp:TextBox ID="txtAnswer" runat="server" CssClass="form-control "></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfanswer" ControlToValidate="txtAnswer"  runat="server" ErrorMessage="Please enter answer!"></asp:RequiredFieldValidator>  
        </div>
    </div>
    <div class="form-group row">
        <label for="inputEmail3" class="col-sm-4 col-form-label">Order Level:*</label>
        <div class="col-sm-8">
            <asp:TextBox ID="txtlavel" runat="server" CssClass="form-control "></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rflavel" ControlToValidate="txtlavel" runat="server" ErrorMessage="Please enter order lavel!"></asp:RequiredFieldValidator>  

        </div>
    </div>
 
  

  
    </div>
</asp:Content>

