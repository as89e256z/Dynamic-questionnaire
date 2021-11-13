<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Statistics.aspx.cs" Inherits="Dynamic_questionnaire.Statistics" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="css/bootstrap.css" rel="stylesheet" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div id="divChoiceProgressContent" style="width: 80%; margin: 0px auto;" runat="server">
            </div>
        </div>
        <asp:PlaceHolder runat="server" ID="plcNoData" Visible="false">
            <p style="color: red">
                No data.
            </p>
        </asp:PlaceHolder>
        <asp:Button Text="返回" ID="btnBack" OnClick="btnBack_Click" runat="server" />
    </form>
</body>
</html>
