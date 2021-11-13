<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrequentlyAskedList.aspx.cs" Inherits="Dynamic_questionnaire.SystemAdmin.FrequentlyAskedList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>常用問題一覽</h1>
            <asp:GridView runat="server" ID="gvQuestionUsedList" AutoGenerateColumns="False" OnRowDataBound="gvQuestionType">
                <Columns>
                    <asp:BoundField DataField="ProblemTitle" HeaderText="題目標題" />
                    <asp:BoundField DataField="TypeOfProblem" HeaderText="題種" />
                    <asp:TemplateField HeaderText="答案">
                        <ItemTemplate>
                            <select>
                                <option value="0"><%# Eval("Ans1") %></option>
                                <option value="1"><%# Eval("Ans2") %></option>
                                <option value="2"><%# Eval("Ans3") %></option>
                                <option value="3"><%# Eval("Ans4") %></option>
                                <option value="4"><%# Eval("Ans5") %></option>
                                <option value="5"><%# Eval("Ans6") %></option>
                                <option value="6"><%# Eval("Ans7") %></option>
                                <option value="7"><%# Eval("Ans8") %></option>
                                <option value="8"><%# Eval("Ans9") %></option>
                            </select>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <asp:PlaceHolder runat="server" ID="plcNoData" Visible="false">
                <p style="color: red">
                    No data in your Accounting Note.
                </p>
            </asp:PlaceHolder>
            <asp:Button Text="返回" ID="btnBack" runat="server" OnClick="btnBack_Click" />
        </div>
    </form>
</body>
</html>
