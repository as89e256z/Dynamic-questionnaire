<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrequentlyAskedManagementPage.aspx.cs" Inherits="Dynamic_questionnaire.SystemAdmin.FrequentlyAskedManagementPage" %>

<%@ Register Src="~/ucPager.ascx" TagPrefix="uc1" TagName="ucPager" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            &nbsp;&nbsp;Frequently Asked Questions&nbsp;&nbsp;
            <asp:Literal ID="ltlProID" runat="server" /><br />
            問題：<asp:TextBox ID="txtProblemTitle" runat="server" /><br />
            題型：
            <asp:DropDownList ID="ddlTypeOfProblem" runat="server">
                <asp:ListItem Value="0">單選題</asp:ListItem>
                <asp:ListItem Value="1">複選題</asp:ListItem>
                <asp:ListItem Value="2">填空題</asp:ListItem>
            </asp:DropDownList>&nbsp;&nbsp;
            <br />
            回答：<asp:TextBox ID="txtAns" runat="server" /><br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:CheckBox ID="ckbRequired" Text="必填" runat="server" /><br />
            <asp:Button Text="取消" ID="btnCancel" runat="server" OnClick="btnCancel_Click" />&nbsp;&nbsp;&nbsp;
            <asp:Button Text="儲存" ID="btnSumitSession" runat="server" OnClick="btnSumit_Click" />
            <br />
        </div>
        <asp:GridView runat="server" ID="gvQuestionUsedList" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="ProblemTitle" HeaderText="題目標題" />
                <asp:BoundField DataField="TypeOfProblem" HeaderText="題種" />
                <asp:TemplateField HeaderText="答案">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlAns" runat="server">
                            <asp:ListItem Value="0"></asp:ListItem>
                            <asp:ListItem Value="1"></asp:ListItem>
                            <asp:ListItem Value="2"></asp:ListItem>
                            <asp:ListItem Value="3"></asp:ListItem>
                            <asp:ListItem Value="4"></asp:ListItem>
                            <asp:ListItem Value="5"></asp:ListItem>
                            <asp:ListItem Value="6"></asp:ListItem>
                            <asp:ListItem Value="7"></asp:ListItem>
                            <asp:ListItem Value="8"></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <asp:PlaceHolder runat="server" ID="plcNoData" Visible="false">
            <p style="color: red">
                No data.
            </p>
        </asp:PlaceHolder>

        <uc1:ucPager runat="server" ID="ucPager" PageSize="10" CurrentPage="1" TotalSize="10"
            Url="FrequentlyAskedManagementPage.aspx" />

        <asp:Literal ID="ltMsg" runat="server"></asp:Literal>
    </form>
</body>
</html>
