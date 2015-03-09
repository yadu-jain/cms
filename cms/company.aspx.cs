using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace cms
{
    public partial class company : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try { object objSession = Session["IsLoggedIn"]; if (objSession == null) { Response.Redirect("Default.aspx"); } }
            catch (Exception ex) { Response.Redirect("Default.aspx"); }
        }
    }
}
