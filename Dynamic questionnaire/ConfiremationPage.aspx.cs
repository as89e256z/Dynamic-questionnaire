using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Dynamic_questionnaire
{
    public partial class ConfiremationPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (this.Request.QueryString["QuestionnaireNumber"] != null)
                {
                    string qstName = this.Request.QueryString["QuestionnaireNumber"];

                    var dtqstnaireQuestion = DB.DBHelper.GetQuestionnaire(qstName);

                    this.ltlQuestionnaireName.Text = dtqstnaireQuestion["QuestionnaireName"].ToString();

                    #region 個人資料
                    this.ltlFillerName.Text = this.Session["Name"].ToString();
                    this.ltlPhone.Text = this.Session["Phone"].ToString();
                    this.ltlEmail.Text = this.Session["Email"].ToString();
                    this.ltlAges.Text = this.Session["Ages"].ToString();
                    #endregion


                    #region Session答案取出並填入

                    CheckBox chk;
                    TextBox txb;
                    for (int i = 1; i <= _questionCount; i++)//第幾題
                    {
                        var drProblem = DB.DBHelper.GetQuestionnaireQuestionChoice(_QuestionnaireName, i);
                        string[] _Ans = new string[9];
                        int typePro = Convert.ToInt32(drProblem["TypeOfProblem"].ToString());
                        if (typePro != 2)
                        {
                            for (int j = 1; j < 10; j++)
                            {
                                _Ans[j - 1] = drProblem["Ans" + j].ToString();
                            }
                        }
                        string[] arrayAns;

                        arrayAns = _Ans.Where(s => !string.IsNullOrEmpty(s)).ToArray();//查空

                        string problemID = drProblem["ProblemID"].ToString();

                        if (arrayAns.Length == 0)
                        {
                            txb = (TextBox)this.divQuestionnaireContent.FindControl("Ans" + problemID + "-" + 1);
                            txb.Text = this.Session[drProblem["QuestionID"].ToString()].ToString();
                        }


                        System.Web.HttpContext context = System.Web.HttpContext.Current;

                        for (int j = 1; j <= arrayAns.Length; j++)//選項
                        {
                            if (context.Session[drProblem["QuestionID"].ToString() + j] != null)
                            {
                                if (arrayAns[j - 1].ToString() == this.Session[drProblem["QuestionID"].ToString() + j].ToString())
                                {
                                    chk = (CheckBox)this.divQuestionnaireContent.FindControl("Ans" + problemID + "-" + j);
                                    chk.Checked = true;
                                }
                            }
                        }
                    }
                    #endregion
                }
                else//滾回問卷一覽
                {
                    Response.Redirect("QuestionnaireList.aspx");
                }
            }

        }

        private string _QuestionnaireName;
        private int _questionCount;
        protected void Page_Init(object sender, EventArgs e)
        {
            var drQustaire = DB.DBHelper.GetQuestionnaire(this.Request.QueryString["QuestionnaireNumber"]);
            _QuestionnaireName = drQustaire["QuestionnaireName"].ToString();
            _questionCount = DB.DBHelper.GetCountQuestion(drQustaire["QuestionnaireName"].ToString());//問題數


            for (int i = 1; i <= _questionCount; i++)
            {
                var drProblem = DB.DBHelper.GetQuestionnaireQuestionChoice(_QuestionnaireName, i);
                string[] _Ans = new string[9];
                int problemID = Convert.ToInt32(drProblem["ProblemID"].ToString());
                int typePro = Convert.ToInt32(drProblem["TypeOfProblem"].ToString());
                if (typePro != 2)
                {
                    for (int j = 1; j < 10; j++)
                    {
                        _Ans[j - 1] = drProblem["Ans" + j].ToString();
                    }
                }

                #region +DIV
                HtmlGenericControl newControl = new HtmlGenericControl("div");

                newControl.ID = drProblem["QuestionID"].ToString() + "<br/>";
                newControl.InnerHtml = drProblem["ProblemTitle"].ToString() + "<br/>";
                this.divQuestionnaireContent.Controls.Add(newControl);
                #endregion

                string[] arrayAns;

                arrayAns = _Ans.Where(s => !string.IsNullOrEmpty(s)).ToArray();//查空


                switch (typePro)
                {

                    case 0:
                        rdoBANG(arrayAns, arrayAns.Length, drProblem["QuestionID"].ToString(), problemID, newControl);
                        break;
                    case 1:
                        ckBANG(arrayAns, arrayAns.Length, problemID, newControl);
                        break;
                    case 2:
                        txtBANG(problemID, newControl);
                        break;
                }

                this.ltMsg.Text = "一共" + _questionCount + "個問題";

            }

        }//動生成

        void rdoBANG(string[] ans, int maxdt, string questionID, int problemID, HtmlGenericControl _div)//單選
        {

            for (int i = 0; i < maxdt; i++)
            {
                RadioButton rdoBtn = new RadioButton();

                rdoBtn.ID = "Ans" + problemID.ToString() + "-" + (i + 1);
                rdoBtn.Text = ans[i];
                rdoBtn.GroupName = questionID;
                rdoBtn.Attributes.Add("onclick", "return false;");
                //rdoBtn.Attributes.Add( "disabled" , "Disabled ");//關修改
                _div.Controls.Add(rdoBtn);
                //this._div.Controls.Add;
            }
        }

        void ckBANG(string[] ans, int maxdt, int problemID, HtmlGenericControl _div)//複選
        {

            for (int i = 0; i < maxdt; i++)
            {
                CheckBox ckB = new CheckBox();

                ckB.ID = "Ans" + problemID.ToString() + "-" + (i + 1);
                ckB.Text = ans[i];
                ckB.Attributes.Add("onclick", "return false;");
                //ckB.Attributes.Add( "disabled" , "Disabled ");//關修改
                _div.Controls.Add(ckB);
            }
        }



        void txtBANG(int problemID, HtmlGenericControl _div)//舔空
        {
            TextBox txtB = new TextBox();

            txtB.ID = "Ans" + problemID.ToString() + "-1";
            txtB.Attributes.Add("disabled", "Disabled ");//關修改

            _div.Controls.Add(txtB);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("QuestionnaireContent.aspx" + Request.Url.Query);
        }

        protected void btnSumit_Click(object sender, EventArgs e)
        {
            #region 個人資料
            string name = this.Session["Name"].ToString();
            string phone = this.Session["Phone"].ToString();
            string email = this.Session["Email"].ToString();
            int ages = Convert.ToInt32(this.Session["Ages"].ToString());
            #endregion



            DateTime dtCreateTime = DateTime.Now;

            for (int i = 1; i <= _questionCount; i++)//第幾題
            {
                string[] _ans = new string[9];
                var drProblem = DB.DBHelper.GetQuestionnaireQuestionChoice(_QuestionnaireName, i);
                string[] _Ans = new string[9];
                int typePro = Convert.ToInt32(drProblem["TypeOfProblem"].ToString());
                if (typePro != 2)
                {
                    for (int j = 1; j < 10; j++)
                    {
                        _Ans[j - 1] = drProblem["Ans" + j].ToString();
                    }
                }
                string[] arrayAns;

                arrayAns = _Ans.Where(s => !string.IsNullOrEmpty(s)).ToArray();//查空

                string problemID = drProblem["ProblemID"].ToString();

                if (arrayAns.Length == 0)//填空
                {
                    _ans[0] = this.Session[drProblem["QuestionID"].ToString()].ToString();
                }
                else
                {
                    System.Web.HttpContext context = System.Web.HttpContext.Current;
                    int n = 0;
                    for (int j = 1; j <= arrayAns.Length; j++)//選項
                    {
                        if (context.Session[drProblem["QuestionID"].ToString() + j] != null)
                        {
                            _ans[n] = this.Session[drProblem["QuestionID"].ToString() + j].ToString();
                            n++;
                        }
                    }
                }
                for (int h = 0; h < _ans.Length; h++)
                {
                    if (string.IsNullOrWhiteSpace(_ans[h]))
                    {
                        _ans[h] = "";
                    }
                    else
                    {
                        if (typePro == 2 && h != 0)
                        {
                            _ans[h] = "";
                        }
                    }
                }
                //Response.Write(drProblem["QuestionID"].ToString()+ drProblem["QuestionnaireTitle"].ToString()
                //    + Convert.ToInt32(drProblem["ProblemID"].ToString())+ drProblem["ProblemTitle"].ToString()
                //    + typePro+ Convert.ToBoolean(drProblem["Required"].ToString())+ _ans);

                if (!DB.DBHelper.UpdateFillerChoice
                 (name, phone
                 , email, ages
                 , drProblem["QuestionnaireTitle"].ToString(), drProblem["ProblemTitle"].ToString(),
                 dtCreateTime, _ans)
                 )
                {
                    DB.DBHelper.InsertFillerChoice
                     (name, phone
                     , email, ages
                     , drProblem["QuestionnaireTitle"].ToString(), drProblem["ProblemTitle"].ToString(),
                     dtCreateTime, _ans);
                }

            }
            Session.Abandon();
            Response.Redirect("QuestionnaireList.aspx");
        }


    }
}