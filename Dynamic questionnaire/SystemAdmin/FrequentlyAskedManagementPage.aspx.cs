using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dynamic_questionnaire.SystemAdmin
{
    public partial class FrequentlyAskedManagementPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)//非第一次載入時
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
                QuestionInput();//導入資料庫
            }
        }

        private void QuestionInput()
        {
            DataRow drQstnaire = DB.DBHelper.GetQuestionnaire(Request.QueryString["QuestionnaireNumber"]);

            string QustireName = drQstnaire["QuestionnaireName"].ToString();
            CountQuestion(QustireName, out List<DBModels.Question> list);
            try
            {
                if ((List<DBModels.Question>)this.Session["totalList"] == null)
                {
                    BANG(list, out List<DBModels.Question> hislist);
                    list.AddRange(hislist);
                    if (list.Count > 0)
                    {
                        this.gvQuestionUsedList.DataSource = list;
                        this.gvQuestionUsedList.DataBind();
                    }
                    else
                    {//沒資料
                        this.gvQuestionUsedList.Visible = false;
                        this.plcNoData.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                DB.Logger.WriteLog(ex);
                throw;
            }
            Response.Redirect("/SystemAdmin/AdminQuestionnaireList.aspx");
        }


        #region Check
        private bool CheckInput(out List<string> errorMsgList)
        {
            List<string> msgList = new List<string>();

            //檢查ProblemTitle
            if (string.IsNullOrWhiteSpace(this.txtProblemTitle.Text))
            {
                msgList.Add("ProblemTitle is Required.");
            }
            else
            {
                if (this.txtProblemTitle.Text.Length > 100)
                {
                    msgList.Add("ProblemTitle can't over 100 characters.");
                }
            }

            //檢查TypeOfProblem
            if (!int.TryParse(this.ddlTypeOfProblem.SelectedValue, out int typePro))
            {
                msgList.Add("TypeOfProblem must a number.");
            }
            else
            {
                if (!(typePro > -1 && typePro < 3))
                {
                    msgList.Add("type can't over 100 characters.");
                }
            }

            //檢查Ans
            char[] delimiterChars = { ';', '；' };
            char[] unwanteddelimiterChars = { '-', '~', ',', '.', ' ' };
            //string[] unwanteddelimiterChars = {"-","~",",","." ," "};
            var _ckAns = this.txtAns.Text.Split(delimiterChars);
            if (string.IsNullOrWhiteSpace(this.txtAns.Text))
            {
                if (typePro != 2)//非填空題
                {
                    msgList.Add("Ans is Required.");
                }
                else {/*填空題可以不用答案*/}
            }
            else
            {
                if (_ckAns.Length < 2)
                {
                    msgList.Add("Type is Multiple Question,Please have more Answers.");
                }
                else
                {
                    if (this.txtAns.Text.Split(unwanteddelimiterChars).Length > 1)
                    {
                        msgList.Add("Please use ; as delimiterchars");
                    }
                }
            }

            //檢查Required            
            errorMsgList = msgList;
            if (msgList.Count == 0)
                return true;
            else
                return false;
        }
        #endregion
        protected void gvQuestionType(object sender, GridViewRowEventArgs e)
        {
            var questiontype = e.Row.Cells[1].Text;

            switch ((questiontype))
            {
                case "0":
                    e.Row.Cells[1].Text = "單選題";
                    break;
                case "1":
                    e.Row.Cells[1].Text = "複選題";
                    break;
                case "2":
                    e.Row.Cells[1].Text = "填空題";
                    break;
                default:
                    break;
            }
        }


        private void CountQuestion(string qustirename, out List<DBModels.Question> list)
        {
            list = new List<DBModels.Question>();
            var dt = DB.DBHelper.GetQuestionnaireQuestionList(qustirename);//查問題

            foreach (DataRow dr in dt.Rows)
            {
                DBModels.Question model = new DBModels.Question();
                model.QuestionID = (string)dr["QuestionID"].ToString();
                model.QuestionnaireTitle = (string)dr["QuestionnaireTitle"].ToString();
                model.ProblemID = (int)dr["ProblemID"];
                model.ProblemTitle = (string)dr["ProblemTitle"].ToString();
                model.TypeOfProblem = (int)dr["TypeOfProblem"];
                model.Required = (bool)dr["Required"];
                //model.FrequentlyAsked = (bool)dr["FrequentlyAsked"];
                model.Ans1 = (string)dr["Ans1"].ToString();
                model.Ans2 = (string)dr["Ans2"].ToString();
                model.Ans3 = (string)dr["Ans3"].ToString();
                model.Ans4 = (string)dr["Ans4"].ToString();
                model.Ans5 = (string)dr["Ans5"].ToString();
                model.Ans6 = (string)dr["Ans6"].ToString();
                model.Ans7 = (string)dr["Ans7"].ToString();
                model.Ans8 = (string)dr["Ans8"].ToString();
                model.Ans9 = (string)dr["Ans9"].ToString();
                list.Add(model);
                Session.Add(model.QuestionID, list); // put into Session
            }
        }

        /// <summary>
        /// 回答INT轉中文顯示
        /// </summary>
        /// <param name="e"></param>
        /// 可換<%$ ((int)Eval("TypeOfProblem") == 0) ? "題" : "題"%>

        protected void btnSumit_Click(object sender, EventArgs e)
        {
            List<string> msgList = new List<string>();
            if (!this.CheckInput(out msgList))
            {
                this.ltMsg.Text = string.Join("<br/>", msgList);
                return;
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Session.Contents.Remove("totalList");
            Response.Redirect("/SystemAdmin/AdminQuestionnaireList.aspx");
        }

        protected void btnInput_Click(object sender, EventArgs e)
        {
            List<string> msgList = new List<string>();
            if (!this.CheckInput(out msgList))
            {
                this.ltMsg.Text = string.Join("<br/>", msgList);
                return;
            }

            //string frequentlyasked = this.seleProAsked.Value;
            string ProblemTitle = this.txtProblemTitle.Text;
            string Type = this.ddlTypeOfProblem.SelectedValue;
            bool Required = this.ckbRequired.Checked;
            string Ans = this.txtAns.Text;




            string _Ans = this.txtAns.Text;
            string[] strarrayAns = _Ans.Split(';');
            string[] arrayAns = new string[9];
            arrayAns = strarrayAns.Where(s => !string.IsNullOrEmpty(s)).ToArray();//去空

            string[] _arrayAns = new string[9];
            for (int i = 0; i < 9; i++)
            {
                if (i < arrayAns.Length)
                {
                    _arrayAns[i] = arrayAns[i];
                }
                else
                {
                    _arrayAns[i] = "";
                }
            }
            if (Request.QueryString["QuestionnaireNumber"] != null)
            {
                DataRow drQstnaire = DB.DBHelper.GetQuestionnaire(Request.QueryString["QuestionnaireNumber"]);

                string questionnairename = drQstnaire["QuestionnaireName"].ToString();
                CountQuestion(questionnairename, out List<DBModels.Question> _list);
                BANG(_list, out List<DBModels.Question> hisList);

                int nowLength = _list.Count + hisList.Count;
                var dt = DB.DBHelper.GetQuestionnaireQuestionList(questionnairename);//查問題
                string strqid;

                strqid = dt.Rows[_list.Count - 1]["QuestionID"].ToString();
                //strqid = Request.QueryString["QuestionID"].ToString();
                int Length = this.gvQuestionUsedList.Rows.Count - _list.Count;

                string[] qid = strqid.Split('-');
                string[] arrayQid;
                arrayQid = qid.Where(s => !string.IsNullOrEmpty(s)).ToArray();//去空,必為0,1
                int intProID = Convert.ToInt32(arrayQid[1]) + 1 + Length;


                List<DBModels.Question> newlist = new List<DBModels.Question>();

                DBModels.Question model = new DBModels.Question();
                if (Request.QueryString["QuestionID"] != null)
                {
                    model.QuestionID = Request.QueryString["QuestionID"].ToString();
                    strqid = Request.QueryString["QuestionID"].ToString();
                    qid = strqid.Split('-');

                    arrayQid = qid.Where(s => !string.IsNullOrEmpty(s)).ToArray();//去空,必為0,1
                    intProID = Convert.ToInt32(arrayQid[1]);
                }
                else
                {
                    model.QuestionID = (string)arrayQid[0] + "-" + intProID;
                }

                model.QuestionnaireTitle = (string)questionnairename;
                model.ProblemID = (int)intProID;
                model.ProblemTitle = (string)ProblemTitle;
                model.TypeOfProblem = (int)Convert.ToInt32(Type);
                model.Required = (bool)Required;
                //model.FrequentlyAsked = (bool)(Convert.ToInt32(frequentlyasked) != 0 ? true : false);
                model.Ans1 = _arrayAns[0].ToString();
                model.Ans2 = _arrayAns[1].ToString();
                model.Ans3 = _arrayAns[2].ToString();
                model.Ans4 = _arrayAns[3].ToString();
                model.Ans5 = _arrayAns[4].ToString();
                model.Ans6 = _arrayAns[5].ToString();
                model.Ans7 = _arrayAns[6].ToString();
                model.Ans8 = _arrayAns[7].ToString();
                model.Ans9 = _arrayAns[8].ToString();
                Session.Add(arrayQid[0] + "-" + intProID, model); // put into Session

                hisList.Add(model);

                _list.AddRange(hisList);

                if (_list.Count > 0)
                {
                    Session.Add("totalList", _list);
                    this.gvQuestionUsedList.DataSource = _list;
                    this.gvQuestionUsedList.DataBind();
                }
                else
                {//沒資料
                    this.gvQuestionUsedList.Visible = false;
                    this.plcNoData.Visible = true;
                }
            }
        }
        void BANG(List<DBModels.Question> list, out List<DBModels.Question> hisList)
        {
            hisList = new List<DBModels.Question>();
            int lengthInputList = 0;
            if (this.gvQuestionUsedList.Rows.Count > list.Count)
            {
                int OriginLength = list.Count;
                lengthInputList = this.gvQuestionUsedList.Rows.Count - OriginLength;
                for (int i = 0; i < lengthInputList; i++)
                {
                    string questionid = this.gvQuestionUsedList.Rows[OriginLength + i].Cells[0].Text;

                    if (this.Session[questionid] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(this.Session[questionid].ToString()))
                        {
                            List<DBModels.Question> questions;
                            questions = this.Session[questionid] as List<DBModels.Question>;
                            hisList.Add(questions[i]);
                        }
                    }
                }

            }
        }
    }
}