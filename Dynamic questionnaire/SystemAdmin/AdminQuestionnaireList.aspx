<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminQuestionnaireList.aspx.cs" Inherits="Dynamic_questionnaire.Admin.AdminQuestionnaireList" %>

<%@ Register Src="~/ucPager.ascx" TagPrefix="uc1" TagName="ucPager" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>我是後台</h1>
        <asp:Button Text="LoginOut" ID="btnLoginOut" align="right" OnClick="btnLoginOut_Click" runat="server" />
        <div>
            &nbsp;&nbsp;<asp:Label Text="問卷標題" runat="server" />&nbsp;
            <asp:TextBox ID="txtQuestionnaireName" runat="server" /><br />
            <br />
            &nbsp;&nbsp;<asp:Label Text="開始／結束" runat="server" />&nbsp;
            <asp:TextBox ID="txtStartTime" runat="server" TextMode="DateTimeLocal" />
            &nbsp;&nbsp;<asp:Literal Text="～" runat="server" />&nbsp;&nbsp;
            <asp:TextBox ID="txtEndTime" runat="server" TextMode="DateTimeLocal" /><br />
            <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
        </div>
        <br />
        <br />
        <asp:Literal ID="ltMsg" runat="server" />
        <br />
        <br />
        <asp:Button Text="Add Questionnaire" runat="server" ID="btnCreateQuestionnaire" OnClick="btnCreateQuestionnaire_Click" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button Text="Delete Questionnaire" runat="server" ID="btnDeleteQuestionnaire" OnClientClick="return confirm('Are you sure?');" OnClick="btnDeleteQuestionnaire_Click" />
        <asp:Button Text="常用問題管理" ID="btnUrl" runat="server" OnClick="btnUrl_Click" />
        <asp:Button ID="Button1" runat="server" Text="TEST" OnClick="Button1_Click" />
        <asp:GridView runat="server" ID="gvAccountQuestionnaireList" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" OnRowDataBound="gvtypeQuestionnaire">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField InsertVisible="false">
                    <ItemTemplate>
                        <%--<input id="Radio" type="radio" name="<STRONG><FONT color=#ff0000>RowSelected</FONT></STRONG>"
                            value='<FONT color=#ff0000><STRONG><%# Eval("QuestionnaireNumber") %></STRONG></FONT>' onclick="ckbQuestnb_CheckedChanged"
                            />
                        <asp:RadioButton value='<%# Eval("QuestionnaireNumber") %>' ID="rdobtnQuestnb"
                            Text='<%# Eval("QuestionnaireNumber") %>' runat="server" 
                            OnCheckedChanged="rdobtnQuestnb_CheckedChanged" AutoPostBack="true"/>--%>
                        <asp:CheckBox value='<%# Eval("QuestionnaireNumber") %>'
                            runat="server" ID="ckbQuestnb" OnCheckedChanged="ckbQuestnb_CheckedChanged" AutoPostBack="true" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="問卷編號" DataField="QuestionnaireNumber" />
                <asp:TemplateField HeaderText="問卷名稱">
                    <ItemTemplate>
                        <a href="AdminQuestionnaireQuestion.aspx?QuestionnaireNumber=<%# Eval("QuestionnaireNumber") %>">
                            <%# Eval("QuestionnaireName") %></a>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField HeaderText="建立人" DataField="CreateAccount" />
                <asp:BoundField HeaderText="狀態" DataField="State" />
                <asp:BoundField HeaderText="開始時間" DataField="StartTime" />
                <asp:BoundField HeaderText="結束時間" DataField="EndTime" />
                <asp:BoundField HeaderText="建立時間" DataField="CreateTime" />
                <asp:TemplateField HeaderText="編輯問卷">
                    <ItemTemplate>
                        <a href="AdminQuestionnaireContent.aspx?QuestionnaireNumber=<%# Eval("QuestionnaireNumber") %>">點我進<%# Eval("QuestionnaireName") %></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="觀看回應">
                    <ItemTemplate>
                        <a href="QuestionnaireFillerList.aspx?QuestionnaireTitle=<%# Eval("QuestionnaireName") %>">統計填寫人<%# Eval("QuestionnaireName") %></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="真-統計回應">
                    <ItemTemplate>
                        <a href="/SystemAdmin/AdminQuestionnaireStatistics.aspx?QuestionnaireTitle=<%# Eval("QuestionnaireName") %>">統計<%# Eval("QuestionnaireName") %></a>
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
        <br />
        <uc1:ucPager runat="server" ID="ucPager" PageSize="10" CurrentPage="1" TotalSize="10"
            Url="AdminQuestionnaireList.aspx" />
        <asp:PlaceHolder runat="server" ID="plcNoData" Visible="false">
            <p style="color: red">
                No data in your Accounting Note.
            </p>
        </asp:PlaceHolder>
        <asp:Button Text="取消" align="right" runat="server" />
        <asp:Button Text="送出" align="right" runat="server" />
    </form>
</body>
</html>
