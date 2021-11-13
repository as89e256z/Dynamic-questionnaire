using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dynamic_questionnaire.SystemAdmin
{
    public partial class QuestionnaireFillerList : System.Web.UI.Page
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


                //抓
                if (this.Request.QueryString["QuestionnaireTitle"] != null)
                {
                    try
                    {
                        this.Session["QuestionnaireTitle"] = this.Request.QueryString["QuestionnaireTitle"];

                        var dt = DB.DBHelper.GetFillerNameList(this.Session["QuestionnaireTitle"].ToString());
                        if (dt.Rows.Count > 0)
                        {
                            this.gvQustireFillerList.DataSource = dt;
                            this.gvQustireFillerList.DataBind();
                        }
                        else
                        {
                            this.gvQustireFillerList.Visible = false;
                            this.plcNoData.Visible = true;
                        }
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


        protected void btnTocsv_Click(object sender, EventArgs e)
        {
            var list = DB.DBHelper.GetFillerList(this.Session["QuestionnaireTitle"].ToString());
            if (list.Count > 0)
            {
                try
                {
                    //string filepath = @"D:\PUI\write.csv";
                    string filepath = "PUI.csv";
                    WriteToCSV(filepath, list);

                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {
                this.gvQustireFillerList.Visible = false;
                this.plcNoData.Visible = true;
            }
        }

        void WriteToCSV(string FilePath, List<DBModels.Filler> list)
        {
            Response.AddHeader("Content-disposition", "attachment; filename=\"" + FilePath + "" + "\"");
            Response.ContentType = "text/csv";
            //寫入資料
            using (var file = new StreamWriter(Response.OutputStream, Encoding.UTF8))
            {
                foreach (var item in list)
                {
                    //file.Write(item.ToString());
                    file.WriteLineAsync($"{item.Name},{item.Phone},{item.Email},{item.Ages},{item.CreateTime},{item.QuestionnaireTitle},{item.ProblemTitle},{item.Ans1},{item.Ans2},{item.Ans3},{item.Ans4},{item.Ans5},{item.Ans7},{item.Ans8},{item.Ans9}");
                }
                file.Close();
                file.Dispose();
                Response.End();
            }
        }

    }
}