<%@ Page Title="" Language="C#" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" AutoEventWireup="true" CodeBehind="PollsAddEdit.aspx.cs" Theme="Default" Inherits="CMSModules_Eurobank_PollsAddEdit" %>

<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<%@ Register Namespace="CMS.UIControls.UniGridConfig" TagPrefix="ug" Assembly="CMS.UIControls" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
        <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
     <script type="text/javascript">
         $(function () {

             $(".datePicker").datepicker({ minDate: new Date(), dateFormat: 'mm/dd/yy' });

         })
    </script>
    <div class=" col-sm-5">
        <asp:HiddenField ID="hdnPollsID" runat="server" />
       
        <div class="form-group row">
            <div class="col-sm-10">
                <asp:Button ID="btnPolls" CssClass="btn btn-primary" runat="server" Text="Save" OnClick="btnPolls_Click" />
            </div>
        </div>
         <h4>General</h4>
        <div class="form-group row">
            <label for="inputEmail3" class="col-sm-4 col-form-label">Poll Display name:*</label>
            <div class="col-sm-8">
                <asp:TextBox ID="txtName" runat="server" CssClass="form-control "></asp:TextBox>
    <asp:RequiredFieldValidator ID="rflavel" ControlToValidate="txtName" runat="server" ErrorMessage="Please enter display name!"></asp:RequiredFieldValidator>  

            </div>
        </div>
        <div class="form-group row">
            <label for="inputEmail3" class="col-sm-4 col-form-label">Question:*</label>
            <div class="col-sm-8">
                <asp:TextBox ID="txtQuestion" runat="server" CssClass="form-control "></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtQuestion" runat="server" ErrorMessage="Please enter question!"></asp:RequiredFieldValidator>  

            </div>

        </div>
        <div class="form-group row">
            <label for="inputPassword3" class="col-sm-4 col-form-label">Message after vote:</label>
            <div class="col-sm-8">
                <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control "></asp:TextBox>
            </div>
        </div>
        <h4>Advanced settings</h4>
        <div class="form-group row">
            <label for="inputPassword3" class="col-sm-4 col-form-label">Open from:</label>
            <div class="col-sm-8">
                <asp:TextBox ID="txtOpenDate" runat="server" TextMode="DateTime" CssClass="form-control datePicker"></asp:TextBox>
            </div>
        </div>
        <div class="form-group row">
            <label for="inputPassword3" class="col-sm-4 col-form-label">To from:</label>
            <div class="col-sm-8">
                <asp:TextBox ID="txtToDate" runat="server" TextMode="DateTime" CssClass="form-control datePicker"></asp:TextBox>
            </div>
        </div>
        <div class="form-group row">
            <label for="inputPassword3" class="col-sm-4 col-form-label">Active:</label>
            <div class="col-sm-8">
                <asp:CheckBox ID="cbActive" runat="server" CssClass="form-check-input "/>
            </div>
        </div>
    
        </div>
        <div class="col-sm-7">
        <asp:Panel ID="pnlAnswer" runat="server">
            
            <asp:Button ID="btnNewPollsAns" CssClass="btn btn-primary" runat="server" Text="New Answer " OnClick="btnPollsAns_Click" /><br /><br />
            <cms:UniGrid ID="PollsAnsList" runat="server" Columns="ItemID,AnswerText,AnswerOrder,AnswerCount,AnswerEnabled,AnswerPollID">

                <GridActions>
                    <ug:Action Name="edit" Caption="$General.Edit$" FontIconClass="icon-edit" FontIconStyle="allow" />
                    <ug:Action Name="deleteaction" Caption="$General.Delete$" Icon="Delete.png"  Confirmation="$General.ConfirmDelete$"/>
                </GridActions>
                <GridColumns>
                    <ug:Column Source="AnswerText" Caption="Answer" />
                    <ug:Column Source="AnswerCount" Caption="Count" />
                    <ug:Column Source="AnswerOrder" Caption="Order level" />
                    <ug:Column Source="AnswerEnabled" Caption="Active" />
                </GridColumns>

            </cms:UniGrid>
        </asp:Panel>
    </div>
  
</asp:Content>

