<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FillerAns.aspx.cs" Inherits="Dynamic_questionnaire.Admin.FillerAns" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
                問卷名稱：<asp:Literal ID="ltlQuestionnaireName" runat="server" /><br />
                姓名：<asp:Literal ID="ltlFillerName" runat="server" /><br />
                手機：<asp:Literal ID="ltlPhone" runat="server" /><br />
                Email：<asp:Literal ID="ltlEmail" runat="server" /><br />
                年齡：<asp:Literal ID="ltlAges" runat="server" /><br />
                <br />
                <asp:Literal ID="ltlCreateTime" Text="填寫時間：" runat="server" />
                <br />
                <br />
                <br />
                <br />
                <div id="divQuestionnaireContent" runat="server">
                </div>
                <br />
                <br />
                <br />
                <br />
            </div>
            <asp:Literal ID="ltMsg" runat="server"></asp:Literal><br />
            <asp:Button Text="返回" ID="btnBack" Onclick="btnBack_Click" runat="server" />

        </div>
    </form>
</body>
</html>
