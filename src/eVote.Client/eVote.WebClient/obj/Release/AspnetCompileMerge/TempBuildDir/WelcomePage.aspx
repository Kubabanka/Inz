<%@ Page Title="" Language="C#" MasterPageFile="~/SiteUnauthorized.Master" AutoEventWireup="true" CodeBehind="WelcomePage.aspx.cs" Inherits="eVote.WebClient.WelcomePage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="text-align:center" >
        <h1>Hello!</h1>
        <h2>
            <a href="LogonPage.aspx">Sign in</a> to see your polls.</h2>
      <h3><a href="RegisterPage.aspx">Sign up</a> if you don't have an account.</h3>
    </div>
</asp:Content>
