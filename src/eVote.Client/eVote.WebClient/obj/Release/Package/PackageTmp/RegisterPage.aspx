<%@ Page Title="" Language="C#" MasterPageFile="~/SiteUnauthorized.Master" AutoEventWireup="true" CodeBehind="RegisterPage.aspx.cs" Inherits="eVote.WebClient.RegisterPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="font-family: Calibri; font-size: 20px; font-weight: normal; font-style: normal; font-variant: normal; text-transform: none; vertical-align: middle; text-align: left; padding: 5px; margin: 5px; position: relative; width: auto; height: auto">
        <h1 style="text-align: center">Sign up</h1>
        <div class="row" style="margin: 0 auto; text-align: left;">
            <table style="width: 100%">
                <tr>
                    <td style="text-align: right">E-mail:
                    </td>
                    <td style="text-align: left; width: 400px">
                        <asp:TextBox CssClass="form-control" ID="LoginTextbox" Style="width: 400px" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="LoginTextbox" Display="Dynamic" ErrorMessage="Cannot be empty." runat="server" ForeColor="Red" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">First name:
                    </td>
                    <td style="width: 400px">
                        <asp:TextBox CssClass="form-control" ID="FirstNameTextBox" Style="width: 400px" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="FirstNameTextBox" Display="Dynamic" ErrorMessage="Cannot be empty." runat="server" ForeColor="Red" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">Last Name: 
                    </td>
                    <td>
                        <asp:TextBox CssClass="form-control" ID="LastNameTextbox" Style="width: 400px" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="LastNameTextbox" Display="Dynamic" ErrorMessage="Cannot be empty." runat="server" ForeColor="Red" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label2" runat="server" Text="Password: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox CssClass="form-control" ID="Password1TextBox" Style="width: 400px" runat="server" TextMode="Password"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="Password1TextBox" Display="Dynamic" ErrorMessage="Cannot be empty." runat="server" ForeColor="Red" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label3" runat="server" Text="Confirm password: " />
                    </td>
                    <td>
                        <asp:TextBox CssClass="form-control" ID="Password2TextBox" runat="server" Style="width: 400px" TextMode="Password"></asp:TextBox>
                    </td>
                    <td>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Passwords don't match." ForeColor="Red" ControlToCompare="Password1TextBox" ControlToValidate="Password2TextBox"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center" colspan="3">
                        <asp:Button CssClass="btn btn-primary" ID="RegisterButton" Style="margin: 10px" runat="server" Text="Sign me up!" OnClick="RegisterButton_Click" />
                        <asp:Button CssClass="btn btn-primary" ID="CancelButton" Style="margin: 10px" runat="server" Text="Take me back" OnClick="CancelButton_Click" CausesValidation="False" />
                    </td>
                </tr>
            </table>
        </div>

    </div>
</asp:Content>
