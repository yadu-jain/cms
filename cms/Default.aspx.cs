using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;

namespace cms
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session.Clear();
            }
        }

        partial class clsUsers { public clsUser[] users { get; set; } }
        partial class clsUser
        {
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
        }
        protected string UserValidate(string strUserName, string strPassword)
        {
            try
            {
                string strJSONFilePath = Server.MapPath(ConfigurationManager.AppSettings["Login_File"].ToString());
                StreamReader r = new StreamReader(strJSONFilePath);
                string json = r.ReadToEnd();
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var model = serializer.Deserialize<clsUsers>(json);
                foreach (var user in model.users)
                {
                    if (user.UserId == strUserName && user.Password == strPassword) { return user.UserId.ToString(); }
                }
            }
            catch (Exception ex)
            {
                //throw;
            }
            return null;
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string strUserName = Context.Request.Form["username"];
            string strPassword = Context.Request.Form["password"];

            string strUserId = UserValidate(strUserName, strPassword);

            if (strUserId == null)
            {
                Session.Clear();
                lblError.InnerText = "Invalid Login/Password";
                divError.Visible = true;
                return;
            }
            else
            {
                Session.Clear();
                Session["IsLoggedIn"] = "Y";
                Session["UserId"] = strUserId;
                Session["UserName"] = strUserName;
                Response.Redirect("home.aspx");
            }
        }
    }
}
