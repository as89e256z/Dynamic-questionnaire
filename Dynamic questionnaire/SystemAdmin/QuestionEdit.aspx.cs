using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dynamic_questionnaire.SystemAdmin
{
    public partial class QuestionEdit : System.Web.UI.Page
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!this.IsPostBack)
        //    {
        //        //Question沒放Number，但需要查QuestionnaireName
        //        string qstNB = this.Request.QueryString["QuestionnaireNumber"];
        //        var dtqstnaireQuestion = DB.DBHelper.GetQuestionnaire(qstNB);
        //        var dt = DB.DBHelper.GetQuestionnaireQuestionChoice(dtqstnaireQuestion["QuestionnaireName"].ToString(),
        //            Convert.ToInt32(Request.QueryString["ProblemID"]));
        //        this.Session["QuestionnaireName"] = dtqstnaireQuestion["QuestionnaireName"].ToString();

        //        this.txtProblemTitle.Text = dt["ProblemTitle"].ToString();

        //        this.ddlTypeOfProblem.SelectedValue = dt["TypeOfProblem"].ToString();
        //        for (int i = 1; i < 9; i++)
        //        {
        //            if (dt["Ans" + i].ToString() != null || dt["Ans" + i].ToString() != string.Empty)
        //            {
        //                if (i > 1)
        //                {
        //                    this.txtAns.Text = this.txtAns.Text + ";" + dt["Ans" + i].ToString();
        //                }
        //                else
        //                {
        //                    this.txtAns.Text = dt["Ans" + i].ToString();
        //                }
        //            }
        //        }
        //        this.ckbRequired.Checked = Convert.ToBoolean(dt["Required"].ToString());
        //    }
        //}

        //#region Check
        //private bool CheckInput(out List<string> errorMsgList)
        //{
        //    List<string> msgList = new List<string>();

        //    //檢查ProblemTitle
        //    if (string.IsNullOrWhiteSpace(this.txtProblemTitle.Text))
        //    {
        //        msgList.Add("ProblemTitle is Required.");
        //    }
        //    else
        //    {
        //        if (this.txtProblemTitle.Text.Length > 100)
        //        {
        //            msgList.Add("ProblemTitle can't over 100 characters.");
        //        }
        //    }

        //    //檢查TypeOfProblem
        //    if (!int.TryParse(this.ddlTypeOfProblem.SelectedValue, out int typePro))
        //    {
        //        msgList.Add("TypeOfProblem must a number.");
        //    }
        //    else
        //    {
        //        if (!(typePro > -1 && typePro < 3))
        //        {
        //            msgList.Add("type can't over 100 characters.");
        //        }
        //    }

        //    //檢查Ans
        //    char[] delimiterChars = { ';', '；' };
        //    char[] unwanteddelimiterChars = { '-', '~', ',', '.', ' ' };
        //    //string[] unwanteddelimiterChars = {"-","~",",","." ," "};
        //    var _ckAns = this.txtAns.Text.Split(delimiterChars);
        //    if (string.IsNullOrWhiteSpace(this.txtAns.Text))
        //    {
        //        if (typePro != 2)//非填空題
        //        {
        //            msgList.Add("Ans is Required.");
        //        }
        //        else {/*填空題可以不用答案*/}
        //    }
        //    else
        //    {
        //        if (_ckAns.Length < 2)
        //        {
        //            msgList.Add("Type is Multiple Question,Please have more Answers.");
        //        }
        //        else
        //        {
        //            if (this.txtAns.Text.Split(unwanteddelimiterChars).Length > 1)
        //            {
        //                msgList.Add("Please use ; as delimiterchars");
        //            }
        //        }
        //    }

        //    //檢查Required            
        //    errorMsgList = msgList;
        //    if (msgList.Count == 0)
        //        return true;
        //    else
        //        return false;
        //}
        //#endregion

        //protected void btnSumit_Click(object sender, EventArgs e)
        //{
        //    List<string> msgList = new List<string>();
        //    if (!this.CheckInput(out msgList))
        //    {
        //        this.ltMsg.Text = string.Join("<br/>", msgList);
        //        return;
        //    }

        //    string _questionid = Request.QueryString["QuestionnaireNumber"] + "-" + Request.QueryString["ProblemID"];
        //    //for (int i = 1; i < 10; i++)
        //    //{
        //    //    if (Request.QueryString["QuestionID"+i] != null)
        //    //    {
        //            this.Session["QuestionID"+1] = _questionid;
        //    //        break;
        //    //    }

        //    //}
        //    this.Session[_questionid + "ProblemID"] = Request.QueryString["ProblemID"];
        //    this.Session[_questionid + "ProblemTitle"] = this.txtProblemTitle.Text;
        //    this.Session[_questionid + "TypeOfProblem"] = this.ddlTypeOfProblem.SelectedValue;
        //    this.Session[_questionid + "Required"] = this.ckbRequired.Checked;
        //    char[] delimiterChars = { ';', '；' };

        //    this.Session["ProblemID"] += Request.QueryString["ProblemID"] + ";";
        //    string strAns = this.txtAns.Text;

        //    string[] words = strAns.Split(delimiterChars);

        //    string[] arrayAns;

        //    arrayAns = words.Where(s => !string.IsNullOrEmpty(s)).ToArray();//查空
        //    for (int i = 0; i < arrayAns.Length; i++)
        //    {
        //        this.Session[_questionid+"Ans" + (i + 1)] = arrayAns[i];
        //    }

        //    BANG();
        //}


        //protected void btnCancel_Click(object sender, EventArgs e)
        //{
        //    BANG();
        //}
        //void BANG()
        //{
        //    var urlass = Request.Url.Query;
        //    Response.Redirect("/SystemAdmin/AdminQuestionnaireQuestion.aspx" + urlass);

        //}
    }
}