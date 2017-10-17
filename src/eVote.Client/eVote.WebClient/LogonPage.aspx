<%@ Page Title="" Language="C#" MasterPageFile="~/SiteUnauthorized.Master" AutoEventWireup="true" CodeBehind="LogonPage.aspx.cs" Inherits="eVote.WebClient.LogonPage" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <asp:Label ID="Label1" runat="server" Text="E-mail:"></asp:Label> <br />
        <asp:TextBox CssClass="form-control" ID="LoginTextbox" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" 
            ControlToValidate="LoginTextbox"
            Display="Dynamic" 
            ErrorMessage="Cannot be empty." 
            runat="server" ForeColor="Red" /><br />
        <asp:Label ID="Label2" runat="server" Text="Password:"></asp:Label> <br />
        <asp:TextBox CssClass="form-control" ID="PasswordTextBox" runat="server" TextMode="Password"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" 
            ControlToValidate="PasswordTextBox"
            Display="Dynamic" 
            ErrorMessage="Cannot be empty." 
            runat="server" ForeColor="Red" /><br />
        Remember me  <asp:CheckBox CssClass="checkbox" ID="PersistCheckbox" runat="server" /><br />
        <asp:Button ID="LoginButton" CssClass="btn btn-primary" runat="server" Text="Login" OnClick="Login_Click" /><br />
        <asp:Label ID="InvalidCredentialsLabel" ForeColor="Red" runat="server" Text="" />
    </div>
</asp:Content>
