using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Dynamic_questionnaire.Admin
{
    public partial class FillerAns : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                #region 登入否?
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
                #endregion

                if (this.Request.QueryString["Name"] != null)
                {
                    System.Web.HttpContext context = System.Web.HttpContext.Current;
                    try
                    {
                        string fillername = Request.QueryString["Name"].ToString();
                        string qstTitle = this.Session["QuestionnaireTitle"].ToString();
                        var dtFiller = DB.DBHelper.GetFillerTable(fillername, qstTitle);
                        var list0 = dtFiller.Rows[0];

                        this.ltlFillerName.Text = list0["Name"].ToString();
                        this.ltlPhone.Text = list0["Phone"].ToString();
                        this.ltlEmail.Text = list0["Email"].ToString();
                        this.ltlAges.Text = list0["Ages"].ToString();
                        this.ltlCreateTime.Text += list0["CreateTime"].ToString();
                        #region 答案取出並填入


                        CheckBox chk;
                        TextBox txb;
                        for (int i = 1; i <= _questionCount; i++)//第幾題
                        {
                            var rowI = dtFiller.Rows[i-1];


                            var drProblem = DB.DBHelper.GetQuestionnaireQuestionChoice(this.Session["QuestionnaireTitle"].ToString(), i);
                            string[] _Ans = new string[9];
                            int typePro = Convert.ToInt32(drProblem["TypeOfProblem"].ToString());
                            if (typePro != 2)
                            {
                                for (int j = 1; j < 10; j++)
                                {
                                    _Ans[j - 1] = drProblem["Ans" + j].ToString();
                                }
                            }
                            string[] _FillerAns = new string[9];

                            for (int h = 1; h < 10; h++)
                            {
                                _FillerAns[h-1] = rowI["Ans" + h].ToString();
                            }

                            string[] arrayAns;
                            string[] arrayFillerAns;

                            arrayAns = _Ans.Where(s => !string.IsNullOrEmpty(s)).ToArray();//去空
                            arrayFillerAns = _FillerAns.Where(s => !string.IsNullOrEmpty(s)).ToArray();//去空

                            string problemID = drProblem["ProblemID"].ToString();

                            if (arrayAns.Length == 0 && arrayFillerAns.Length == 1)
                            {
                                txb = (TextBox)this.divQuestionnaireContent.FindControl("Ans" + problemID + "-" + 1);
                                txb.Text = rowI["Ans" + 1].ToString();
                            }


                            for (int n = 0; n < arrayFillerAns.Length; n++)
                            {
                                for (int j = 1; j <= arrayAns.Length; j++)//選項
                                {
                                    if (arrayAns[j - 1].ToString() == arrayFillerAns[n].ToString())
                                    {
                                        chk = (CheckBox)this.divQuestionnaireContent.FindControl("Ans" + problemID + "-" + j);
                                        chk.Checked = true;
                                    }

                                }
                            }
                        }
                        #endregion
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else
                {
                    Response.Redirect("AdminQuestionnaireList.aspx");
                }
            }
        }

        int _questionCount;
        protected void Page_Init(object sender, EventArgs e)
        {
            string qstTitle = this.Session["QuestionnaireTitle"].ToString();

            _questionCount = DB.DBHelper.GetCountQuestion(qstTitle);//問題數


            for (int i = 1; i <= _questionCount; i++)
            {
                var drProblem = DB.DBHelper.GetQuestionnaireQuestionChoice(qstTitle, i);
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

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Write("<script language=javascript>history.go(-2);</script>)");
        }
    }
}