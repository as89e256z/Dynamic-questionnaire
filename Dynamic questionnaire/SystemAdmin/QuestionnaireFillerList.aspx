<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuestionnaireFillerList.aspx.cs" Inherits="Dynamic_questionnaire.SystemAdmin.QuestionnaireFillerList" %>

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
            <asp:Button Text="匯出" ID="btnTocsv" OnClick="btnTocsv_Click" runat="server" />
            <asp:GridView ID="gvQustireFillerList" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="姓名" />
                    <asp:BoundField HeaderText="填寫時間" DataField="CreateTime" />
                    <asp:TemplateField HeaderText="觀看作答">
                        <ItemTemplate>
                            <a href="FillerAns.aspx?Name=<%# Eval("Name") %>">統計<%# Eval("Name") %></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="觀看作答">
                        <ItemTemplate>
                            <input id="btnQuttionEdit" type="button"  
                                value="觀看<%# DataBinder.Eval(Container.DataItem,"Name") %>"
                                onclick="<%# "javascript:ChangeHref("+"'" + Eval("Name")+"'" +");" %>" />
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
        </div>

        <asp:PlaceHolder runat="server" ID="plcNoData" Visible="false">
            <p style="color: red">
                No data.
            </p>
        </asp:PlaceHolder>
        <script>

            //傳ID工具"/SystemAdmin/QuestionEdit.aspx"
            function ChangeHref( Name) {
                var urldest = ChangeURLArg(window.location.href, "Name", Name);
                //history.pushState(null, null, urldest);
                var href = ChangeURL("/SystemAdmin/FillerAns.aspx", urldest);
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
