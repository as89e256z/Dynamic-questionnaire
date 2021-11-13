using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Dynamic_questionnaire
{
    public partial class QuestionnaireContent : System.Web.UI.Page
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

                    if (dtqstnaireQuestion["QuestionnaireDescribe"].ToString() != null)
                    {
                        this.ltlQuestionnaireDescribe.Text = dtqstnaireQuestion["QuestionnaireDescribe"].ToString();
                        if (this.Session["Name"] != null && this.Session["Phone"] != null 
                            && this.Session["Email"] != null && this.Session["Ages"] != null)
                        {
                        #region 個人資料
                        this.txtFillerName.Text = this.Session["Name"].ToString();
                        this.txtPhone.Text = this.Session["Phone"].ToString();
                        this.txtEmail.Text = this.Session["Email"].ToString();
                        this.txtAges.Text = this.Session["Ages"].ToString();
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

                    }


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

        }

        public static void Production()
        {

        }


        void rdoBANG(string[] ans, int maxdt, string questionID, int problemID, HtmlGenericControl _div)//單選
        {

            for (int i = 0; i < maxdt; i++)
            {
                RadioButton rdoBtn = new RadioButton();

                rdoBtn.ID = "Ans" + problemID.ToString() + "-" + (i + 1);
                rdoBtn.Text = ans[i];
                rdoBtn.GroupName = questionID;
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

                _div.Controls.Add(ckB);
            }
        }
        void txtBANG(int problemID, HtmlGenericControl _div)//舔空
        {
            TextBox txtB = new TextBox();

            txtB.ID = "Ans" + problemID.ToString() + "-1";

            _div.Controls.Add(txtB);
        }

        #region btnTest
        void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;
            Response.Write(btn.Text);
        }

        #endregion
        void btnBANG()
        {
            for (int i = 1; i <= 10; i++)
            {
                Button btn = new Button();
                string btnDescript = string.Format("btn{0}", i.ToString("00"));

                btn.ID = btnDescript;
                btn.Text = btnDescript;
                btn.Click += btn_Click;
                Page.Form.Controls.Add(btn);
            }
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("QuestionnaireList.aspx");
        }

        protected void btnSumit_Click(object sender, EventArgs e)
        {
            #region 個人資料
            this.Session["Name"] = this.txtFillerName.Text;
            this.Session["Phone"] = this.txtPhone.Text;
            this.Session["Email"] = this.txtEmail.Text;
            this.Session["Ages"] = this.txtAges.Text;
            #endregion

            #region 問卷內容處理
            CheckBox chk;
            TextBox txb;
            for (int i = 1; i <= _questionCount; i++)//題墓
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

                arrayAns = _Ans.Where(s => !string.IsNullOrEmpty(s)).ToArray();//除該問題選項空值

                if (arrayAns.Length == 0)//填空題時
                {
                    txb = (TextBox)this.divQuestionnaireContent.FindControl("Ans" + drProblem["ProblemID"].ToString() + "-" + 1);
                    this.Session[drProblem["QuestionID"].ToString()] = txb.Text;
                }

                for (int j = 1; j <= arrayAns.Length; j++)//選項
                {
                    string problemID = drProblem["ProblemID"].ToString();
                    chk = (CheckBox)this.divQuestionnaireContent.FindControl("Ans" + problemID + "-" + j);
                    if (chk.Checked == true)
                    {
                        this.Session[drProblem["QuestionID"].ToString() + j] = arrayAns[j - 1];
                    }
                }
            }
            #endregion

            Response.Redirect("ConfiremationPage.aspx" + Request.Url.Query);
        }
    }
}