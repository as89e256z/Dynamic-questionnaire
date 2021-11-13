<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuestionnaireList.aspx.cs" Inherits="Dynamic_questionnaire.QuestionnaireList" %>

<%@ Register Src="~/ucPager.ascx" TagPrefix="uc1" TagName="ucPager" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>我是前台</h1>
        <asp:Button runat="server" Text="Login" OnClick="btnLogin_Click" />
        <div>
            &nbsp;&nbsp;<asp:Label Text="問卷標題" runat="server" />&nbsp;
            <asp:TextBox ID="txtQuestionnaireName" runat="server" /><br />
            <br />
            &nbsp;&nbsp;<asp:Label Text="開始／結束" runat="server" />&nbsp;
            <asp:TextBox ID="txtStartTime" runat="server" TextMode="DateTimeLocal" />
            &nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="txtEndTime" runat="server" TextMode="DateTimeLocal" /><br />
            <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
        </div>
        <asp:Literal ID="ltMsg" runat="server" />

        <div>
            <br />

            <asp:GridView runat="server" ID="gvAccountQuestionnaireList" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" OnRowDataBound="gvtypeQuestionnaire">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField HeaderText="問卷編號" DataField="QuestionnaireNumber" />
                    <asp:TemplateField HeaderText="問卷名稱">
                        <ItemTemplate>
                            <a id="idHrf" href="QuestionnaireContent.aspx?QuestionnaireNumber=<%# Eval("QuestionnaireNumber") %>">
                                <%# Eval("QuestionnaireName") %></a>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField HeaderText="狀態" DataField="State" />
                    <asp:BoundField HeaderText="開始時間" DataField="StartTime" />
                    <asp:BoundField HeaderText="結束時間" DataField="EndTime" />
                    <asp:TemplateField HeaderText="觀看統計">
                        <ItemTemplate>
                            <a href="Statistics.aspx?QuestionnaireName=<%# Eval("QuestionnaireName") %>">點我進<%# Eval("QuestionnaireName") %></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                <SortedAscendingCellStyle BackColor="#FDF5AC" />
                <SortedAscendingHeaderStyle BackColor="#4D0000" />
                <SortedDescendingCellStyle BackColor="#FCF6C0" />
                <SortedDescendingHeaderStyle BackColor="#820000" />
            </asp:GridView>
            <uc1:ucPager runat="server" ID="ucPager" PageSize="10" CurrentPage="1" TotalSize="10"
                Url="QuestionnaireList.aspx" />

            <asp:PlaceHolder runat="server" ID="plcNoData" Visible="false">
                <p style="color: red">
                    No data.
                </p>
            </asp:PlaceHolder>
            <br />
            <br />
        </div>
    </form>
</body>
</html>
