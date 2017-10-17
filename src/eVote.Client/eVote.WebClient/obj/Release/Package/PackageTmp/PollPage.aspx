<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PollPage.aspx.cs" Inherits="eVote.WebClient.PollPage" %>

<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container" style="text-align: center">
        <asp:Label runat="server" ID="PollNameLabel" Text="" Font-Bold="true" Font-Size="40px" /><br />
        <asp:Label ID="EndDateLabel" runat="server" Text="" />
        <asp:ListView runat="server" ID="PollOptionsListView" ItemPlaceholderID="itemPlaceholder">
            <LayoutTemplate>
                <h3 style="width: 100%; text-align: left; margin: 10px;">Vote options:</h3>
                <ul class="list-group" style="width: 100%; text-align: left; margin: 10px;">
                    <li runat="server" class="list-group-item" id="itemPlaceholder" />
                </ul>
            </LayoutTemplate>
            <ItemTemplate>
                <li runat="server" class="list-group-item"><%# Container.DataItem %></li>
            </ItemTemplate>
        </asp:ListView>
        <table style="text-align: left; margin: 25px; margin: 0 auto; width: 70%">
            <tr style="width: 100%">
                <td>
                    <p>First choice</p>
                </td>
                <td style="width: 80%">
                    <asp:DropDownList ID="FirstChoiceDropDownList" CssClass="col-xs-5 form-control dropdown-toggle" Style="width: 100%" runat="server" DataTextField="Name" DataValueField="ID" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select an option" Value="-1" Selected="True" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <p>Second choice</p>
                </td>
                <td>
                    <asp:DropDownList ID="SecondChoiceDropDownList" CssClass="col-xs-5 form-control dropdown-toggle" Style="width: 100%" runat="server" DataTextField="Name" DataValueField="ID" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select an option" Value="-1" Selected="True" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <p>Third choice</p>
                </td>
                <td>
                    <asp:DropDownList ID="ThirdChoiceDropDownList" CssClass="col-xs-5 form-control dropdown-toggle" Style="width: 100%" runat="server" DataTextField="Name" DataValueField="ID" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select an option" Value="-1" Selected="True" />
                    </asp:DropDownList>
                </td>
            </tr>
        </table>

        <asp:Label ID="CheckLabel" runat="server" ForeColor="Red" Text="" />
        <br />
        <asp:Button ID="VoteButton" runat="server" Text="Vote" CssClass="btn btn-primary" OnClick="VoteButton_Click" />
        <br />
        <asp:Label ID="LastVoteLabel" runat="server" Text="" />


    </div>
</asp:Content>
