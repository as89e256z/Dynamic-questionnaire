using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dynamic_questionnaire.Admin
{
    public partial class AdminQuestionnaireList : System.Web.UI.Page
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
            if (this.Session["UserLoginInfo"] is null)
            {
                Response.Redirect("/QuestionnaireList.aspx");
                return;
            }
            string account = this.Session["UserLoginInfo"] as string;
            DataRow dr = DB.DBHelper.GetUserInfoByAccount(account);
            if (dr == null)
            {
                this.Session["UserLoginInfo"] = null;
                Response.Redirect("/QuestionnaireList.aspx");
                return;
            }

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

            var list = DB.DBHelper.GetQuestionnaireList
                (this.Session["UserLoginInfo"] as string, qustnireName, strStr, strEnd);
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




        protected void btnLoginOut_Click(object sender, EventArgs e)
        {
            this.Session["UserLoginInfo"] = null;
            Response.Redirect("/QuestionnaireList.aspx");
        }

        protected void btnCreateQuestionnaire_Click(object sender, EventArgs e)
        {
            Response.Redirect("/SystemAdmin/AdminQuestionnaireContent.aspx");
        }

        protected void btnDeleteQuestionnaire_Click(object sender, EventArgs e)
        {
            if (this.Session["DeleteQstireList"] != null)
            {
                this.ltMsg.Text = "";
                string[] rid = this.Session["DeleteQstireList"].ToString().Split(',');
                string[] deleList;
                deleList = rid.Where(s => !string.IsNullOrEmpty(s)).ToArray();//去空
                DB.DBHelper.DeleteQuestionnaireArray(deleList);
                Response.Redirect(Request.RawUrl);
            }
            else { this.ltMsg.Text = "Please checked need to delete Questionnaire."; }
        }

        protected void gvtypeQuestionnaire(object sender, GridViewRowEventArgs e)
        {
            var questionairetype = e.Row.Cells[4].Text;

            switch ((questionairetype))
            {
                case "0":
                    e.Row.Cells[4].Text = "開放中";
                    break;
                case "1":
                    e.Row.Cells[4].Text = "未開放";
                    break;
                case "2":
                    e.Row.Cells[4].Text = "關閉中";
                    break;
                default:
                    break;
            }
        }

        protected void ckbQuestnb_CheckedChanged(object sender, EventArgs e)
        {
            #region 選中的變色
            CheckBox chk = (CheckBox)sender;
            //以下兩句為 選中背景色       第一種方法通過 Parent 獲得GridViewRow
            DataControlFieldCell dcf = (DataControlFieldCell)chk.Parent;    //這個物件的父類為cell
            GridViewRow gr = (GridViewRow)dcf.Parent;                       //cell的父類就是row，這樣就得到了該checkbox所在的該行

            //另外一種NamingContainer獲得 GridViewRow
            int index = ((GridViewRow)(chk.NamingContainer)).RowIndex;      //通過NamingContainer可以獲取當前checkbox所在容器物件，即gridviewrow
            if (chk.Checked)
            {
                gr.BackColor = System.Drawing.Color.Green;
                if (this.Session["DeleteQstireList"] == null)
                {
                    this.Session["DeleteQstireList"] += this.gvAccountQuestionnaireList.Rows[index].Cells[1].Text.Trim();
                }
                else { this.Session["DeleteQstireList"] += "," + this.gvAccountQuestionnaireList.Rows[index].Cells[1].Text.Trim(); }

            }
            else
            {
                var a = this.gvAccountQuestionnaireList.Rows[index].Cells[1].Text.Trim();
                int aasas = this.Session["DeleteQstireList"].ToString().IndexOf(a);
                if (this.Session["DeleteQstireList"].ToString().IndexOf(a) != -1)
                {
                    var intStr = this.Session["DeleteQstireList"].ToString().IndexOf(a);
                    var intEnd = a.ToString().Length;
                    int dd = intStr + intEnd;
                    int ee;
                    if (dd != this.Session["DeleteQstireList"].ToString().Length)
                    { this.Session["DeleteQstireList"] = this.Session["DeleteQstireList"].ToString().Remove(intStr, intEnd); }
                    else
                    {
                        ee = dd - 2;
                        this.Session["DeleteQstireList"] = this.Session["DeleteQstireList"].ToString().Remove(intStr, intEnd);
                    }
                }

                gr.BackColor = this.gvAccountQuestionnaireList.RowStyle.BackColor;
            }
            #endregion


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
                        msgList.Add("EndTime can't earlier than StartTime.");
                    }
                }
            }

            errorMsgList = msgList;
            if (msgList.Count == 0)
                return true;
            else
                return false;
        }


        #region uc分頁
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
        #endregion

        protected void Button1_Click(object sender, EventArgs e)
        {
            string StartTime = "2021-11-05T09:04";
            DateTime.TryParse(StartTime, out DateTime aaa);
            Response.Write(aaa);
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

            string _url = Request.Url.Authority + Request.Path + template;
            //Response.Write(Request.Path + template);
            //Response.Redirect($"{Request.Path} + {template}", true);
            Response.Redirect($"{Request.Path}{template}", true);
            //Response.Write($"{Request.Url.Authority} + {Request.Path} + {template}");
            //Response.Write("< script> window.location.href('" + Request.Url.Authority + Request.Path + template + "', '_blank'); </ script >");
        }

        /// <summary>
        /// 還原搜索值
        /// </summary>
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

        protected void btnUrl_Click(object sender, EventArgs e)
        {
            Response.Redirect("/SystemAdmin/FrequentlyAskedList.aspx");
        }
    }
}