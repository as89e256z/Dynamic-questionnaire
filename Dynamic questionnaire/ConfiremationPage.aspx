<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfiremationPage.aspx.cs" Inherits="Dynamic_questionnaire.ConfiremationPage" %>

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
                <div id="divQuestionnaireContent" runat="server">
                </div>
                <br />
                <br />
            </div>
            <asp:Literal ID="ltMsg" runat="server"></asp:Literal><br />
            <asp:Button Text="修改" ID="btnCancel" runat="server" OnClick="btnCancel_Click"/>
            <asp:Button Text="真-送出" ID="btnSumit" runat="server" OnClick="btnSumit_Click" />

        </div>
    </form>
</body>
</html>
