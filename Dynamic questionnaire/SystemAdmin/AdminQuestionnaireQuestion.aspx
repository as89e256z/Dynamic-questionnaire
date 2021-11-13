<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminQuestionnaireQuestion.aspx.cs" Inherits="Dynamic_questionnaire.SystemAdmin.AdminQuestionnaireQuestion" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            &nbsp;
            <asp:Label Text="種類" runat="server" />&nbsp;
            <select id="seleProAsked" runat="server">
                <option value="0">自訂問題</option>
                <option value="1">常用問題</option>
            </select>
            <br />
            &nbsp;&nbsp;<asp:Label Text="問題" runat="server" />&nbsp;
            <asp:TextBox runat="server" ID="txtProblemTitle"></asp:TextBox>&nbsp;&nbsp;&nbsp;
            <select id="seleType" runat="server">
                <option value="0">單選題</option>
                <option value="1">複選題</option>
                <option value="2">填空題</option>
            </select>&nbsp;&nbsp;
            <asp:CheckBox Text="必填" runat="server" ID="ckbRequired" />
            <br />
            &nbsp;&nbsp;<asp:Label Text="回答" runat="server" />&nbsp;
            <asp:TextBox runat="server" ID="txtAns"></asp:TextBox>&nbsp;(多個答案以；分隔)&nbsp;&nbsp;&nbsp;
            <asp:Button Text="加入" runat="server" ID="btnInput" OnClick="btnInput_Click" />
        </div>

        <div>
            <asp:GridView ID="gvQuestionnaireQuestionList" runat="server" OnRowDataBound="gvQuestionType" AutoGenerateColumns="False" >
                <Columns>

                    <asp:BoundField DataField="QuestionID" HeaderText="#" />
                    <asp:BoundField DataField="ProblemTitle" HeaderText="問題" />
                    <asp:BoundField DataField="TypeOfProblem" HeaderText="題型" />
                    <asp:CheckBoxField DataField="Required" HeaderText="必填" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <input id="btnQuttionEdit" name="<%# DataBinder.Eval(Container.DataItem,"QuestionID") %>" type="button" 
                                value="編輯<%# DataBinder.Eval(Container.DataItem,"ProblemTitle") %>"
                                onclick="<%# "javascript:ChangeHref(" +"'"+ Eval("QuestionID")+"'" + ");" %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ProblemID" HeaderText="ProblemID" />
                </Columns>
            </asp:GridView>

            <asp:PlaceHolder runat="server" ID="plcNoData" Visible="false">
                <p style="color: red">
                    No data.
                </p>
            </asp:PlaceHolder>
            <asp:Button Text="取消" ID="btnCancel" runat="server" OnClick="btnCancel_Click" />
            <asp:Button Text="送出" ID="btnSumit" runat="server" OnClick="btnSumit_Click" />
        </div>
        <asp:Literal id="ltMsg" runat="server" />

        <script>

            //傳ID工具"/SystemAdmin/QuestionEdit.aspx"
            function ChangeHref(QuestionID) {
                var urldest = ChangeURLArg(window.location.href, "QuestionID", QuestionID);
                //history.pushState(null, null, urldest);
                var href = ChangeURL("/SystemAdmin/AdminQuestionnaireQuestion.aspx", urldest);
                window.location.href = 'http://' + href;
            }
            /**
             * 改URL參數
             * @param url = window.location.href
             * @param arg = 參數名稱
             * @param arg_val = 參數
             */
            function ChangeURLArg(url, arg, arg_val) {
                var pattern = arg + '=([^&]*)';
                var replaceText = arg + '=' + arg_val;
                if (url.match(pattern)) {
                    var tmp = '/(' + arg + '=)([^&]*)/gi';
                    tmp = url.replace(eval(tmp), replaceText);
                    return tmp;
                } else {
                    if (url.match('[\?]')) {
                        return url + '&' + replaceText;
                    } else {
                        return url + '?' + replaceText;
                    }
                }
                return url + '\n' + arg + '\n' + arg_val;
            }

            function ChangeURL(url, hrefurl) {
                let bang = new URL(hrefurl);
                var head = bang.host;
                var butt = bang.search;
                tmp = head + url + butt;
                return tmp;
            }
        </script>

    </form>
</body>
</html>
