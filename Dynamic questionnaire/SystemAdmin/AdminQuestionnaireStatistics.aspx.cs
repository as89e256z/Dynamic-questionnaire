using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Dynamic_questionnaire.Admin
{
    public partial class AdminQuestionnaireStatistics : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string qstTitle = Request.QueryString["QuestionnaireTitle"].ToString();
                var dtFiller = DB.DBHelper.GetFillersAnsTable(qstTitle);
                int[][] _statisticsAns;
                Countstatistics(out _statisticsAns);

                //Response.Write("statisticsAns[0][2]" + (_statisticsAns[0][0]));
                int i;
                for (i = 0; i < dtFiller.Rows.Count; i++)//總筆
                {
                    for (int j = 1; j <= _questionCount; j++)//題目
                    {
                        var drProblem = DB.DBHelper.GetQuestionnaireQuestionChoice(qstTitle, j);
                        int typePro = Convert.ToInt32(drProblem["TypeOfProblem"].ToString());
                        if (typePro != 2)
                        {
                            string[] _Ans = new string[9];
                            for (int t = 1; t < 10; t++)//計算目前資料庫答案
                            {
                                _Ans[t - 1] = drProblem["Ans" + t].ToString();
                            }
                            string[] arrayAns;
                            arrayAns = _Ans.Where(s => !string.IsNullOrEmpty(s)).ToArray();//去空 為題目選項答案 填空預設為0


                            var rowI = dtFiller.Rows[i];
                            string[] _FillerAns = new string[9];
                            for (int h = 1; h < 10; h++)
                            {
                                _FillerAns[h - 1] = rowI["Ans" + h].ToString();
                            }
                            string[] arrayFillerAns;
                            arrayFillerAns = _FillerAns.Where(s => !string.IsNullOrEmpty(s)).ToArray();//去空 為Filler答案



                            //arrayAns[0].ToString();
                            //arrayFillerAns[0].ToString();
                            #region 產Span

                            #endregion

                            for (int n = 0; n < arrayFillerAns.Length; n++)
                            {
                                for (int f = 0; f < arrayAns.Length; f++)//選項
                                {
                                    if (arrayAns[f].ToString() == arrayFillerAns[n].ToString())
                                    {
                                        _statisticsAns[j - 1][f] += 1;
                                    }
                                }
                            }

                            if (i <= _questionCount)//保留問題及回答
                            {
                                this.Session[drProblem["QuestionID"].ToString() + "AnsLength"] = arrayAns.Length;
                                this.Session[drProblem["QuestionID"].ToString() + "FillerAnsLength"] = arrayFillerAns.Length;
                            }
                        }
                        else
                        {
                            this.Session[drProblem["QuestionID"].ToString() + "AnsLength"] = 0;
                        }
                        if ((i + 1) % _questionCount != 0)//IMPORTANT
                        {
                            i++;
                        }
                    }
                }//查各選項出現次數

                int[] countAns;
                CountQuestionAns(_statisticsAns,out countAns);

                CultureInfo ci = new CultureInfo("en-us");
                for (int c = 1; c <= _questionCount; c++)
                {
                    var drProblem = DB.DBHelper.GetQuestionnaireQuestionChoice(qstTitle, c);

                    int AnsLength/*, FillerAnsLength*/;
                    if (Convert.ToInt32(this.Session[drProblem["QuestionID"].ToString() + "AnsLength"].ToString()) != 0)
                    {
                        AnsLength = Convert.ToInt32(this.Session[drProblem["QuestionID"].ToString() + "AnsLength"].ToString());

                        //FillerAnsLength = Convert.ToInt32(this.Session[drProblem["Problem"].ToString() + "FillerAnsLength"].ToString());


                        for (int f = 1; f <= AnsLength; f++)//選項
                        {

                            var newDiv = (HtmlGenericControl)this.divQuestionnaireContent.FindControl
                                (drProblem["QuestionID"].ToString());
                            var divChoice = (HtmlGenericControl)newDiv.FindControl
                                ("ChoiceAns" + drProblem["ProblemID"].ToString() + "-" + (f));
                            //chk = (CheckBox)newDiv.FindControl("Ans" + drProblem["ProblemID"].ToString() + "-" + (f));
                            HtmlGenericControl spanCount = new HtmlGenericControl("span");
                            double doubleDividend = Convert.ToDouble(_statisticsAns[c - 1][f - 1]);
                            double doubleDivisor = Convert.ToDouble(countAns[c - 1]);
                             /// countAns[c - 1]
                            spanCount.ID = drProblem["QuestionID"].ToString() + f;
                            spanCount.InnerHtml = 
                                (doubleDividend/ doubleDivisor).ToString("P00000", ci)
                                + "（" +_statisticsAns[c - 1][f - 1] + "）";
                            
                            divChoice.Controls.Add(spanCount);
                        }
                    }

                }//將次數塞到該選項後面


            }
        }

        int _questionCount;

        protected void Page_Init(object sender, EventArgs e)
        {
            string qstTitle = Request.QueryString["QuestionnaireTitle"].ToString();

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
                HtmlGenericControl divProblemID = new HtmlGenericControl("div");

                divProblemID.ID = drProblem["QuestionID"].ToString();
                divProblemID.InnerHtml = drProblem["ProblemTitle"].ToString() + "<br/>";
                this.divQuestionnaireContent.Controls.Add(divProblemID);
                #endregion

                string[] arrayAns;

                arrayAns = _Ans.Where(s => !string.IsNullOrEmpty(s)).ToArray();//查空


                switch (typePro)
                {

                    case 0:
                        rdoBANG(arrayAns, arrayAns.Length, drProblem["QuestionID"].ToString(), problemID, divProblemID);
                        break;
                    case 1:
                        ckBANG(arrayAns, arrayAns.Length, problemID, divProblemID);
                        break;
                    case 2:
                        txtBANG(problemID, divProblemID);
                        break;
                }
            }

        }//動生成
        private void Countstatistics(out int[][] statisticsAns)
        {
            string qstTitle = Request.QueryString["QuestionnaireTitle"].ToString();

            #region 設計統計容量

            statisticsAns = new int[_questionCount][];
            for (int p = 0; p < _questionCount; p++)
            {
                var drProblem = DB.DBHelper.GetQuestionnaireQuestionChoice(qstTitle, p + 1);
                int typePro = Convert.ToInt32(drProblem["TypeOfProblem"].ToString());
                string[] _Ans = new string[9];
                if (typePro != 2)
                {
                    for (int t = 1; t < 10; t++)
                    {
                        _Ans[t - 1] = drProblem["Ans" + t].ToString();
                    }
                }
                string[] arrayAns;
                arrayAns = _Ans.Where(s => !string.IsNullOrEmpty(s)).ToArray();//去空 為題目選項答案 填空預設為0

                statisticsAns[p] = new int[arrayAns.Length];
            }
            #endregion
        }
        private void CountQuestionAns(int[][] _questionAns ,out int[] countAns)
        {
            string qstTitle = Request.QueryString["QuestionnaireTitle"].ToString();
            int AnsLength;
            countAns = new int[_questionCount];
            for (int i = 0; i < _questionCount; i++)
            {
                var drProblem = DB.DBHelper.GetQuestionnaireQuestionChoice(qstTitle, i+1);
                this.Session[drProblem["QuestionID"].ToString() + "AnsLength"].ToString();
                AnsLength = Convert.ToInt32(this.Session[drProblem["QuestionID"].ToString() + "AnsLength"].ToString());
                for (int j = 0; j < AnsLength; j++)//選項
                {
                    countAns[i] += _questionAns[i][j];
                }
            }
        }
        void divBANG(string ID, out HtmlGenericControl divAns)
        {
            divAns = new HtmlGenericControl("div");

            divAns.ID = "Choice" + ID;
            divAns.InnerHtml = "<br/>";
            this.divQuestionnaireContent.Controls.Add(divAns);
        }
        void rdoBANG(string[] ans, int maxdt, string questionID, int problemID, HtmlGenericControl _div)//單選
        {

            for (int i = 0; i < maxdt; i++)
            {
                RadioButton rdoBtn = new RadioButton();

                rdoBtn.ID = "Ans" + problemID.ToString() + "-" + (i + 1);
                HtmlGenericControl divAns;
                divBANG(rdoBtn.ID, out divAns);
                rdoBtn.Text = ans[i];
                rdoBtn.GroupName = questionID;
                rdoBtn.Attributes.Add("onclick", "return false;");

                //HtmlGenericControl newControl = new HtmlGenericControl("div");

                //newControl.ID = "Ans" + problemID.ToString() + "-" + (i + 1);
                //this.divQuestionnaireContent.Controls.Add(newControl);

                //rdoBtn.Attributes.Add( "disabled" , "Disabled ");//關修改
                _div.Controls.Add(divAns);
                divAns.Controls.Add(rdoBtn);
                //this._div.Controls.Add;
            }
        }
        void ckBANG(string[] ans, int maxdt, int problemID, HtmlGenericControl _div)//複選
        {

            for (int i = 0; i < maxdt; i++)
            {
                CheckBox ckB = new CheckBox();

                ckB.ID = "Ans" + problemID.ToString() + "-" + (i + 1);
                HtmlGenericControl divAns;
                divBANG(ckB.ID, out divAns);
                ckB.Text = ans[i];
                ckB.Attributes.Add("onclick", "return false;");

                //ckB.Attributes.Add( "disabled" , "Disabled ");//關修改
                _div.Controls.Add(divAns);
                divAns.Controls.Add(ckB);
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
            Response.Redirect("AdminQuestionnaireList.aspx");
        }
    }
}