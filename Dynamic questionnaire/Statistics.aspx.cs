using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


namespace Dynamic_questionnaire
{
    public partial class Statistics : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                string qstTitle = Request.QueryString["QuestionnaireName"].ToString();
                var dtFiller = DB.DBHelper.GetFillersAnsTable(qstTitle);
                if (dtFiller != null && dtFiller.Rows.Count > 0)
                {
                    int[][] _statisticsAns;
                    Countstatistics(out _statisticsAns);

                    //Response.Write("statisticsAns[0][2]" + (_statisticsAns[0][0]));
                    #region 查各選項出現次數
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
                            if ((i+1) % _questionCount != 0)//IMPORTANT
                            {
                                i++;
                            }
                        }
                    }
                    #endregion


                    int[] countAns;
                    CountQuestionAns(_statisticsAns, out countAns);

                    #region 將次數And統計塞到該選項後面
                    CultureInfo ci = new CultureInfo("en-us");
                    for (int c = 1; c <= _questionCount; c++)
                    {
                        var drProblem = DB.DBHelper.GetQuestionnaireQuestionChoice(qstTitle, c);

                        int AnsLength/*, FillerAnsLength*/;
                        if (Convert.ToInt32(this.Session[drProblem["QuestionID"].ToString() + "AnsLength"].ToString()) != 0)
                        {
                            AnsLength = Convert.ToInt32(this.Session[drProblem["QuestionID"].ToString() + "AnsLength"].ToString());

                            for (int f = 1; f <= AnsLength; f++)//選項
                            {
                                var newDiv = (HtmlGenericControl)this.divChoiceProgressContent.FindControl
                                    (drProblem["QuestionID"].ToString());
                                var divChoice = (HtmlGenericControl)newDiv.FindControl
                                    ("ChoiceAns" + drProblem["ProblemID"].ToString() + "-" + (f));

                                double doubleDividend = Convert.ToDouble(_statisticsAns[c - 1][f - 1]);
                                double doubleDivisor = Convert.ToDouble(countAns[c - 1]);

                                var strPercentage =
                                    (doubleDividend / doubleDivisor).ToString("P00000", ci);

                                divChoiceProgress(drProblem["QuestionID"].ToString(), f, strPercentage, divChoice);
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    this.plcNoData.Visible = true;
                    this.divChoiceProgressContent.Visible = false;
                }
            }
        }

        int _questionCount;

        protected void Page_Init(object sender, EventArgs e)
        {
            #region 產問題
            string qstTitle = Request.QueryString["QuestionnaireName"].ToString();

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
                    #region +DIV
                    HtmlGenericControl divProblemID = new HtmlGenericControl("div");

                    divProblemID.ID = drProblem["QuestionID"].ToString();
                    divProblemID.InnerHtml = drProblem["ProblemTitle"].ToString() + "<br/>";
                    this.divChoiceProgressContent.Controls.Add(divProblemID);
                    #endregion
                }
                string[] arrayAns;

                arrayAns = _Ans.Where(s => !string.IsNullOrEmpty(s)).ToArray();//查空


                switch (typePro)
                {
                    case 0:
                    case 1:
                        for (int n = 0; n < arrayAns.Length; n++)
                        {
                            divChoice(arrayAns[n], n + 1, problemID);
                        }
                        break;
                    case 2:
                        break;
                }
            }
            #endregion

        }//動生成
        /// <summary>
        /// 統計容量龜殼設計
        /// </summary>
        /// <param name="statisticsAns"></param>
        private void Countstatistics(out int[][] statisticsAns)
        {
            string qstTitle = Request.QueryString["QuestionnaireName"].ToString();

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

        private void CountQuestionAns(int[][] _questionAns, out int[] countAns)
        {
            string qstTitle = Request.QueryString["QuestionnaireName"].ToString();
            int AnsLength;
            countAns = new int[_questionCount];
            for (int i = 0; i < _questionCount; i++)
            {
                var drProblem = DB.DBHelper.GetQuestionnaireQuestionChoice(qstTitle, i + 1);
                this.Session[drProblem["QuestionID"].ToString() + "AnsLength"].ToString();
                AnsLength = Convert.ToInt32(this.Session[drProblem["QuestionID"].ToString() + "AnsLength"].ToString());
                for (int j = 0; j < AnsLength; j++)//選項
                {
                    countAns[i] += _questionAns[i][j];
                }
            }
        }

        void divChoice(string problemtitle, int ID, int problemID)
        {
            HtmlGenericControl divChoice = new HtmlGenericControl("div");
            HtmlGenericControl divproblemtitle = new HtmlGenericControl("div");

            divChoice.ID = "ChoiceAns" + problemID.ToString() + "-" + ID;
            divChoice.Attributes.Add("class", "progress");

            divproblemtitle.InnerText = problemtitle;
            //this.divChoiceProgressContent.InnerText = problemtitle+ "<br/>";
            this.divChoiceProgressContent.Controls.Add(divproblemtitle);
            this.divChoiceProgressContent.Controls.Add(divChoice);
        }

        void divChoiceProgress(string ID, int nbrChoice, string Percentage, HtmlGenericControl divChoice)
        {
            HtmlGenericControl divProgress = new HtmlGenericControl("div");

            divProgress.ID = "Ans" + ID + nbrChoice;
            divProgress.Attributes.Add("class", "progress-bar");
            divProgress.Attributes.Add("role", "progressbar");
            divProgress.Attributes.Add("style", $"width: {Percentage};");
            divProgress.Attributes.Add("aria-valuenow", $"{Percentage}");
            divProgress.Attributes.Add("aria-valuemin", "0");
            divProgress.Attributes.Add("aria-valuemax", "100");
            divProgress.InnerText = Percentage;
            divChoice.Controls.Add(divProgress);
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("QuestionnaireList.aspx");
        }

    }
}