<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminQuestionnaireContent.aspx.cs" Inherits="Dynamic_questionnaire.Admin.AdminQuestionnaireContent" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            問卷名稱：<asp:TextBox ID="txtQuestionnaireName" runat="server" /><br />
            描述內容：<asp:TextBox ID="txtQuestionnaireDescribe" MaxLength="100" runat="server" /><br />
            開始時間：<asp:TextBox ID="txtStartTime" runat="server" TextMode="DateTimeLocal" /><br />
            結束時間：<asp:TextBox ID="txtEndTime" runat="server" TextMode="DateTimeLocal" /><br />
            &nbsp;&nbsp;&nbsp;<asp:CheckBox ID="ckbEnable" Text="啟用" runat="server" /><br />
            <asp:Button Text="取消" ID="btnCancel" runat="server" OnClick="btnCancel_Click" />
            <asp:Button Text="送出" ID="btnSumit" runat="server" OnClick="btnSumit_Click" />
            <asp:Button Text="修改" Visible="false" ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" /><br />
        </div>
            <asp:Literal ID="ltMsg" runat="server"></asp:Literal>
        <div>
        </div>
    </form>
</body>
</html>
