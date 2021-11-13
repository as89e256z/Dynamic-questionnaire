<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuestionnaireContent.aspx.cs" Inherits="Dynamic_questionnaire.QuestionnaireContent" %>

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
                描述內容：<asp:Literal ID="ltlQuestionnaireDescribe" runat="server" /><br />
                姓名：<asp:TextBox ID="txtFillerName" runat="server" /><br />
                手機：<asp:TextBox ID="txtPhone" runat="server" /><br />
                Email：<asp:TextBox ID="txtEmail" runat="server" TextMode="Email"/><br />
                年齡：<asp:TextBox ID="txtAges" runat="server" /><br />
                <br />
                <div id="divQuestionnaireContent" runat="server">
                </div>
                <br />
                <br />
            </div>
            <asp:Literal ID="ltMsg" runat="server"></asp:Literal><br />
            <asp:Button Text="取消" ID="btnCancel" runat="server" OnClick="btnCancel_Click" />
            <asp:Button Text="送出" ID="btnSumit" runat="server" OnClick="btnSumit_Click" />
        </div>
    </form>
</body>
</html>
