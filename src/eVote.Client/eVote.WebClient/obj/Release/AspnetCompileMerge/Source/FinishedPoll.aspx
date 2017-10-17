<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FinishedPoll.aspx.cs" Inherits="eVote.WebClient.FinishedPoll" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label runat="server" ID="PollNameLabel" Text="" Font-Bold="true" Font-Size="40px" /><br />
        <asp:Label ID="EndDateLabel" runat="server" Text="" />
        <asp:ListView runat="server" ID="PollOptionsListView" ItemPlaceholderID="itemPlaceholder">
            <LayoutTemplate>
                <h3 style="width: 100%; text-align: left; margin: 10px;">Vote options with vote counts:</h3>
                <ul class="list-group" style="width: 100%; text-align: left; margin: 10px;">
                    <li runat="server" class="list-group-item" id="itemPlaceholder" />
                </ul>
            </LayoutTemplate>
            <ItemTemplate>
                <li runat="server" class="list-group-item"><%# Container.DataItem %></li>
            </ItemTemplate>
        </asp:ListView>
        <p>Please consider the fact that vote count may still be processed.</p>
</asp:Content>
