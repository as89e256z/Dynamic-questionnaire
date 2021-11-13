using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Dynamic_questionnaire.SystemAdmin
{
    public partial class AdminQuestionnaireQuestion : System.Web.UI.Page
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


                QuestionInput(out List<DBModels.Question> questList);//導入資料庫
                if (questList != null)
                {
                    this.gvQuestionnaireQuestionList.DataSource = questList;
                    this.gvQuestionnaireQuestionList.DataBind();
                }
                else
                {
                    this.gvQuestionnaireQuestionList.Visible = false;
                    this.plcNoData.Visible = true;
                }
            }
        }

        private void QuestionInput(out List<DBModels.Question> questList)
        {
            questList = null;
            if (Request.QueryString["QuestionnaireNumber"] != null)
            {
                DataRow drQstnaire = DB.DBHelper.GetQuestionnaire(Request.QueryString["QuestionnaireNumber"]);

                string QustireName = drQstnaire["QuestionnaireName"].ToString();
                CountQuestion(QustireName, out List<DBModels.Question> list);
                try
                {
                    if (Request.QueryString["QuestionID"] != null)
                    {
                        string qstID = Request.QueryString["QuestionID"].ToString();
                        List<DBModels.Question> qstIDsList = (List<DBModels.Question>)this.Session[qstID];
                        if (qstIDsList != null)
                        {
                            var findItems = from t in qstIDsList
                                            where t.QuestionID == (qstID)
                                            select t;
                            List<DBModels.Question> qstIDList = findItems.ToList();
                            this.txtProblemTitle.Text = qstIDList[0].ProblemTitle.ToString();
                            this.seleType.Value = qstIDList[0].TypeOfProblem.ToString();
                            this.ckbRequired.Checked = qstIDList[0].Required;
                            string txtAns = qstIDList[0].Ans1.ToString() + ";" +
                                            qstIDList[0].Ans2.ToString() + ";" +
                                            qstIDList[0].Ans3.ToString() + ";" +
                                            qstIDList[0].Ans4.ToString() + ";" +
                                            qstIDList[0].Ans5.ToString() + ";" +
                                            qstIDList[0].Ans6.ToString() + ";" +
                                            qstIDList[0].Ans7.ToString() + ";" +
                                            qstIDList[0].Ans8.ToString() + ";" +
                                            qstIDList[0].Ans9.ToString();
                            this.txtAns.Text = txtAns.TrimEnd(';');
                        }
                        else
                        {
                            var listQuestion = DB.DBHelper.GetQuestionChoice(qstID);
                            this.txtProblemTitle.Text = listQuestion[0].ProblemTitle;
                            this.seleType.Value = listQuestion[0].TypeOfProblem.ToString();
                            this.ckbRequired.Checked = listQuestion[0].Required;
                            string txtAns = listQuestion[0].Ans1.ToString() + ";" +
                                        listQuestion[0].Ans2.ToString() + ";" +
                                        listQuestion[0].Ans3.ToString() + ";" +
                                        listQuestion[0].Ans4.ToString() + ";" +
                                        listQuestion[0].Ans5.ToString() + ";" +
                                        listQuestion[0].Ans6.ToString() + ";" +
                                        listQuestion[0].Ans7.ToString() + ";" +
                                        listQuestion[0].Ans8.ToString() + ";" +
                                        listQuestion[0].Ans9.ToString();
                            this.txtAns.Text = txtAns.TrimEnd(';');
                        }
                    }
                    if ((List<DBModels.Question>)this.Session["totalList"] == null)
                    {
                        BANG(list, out List<DBModels.Question> hislist);
                        list.AddRange(hislist);
                        if (list.Count > 0)
                        {
                            questList = list;
                        }
                    }


                    else
                    {
                        List<DBModels.Question> totalList = (List<DBModels.Question>)this.Session["totalList"];
                        if (totalList.Count > 0)
                        {
                            questList = totalList;
                        }
                    }
                }
                catch (Exception ex)
                {
                    DB.Logger.WriteLog(ex);
                }
            }
            else
            {
                Response.Redirect("/SystemAdmin/AdminQuestionnaireList.aspx");
            }
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
            if (!int.TryParse(this.seleType.Value, out int typePro))
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
                model.FrequentlyAsked = (bool)dr["FrequentlyAsked"];
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
        protected void gvQuestionType(object sender, GridViewRowEventArgs e)
        {
            var questiontype = e.Row.Cells[2].Text;

            switch ((questiontype))
            {
                case "0":
                    e.Row.Cells[2].Text = "單選題";
                    break;
                case "1":
                    e.Row.Cells[2].Text = "複選題";
                    break;
                case "2":
                    e.Row.Cells[2].Text = "填空題";
                    break;
                default:
                    break;
            }
        }

        protected void btnSumit_Click(object sender, EventArgs e)
        {
            List<DBModels.Question> totalList = (List<DBModels.Question>)this.Session["totalList"];

            for (int i = 0; i < totalList.Count; i++)
            {
                DBModels.Question question = new DBModels.Question() { };

                question.QuestionID = totalList[i].QuestionID;
                question.QuestionnaireTitle = totalList[i].QuestionnaireTitle.ToString();
                question.ProblemTitle = totalList[i].ProblemTitle.ToString();
                question.ProblemID = totalList[i].ProblemID;
                question.ProblemTitle = totalList[i].ProblemTitle.ToString();
                question.TypeOfProblem = totalList[i].TypeOfProblem;
                question.Required = totalList[i].Required;
                question.FrequentlyAsked = totalList[i].FrequentlyAsked;
                question.Ans1 = totalList[i].Ans1;
                question.Ans2 = totalList[i].Ans2;
                question.Ans3 = totalList[i].Ans3;
                question.Ans4 = totalList[i].Ans4;
                question.Ans5 = totalList[i].Ans5;
                question.Ans6 = totalList[i].Ans6;
                question.Ans7 = totalList[i].Ans7;
                question.Ans8 = totalList[i].Ans8;
                question.Ans9 = totalList[i].Ans9;
                if (!DB.DBHelper.UpdateQuestionChoice(question))
                {
                    DB.DBHelper.CreateQuestionChoice(question);
                }
            }


            #region xr

            //for (int i = 0; i < this.gvQuestionnaireQuestionList.Rows.Count; i++)
            //{
            //    string strQustID = this.gvQuestionnaireQuestionList.Rows[i].Cells[0].Text;


            //}
            //DataRow drQstnaire = DB.DBHelper.GetQuestionnaire
            //    (Request.QueryString["QuestionnaireNumber"]);

            //string QuestionID = Request.QueryString["QuestionnaireNumber"] + "-" + Request.QueryString["ProblemID"];
            //string QuestionnaireName = drQstnaire["QuestionnaireName"].ToString();


            //#region //處理Session["ProblemID"] UPDATA次數
            ////char[] delimiterChars = { ';' };

            ////string[] stringpro = Request.QueryString["ProblemID"].Split(delimiterChars);
            ////int ii = 0;
            ////string[] arrayproID = new string[8];
            ////foreach (var proIDword in stringpro)
            ////{
            ////    int[] ProblemID = Convert.ToInt32(proIDword);
            ////}
            //#endregion

            ////Response.Write("QuestionID=" + QuestionID);


            //int ProblemID = Int32.Parse(this.Session[QuestionID + "ProblemID"].ToString());
            //string ProblemTitle = this.Session[QuestionID + "ProblemTitle"].ToString();
            //int TypeOfProblem = Convert.ToInt32(this.Session[QuestionID + "TypeOfProblem"].ToString());
            //bool Required = Convert.ToBoolean(this.Session[QuestionID + "Required"].ToString());
            //string[] arrayAns = new string[9];
            //for (int i = 0; i < 9; i++)
            //{
            //    if (this.Session[QuestionID + "Ans" + (i + 1)] != null &&
            //        this.Session[QuestionID + "Ans" + (i + 1)].ToString() != string.Empty)
            //    {
            //        arrayAns[i] = this.Session[QuestionID + "Ans" + (i + 1)].ToString();
            //    }
            //    else
            //    {
            //        arrayAns[i] = "";
            //    }
            //}
            //DB.DBHelper.UpdataQuestionChoice(QuestionID, QuestionnaireName, ProblemID, ProblemTitle, TypeOfProblem, Required, false,
            //    arrayAns[0], arrayAns[1], arrayAns[2], arrayAns[3], arrayAns[4], arrayAns[5], arrayAns[6], arrayAns[7], arrayAns[8]);

            //Response.Redirect(Request.RawUrl);

            ////            Response.Write("ProblemID=" + this.Session["ProblemID"] + QuestionID + QuestionnaireName+ this.Session["ProblemID"] + ProblemTitle+ TypeOfProblem+ Required+
            ////arrayAns[0]+ arrayAns[1]+ arrayAns[2]+ arrayAns[3]+ arrayAns[4]+ arrayAns[5]+ arrayAns[6]+arrayAns[7]+arrayAns[8]);
            #endregion
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

            string frequentlyasked = this.seleProAsked.Value;
            string ProblemTitle = this.txtProblemTitle.Text;
            string Type = this.seleType.Value;
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
                string strqid , strLastQuestionID;

                if (this.gvQuestionnaireQuestionList.DataSource != null)//有資料進入
                {
                    strqid = dt.Rows[_list.Count - 1]["QuestionID"].ToString();
                    //strqid = Request.QueryString["QuestionID"].ToString();
                    int Length = this.gvQuestionnaireQuestionList.Rows.Count - _list.Count;

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
                    model.FrequentlyAsked = (bool)(Convert.ToInt32(frequentlyasked) != 0 ? true : false);
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
                        this.gvQuestionnaireQuestionList.DataSource = _list;
                        this.gvQuestionnaireQuestionList.DataBind();
                    }
                    else
                    {//沒資料
                        this.gvQuestionnaireQuestionList.Visible = false;
                        this.plcNoData.Visible = true;
                    }
                }
                else//無資料進入模式
                {
                    DBModels.Question model = new DBModels.Question();
                    string[] qid;
                    string[] arrayQid;
                    int intProID = 1;

                    if (dt.Rows.Count > 0)
                    {
                        strqid = dt.Rows[_list.Count - 1]["QuestionID"].ToString();
                        qid = strqid.Split('-');
                        arrayQid = qid.Where(s => !string.IsNullOrEmpty(s)).ToArray();//去空,必為0,1
                        intProID = Convert.ToInt32(arrayQid[1])+1;
                    }
                    else//0筆全新資料
                    {
                        strLastQuestionID = DB.DBHelper.GetLastQuestionID();

                        qid = strLastQuestionID.Split('-');
                        arrayQid = qid.Where(s => !string.IsNullOrEmpty(s)).ToArray();//去空,必為0,1
                        intProID = 1;

                        this.plcNoData.Visible = false;
                        this.gvQuestionnaireQuestionList.Visible = true;
                    }

                    int intQuestionID = Convert.ToInt32(arrayQid[0]) + 1;//增號後題號
                    string strQuestID = intQuestionID + "-" + intProID;

                    model.QuestionID = strQuestID;

                    model.QuestionnaireTitle = (string)questionnairename;
                    model.ProblemID = (int)intProID;
                    model.ProblemTitle = (string)ProblemTitle;
                    model.TypeOfProblem = (int)Convert.ToInt32(Type);
                    model.Required = (bool)Required;
                    model.FrequentlyAsked = (bool)(Convert.ToInt32(frequentlyasked) != 0 ? true : false);
                    model.Ans1 = _arrayAns[0].ToString();
                    model.Ans2 = _arrayAns[1].ToString();
                    model.Ans3 = _arrayAns[2].ToString();
                    model.Ans4 = _arrayAns[3].ToString();
                    model.Ans5 = _arrayAns[4].ToString();
                    model.Ans6 = _arrayAns[5].ToString();
                    model.Ans7 = _arrayAns[6].ToString();
                    model.Ans8 = _arrayAns[7].ToString();
                    model.Ans9 = _arrayAns[8].ToString();
                    Session.Add(intQuestionID + "-" + intProID, model); // put into Session

                    hisList.Add(model);

                    //_list.AddRange(hisList);
                    if (hisList.Count > 0)
                    {
                        Session.Add("totalList", hisList);
                        this.gvQuestionnaireQuestionList.DataSource = hisList;
                        this.gvQuestionnaireQuestionList.DataBind();
                    }
                    else
                    {//沒資料
                        this.gvQuestionnaireQuestionList.Visible = false;
                        this.plcNoData.Visible = true;
                    }

                }
            }
        }
        void BANG(List<DBModels.Question> list, out List<DBModels.Question> hisList)
        {
            hisList = new List<DBModels.Question>();
            int lengthInputList = 0;
            if (this.gvQuestionnaireQuestionList.Rows.Count > list.Count)
            {
                int OriginLength = list.Count;
                lengthInputList = this.gvQuestionnaireQuestionList.Rows.Count - OriginLength;
                for (int i = 0; i < lengthInputList; i++)
                {
                    string questionid = this.gvQuestionnaireQuestionList.Rows[OriginLength + i].Cells[0].Text;

                    if (this.Session[questionid] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(this.Session[questionid].ToString()))
                        {
                            List<DBModels.Question> questions;
                            //Response.Write(this.Session[questionid].ToString());
                            //Response.Write(this.Session[questionid].GetType());

                            questions = this.Session[questionid] as List<DBModels.Question>;
                            hisList.Add(questions[i]);
                        }
                    }
                }

            }
        }
    }
}