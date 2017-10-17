<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddNewPoll.aspx.cs" Inherits="eVote.WebClient.AddNewPoll" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <label for="PollNameTextbox">Poll name</label>
        <asp:TextBox CssClass="form-control" ID="PollNameTextbox" Style="width: 100%" runat="server"></asp:TextBox>
        <label for="EndDateTextbox">End date: (dd/mm/yyyy HH:MM)</label>
        <asp:TextBox CssClass="form-control" ID="EndDateTextbox" Style="width: 100%" runat="server" TextMode="DateTime"></asp:TextBox>
        <label for="VotersTextbox">Voters e-mails (separated by ;):</label>
        <asp:TextBox CssClass="form-control" ID="VotersTextbox" Style="width: 100%" runat="server"></asp:TextBox>
        <label for="OptionTextbox">Vote options (separated by ;):</label>
        <asp:TextBox CssClass="form-control" ID="OptionTextbox" Style="width: 100%" runat="server"></asp:TextBox><br />
        <asp:Button runat="server" CssClass="btn btn-primary" Text="Add poll" ID="AddButton" OnClick="AddButton_Click" /><br />
        <asp:Label ID="ErrorLabel" ForeColor="Red" runat="server" Text=""></asp:Label><br />
        <asp:Label ID="ResultLabel" runat="server" Text=""></asp:Label>
    </div>
</asp:Content>
