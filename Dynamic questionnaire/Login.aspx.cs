using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dynamic_questionnaire
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(this.Session["UserLoginInfo"] != null)
            {
                this.plcLogin.Visible = false;
                Response.Redirect("/SystemAdmin/AdminQuestionnaireList.aspx");
            }
            else
            {
                this.plcLogin.Visible = true;
            }
        }

        protected void btnLogin_Click(object sender,EventArgs e)
        {
            string inp_Account = this.txtAccount.Text;
            string inp_PWD = this.txtPWD.Text;

            if (string.IsNullOrEmpty(inp_Account) || string.IsNullOrEmpty(inp_PWD))
            {
                this.ltlMsg.Text = "Account/PWD is required.";
                return;
            }
            var dr = DB.DBHelper.GetUserInfoByAccount(inp_Account);

            if(string.Compare(dr["Account"].ToString(), inp_Account,true) == 0 &&
                string.Compare(dr["PassWord"].ToString(), inp_PWD, false) == 0)
            {
                this.Session["UserLoginInfo"] = inp_Account;
                Response.Redirect("/SystemAdmin/AdminQuestionnaireList.aspx");
            }
            else
            {
                this.ltlMsg.Text = "Login fail. Please check Account/PWD.";
                return;
            }


        }
    }
}