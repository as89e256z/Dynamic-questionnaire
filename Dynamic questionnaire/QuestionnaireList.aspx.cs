using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dynamic_questionnaire
{
    public partial class QuestionnaireList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)//非第一次載入時
            {
                LoadPageAndData();
                RestoreParameters();
            }

        }

        private void LoadPageAndData()
        {
            string qustnireName, strStr, strEnd;
            qustnireName = strStr = strEnd = "";
            if (Request.QueryString["QuestionnaireName"] != null &&
                string.Empty != Request.QueryString["QuestionnaireName"])
            {
                qustnireName = Request.QueryString["QuestionnaireName"].ToString();
            }
            DateTime dtStr, dtEnd;
            if (Request.QueryString["StartTime"] != null &&
                DateTime.TryParseExact(Request.QueryString["StartTime"].ToString(), "yyyy-MM-dd" + "T" + "hh:mm", null, System.Globalization.DateTimeStyles.None, out dtStr))
            {
                strStr = Request.QueryString["StartTime"].ToString();
            }
            if (Request.QueryString["EndTime"] != null &&
            DateTime.TryParseExact(Request.QueryString["EndTime"].ToString(), "yyyy-MM-dd" + "T" + "hh:mm", null, System.Globalization.DateTimeStyles.None, out dtEnd))
            {
                strEnd = Request.QueryString["EndTime"].ToString();
            }
            string account = "";
            var list = DB.DBHelper.GetQuestionnaireList(account, qustnireName, strStr, strEnd); ;
            if (list.Count > 0)
            {
                var pageList = this.GetPagedDataTable(list);
                this.gvAccountQuestionnaireList.DataSource = pageList;
                this.gvAccountQuestionnaireList.DataBind();
                this.ucPager.TotalSize = list.Count;
                this.ucPager.Bind();
            }
            else
            {
                this.gvAccountQuestionnaireList.Visible = false;
                this.plcNoData.Visible = true;
            }
        }

        private void RestoreParameters()
        {
            string strQuestionnaireName = Request.QueryString["QuestionnaireName"];
            string strStr = Request.QueryString["StartTime"];
            string strEnd = Request.QueryString["EndTime"];

            if (!string.IsNullOrEmpty(strQuestionnaireName))
                this.txtQuestionnaireName.Text = strQuestionnaireName;

            if (!string.IsNullOrEmpty(strStr))
                this.txtStartTime.Text = strStr;
            if (!string.IsNullOrEmpty(strEnd))
                this.txtEndTime.Text = strEnd;
        }


        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Login.aspx");
        }


        protected void gvtypeQuestionnaire(object sender, GridViewRowEventArgs e)
        {
            var questionairetype = e.Row.Cells[2].Text;

            switch ((questionairetype))
            {
                case "0":
                    e.Row.Cells[2].Text = "投票中";
                    break;
                case "1":
                    e.Row.Cells[2].Text = "尚未開始";
                    break;
                case "2":
                    e.Row.Cells[2].Text = "已完結";
                    break;
                default:
                    break;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            List<string> msgList = new List<string>();
            if (!this.CheckInput(out msgList))
            {
                this.ltMsg.Text = string.Join("<br/>", msgList);
                return;
            }
            string template = "?";
            string strQuestionnaireName = this.txtQuestionnaireName.Text;
            if (!string.IsNullOrEmpty(strQuestionnaireName))
                template += "&QuestionnaireName=" + strQuestionnaireName;


            if (!string.IsNullOrWhiteSpace(this.txtStartTime.Text))
            {
                DateTime dtStr = Convert.ToDateTime(this.txtStartTime.Text);
                string strStr = dtStr.ToString("yyyy-MM-dd" + "T" + "hh:mm");
                if (!string.IsNullOrEmpty(strStr))
                    template += "&StartTime=" + strStr;
            }
            if (!string.IsNullOrWhiteSpace(this.txtEndTime.Text))
            {
                DateTime dtEnd = Convert.ToDateTime(this.txtEndTime.Text);
                string strEnd = dtEnd.ToString("yyyy-MM-dd" + "T" + "hh:mm");
                if (!string.IsNullOrEmpty(strEnd))
                    template += "&EndTime=" + strEnd;
            }

            //Response.Redirect($"{Request.Url.Authority}{Request.Path}{template}");
            Response.Redirect($"{Request.Path}{template}");
        }

        private bool CheckInput(out List<string> errorMsgList)
        {
            List<string> msgList = new List<string>();

            //檢查QuestionnaireName
            if (!string.IsNullOrWhiteSpace(this.txtQuestionnaireName.Text))
            {
                if (this.txtQuestionnaireName.Text.Length > 100)
                {
                    msgList.Add("QuestionnaireName can't over 100 characters.");
                }
            }


            //檢查StartTime
            if (!string.IsNullOrWhiteSpace(this.txtStartTime.Text))
            {
                //檢查EndTime
                if (!string.IsNullOrWhiteSpace(this.txtEndTime.Text))
                {
                    DateTime dtStr = Convert.ToDateTime(this.txtStartTime.Text);
                    DateTime dtEnd = Convert.ToDateTime(this.txtEndTime.Text);
                    if (DateTime.Compare(dtEnd, dtStr) < 0)
                    {
                        msgList.Add("StartTime can't earlier than EndTime or EndTime can't earlier than StartTime.");
                    }
                }
            }
            errorMsgList = msgList;
            if (msgList.Count == 0)
                return true;
            else
                return false;
        }


        private List<DBModels.Questionnaire> GetPagedDataTable(List<DBModels.Questionnaire> list)
        {
            int startIndex = (this.GetCurrentPage() - 1) * 10;
            return list.Skip(startIndex).Take(10).ToList();
        }

        private int GetCurrentPage()
        {
            string pagetext = Request.QueryString["page"];
            if (string.IsNullOrWhiteSpace(pagetext))
            {
                return 1;
            }
            int intpage;
            if (!int.TryParse(pagetext, out intpage))
            {
                return 1;
            }
            if (intpage <= 0)
            {
                return 1;
            }
            else { return intpage; }
        }

    }
}