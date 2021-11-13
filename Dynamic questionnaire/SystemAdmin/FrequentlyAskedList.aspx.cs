using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dynamic_questionnaire.SystemAdmin
{
    public partial class FrequentlyAskedList : System.Web.UI.Page
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
                QuestionInput(account);//導入資料庫
            }
        }


        private void QuestionInput(string account)
        {
            var list = DB.DBHelper.GetAccountQuestionName(account);

            var dt = DB.DBHelper.GetQuestionList(list);

            for (int i = dt.Count - 1; i >= 0; i--)
            {
                if (dt[i].FrequentlyAsked == false)
                    dt.Remove(dt[i]);
            }
            if (dt.Count > 0)
            {
                this.gvQuestionUsedList.DataSource = dt;
                this.gvQuestionUsedList.DataBind();
            }
            else
            {//沒資料
                this.gvQuestionUsedList.Visible = false;
                this.plcNoData.Visible = true;
            }
        }

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

        class MyList<T> : IEnumerator
        {
            List<T> list = new List<T>();
            public object current = null;
            public object Current
            {
                get { return current; }
            }
            int icout = 0;
            public bool MoveNext()
            {
                if (icout >= list.Count)
                {
                    return false;
                }
                else
                {
                    current = list[icout];
                    icout++;
                    return true;
                }
            }

            public void Reset()
            {
                icout = 0;
            }

            public void Add(T t)
            {
                list.Add(t);
            }

            public void Remove(T t)
            {
                if (list.Contains(t))
                {
                    if (list.IndexOf(t) <= icout)
                    {
                        icout--;
                    }
                    list.Remove(t);
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/SystemAdmin/AdminQuestionnaireList.aspx");
        }
    }
}