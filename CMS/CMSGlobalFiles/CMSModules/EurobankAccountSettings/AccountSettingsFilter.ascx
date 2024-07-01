<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountSettingsFilter.ascx.cs" Inherits="CMSGlobalFiles_CMSModules_EurobankAccountSettings_AccountSettingsFilter" %>
<%@ Register namespace="CMS.Base.Web.UI" assembly="CMS.Base.Web.UI" tagPrefix="cms" %>

<cms:CMSDropDownList ID="ddlLookupItems" runat="server" DataTextField="NodeName" DataValueField="NodeGUID" />