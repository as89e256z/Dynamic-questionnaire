using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;

namespace Dynamic_questionnaire.Admin
{
    public partial class AdminQuestionnaireContent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.ckbEnable.Checked = true;
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
                //抓QuestionnaireName為編輯頁 / 否則為新增頁
                if (this.Request.QueryString["QuestionnaireNumber"] != null)
                {
                    this.btnSumit.Visible = false;
                    this.btnUpdate.Visible = true;
                    string qstName = this.Request.QueryString["QuestionnaireNumber"];

                    var dtqstnaireQuestion = DB.DBHelper.GetQuestionnaire(qstName);
                    if (dtqstnaireQuestion != null)
                    {
                        this.txtQuestionnaireName.Text = dtqstnaireQuestion["QuestionnaireName"].ToString();
                        if (Convert.ToInt32(dtqstnaireQuestion["State"].ToString()) != 0)//狀態值:0->開放中;1->尚未開放;2->關閉中
                        {
                            this.ckbEnable.Checked = false;
                        }

                        DateTime str = Convert.ToDateTime(dtqstnaireQuestion["StartTime"].ToString());
                        this.txtStartTime.Text = str.ToString("yyyy-MM-dd" + "T" + "hh:mm");
                        DateTime end = Convert.ToDateTime(dtqstnaireQuestion["EndTime"].ToString());
                        this.txtEndTime.Text = end.ToString("yyyy-MM-dd" + "T" + "hh:mm");
                        if (dtqstnaireQuestion["QuestionnaireDescribe"].ToString() != null)
                        {
                            this.txtQuestionnaireDescribe.Text = dtqstnaireQuestion["QuestionnaireDescribe"].ToString();
                        }
                    }
                }
                else//這裡是新增頁
                {
                    this.txtStartTime.Text = DateTime.Now.ToString("yyyy-MM-dd" + "T" + "hh:mm");
                }
            }
        }

        protected void btnSumit_Click(object sender, EventArgs e)
        {
            List<string> msgList = new List<string>();
            if (!this.CheckInput(out msgList))
            {
                this.ltMsg.Text = string.Join("<br/>", msgList);
                return;
            }

            string account = this.Session["UserLoginInfo"] as string;
            var dr = DB.DBHelper.GetUserInfoByAccount(account);

            if (dr == null)
            {
                Response.Redirect("/QuestionnaireList.aspx");
                return;
            }
            string questionnairename = txtQuestionnaireName.Text;
            string questionnairedescribe = this.txtQuestionnaireDescribe.Text;
            string creataccount = dr["account"].ToString();
            int state;
            if (this.ckbEnable.Checked)
            {
                state = 0;
            }
            else
            {
                state = 2;
            }
            DateTime starttime = Convert.ToDateTime(this.txtStartTime.Text);
            DateTime endtime = Convert.ToDateTime(this.txtEndTime.Text);
            DateTime CreateTime = DateTime.Now;
            DB.DBHelper.CreateQuestionnaireContent(questionnairename, questionnairedescribe, creataccount, state, starttime, endtime, CreateTime);
            Response.Write("<script>alert('Success Join');</script>");
            Response.Redirect("AdminQuestionnaireList.aspx");
        }

        private bool CheckInput(out List<string> errorMsgList)
        {
            List<string> msgList = new List<string>();

            //檢查QuestionnaireName
            if (string.IsNullOrWhiteSpace(this.txtQuestionnaireName.Text))
            {
                msgList.Add("QuestionnaireName is Required.");
            }
            else
            {
                if (this.txtQuestionnaireName.Text.Length > 100)
                {
                    msgList.Add("QuestionnaireName can't over 100 characters.");
                }
            }

            //檢查QuestionnaireDescribe

            if (this.txtQuestionnaireName.Text.Length > 100)
            {
                msgList.Add("QuestionnaireDescribe can't over 100 characters.");
            }

            //檢查StartTime
            if (string.IsNullOrWhiteSpace(this.txtStartTime.Text))
            {
                msgList.Add("StartTime is Required.");
            }
            else
            {
                //DateTime tem;
                //if (!DateTime.TryParseExact(this.txtStartTime.Text,"G",null, DateTimeStyles.None, out tem))
                //{
                //    msgList.Add("StartTime must be DateTime.");
                //}
                DateTime dt = Convert.ToDateTime(this.txtStartTime.Text);
                if (DateTime.Compare(dt, DateTime.Today) < 0)
                {
                    if (this.Request.QueryString["QuestionnaireNumber"] == null)
                        msgList.Add("StartTime can't earlier than Today.");
                }
            }
            //檢查EndTime
            if (string.IsNullOrWhiteSpace(this.txtEndTime.Text))
            {
                msgList.Add("EndTime is Required.");
            }
            else
            {
                //DateTime tem;
                //if (!DateTime.TryParseExact(this.txtEndTime.Text,"G", null, DateTimeStyles.None, out tem))
                //{
                //    msgList.Add("EndTime must be DateTime.");
                //}
                DateTime dtStr = Convert.ToDateTime(this.txtStartTime.Text);
                DateTime dtEnd = Convert.ToDateTime(this.txtEndTime.Text);
                if (DateTime.Compare(dtEnd, dtStr) < 0)
                {
                    msgList.Add("EndTime can't earlier than StartTime.");
                }
            }
            errorMsgList = msgList;
            if (msgList.Count == 0)
                return true;
            else
                return false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/SystemAdmin/AdminQuestionnaireList.aspx");
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            List<string> msgList = new List<string>();
            if (!this.CheckInput(out msgList))
            {
                this.ltMsg.Text = string.Join("<br/>", msgList);
                return;
            }
            string account = this.Session["UserLoginInfo"] as string;
            var dr = DB.DBHelper.GetUserInfoByAccount(account);

            if (dr == null)
            {
                Response.Redirect("/QuestionnaireList.aspx");
                return;
            }
            string questionnairename = txtQuestionnaireName.Text;
            string questionnairedescribe = this.txtQuestionnaireDescribe.Text;
            string creataccount = dr["account"].ToString();
            int state;
            if (this.ckbEnable.Checked)
            {
                state = 0;
            }
            else
            {
                state = 2;
            }
            DateTime starttime = Convert.ToDateTime(this.txtStartTime.Text);
            DateTime endtime = Convert.ToDateTime(this.txtEndTime.Text);
            string qstName = this.Request.QueryString["QuestionnaireNumber"];
            var dtqstnaireQuestion = DB.DBHelper.GetQuestionnaire(qstName);

            DB.DBHelper.UpdateQuestionnaire(Convert.ToInt32(dtqstnaireQuestion["QuestionnaireNumber"].ToString()), questionnairename, questionnairedescribe, state, starttime, endtime);
            Response.Write("<script>alert('Success Update');</script>");
            Response.Redirect("AdminQuestionnaireList.aspx");
        }


    }
}