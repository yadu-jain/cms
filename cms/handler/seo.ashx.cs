using System;
//using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;

namespace cms.handler
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    public class seo : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context) {
            context.Response.ContentType = "application/json";

            try {
                object objSession = context.Session["IsLoggedIn"].ToString();
                if (objSession == null) { context.Response.Redirect("Default.aspx"); }
            } catch (Exception ex) { context.Response.Redirect("cms_login.aspx"); }
            
            if (context.Request.HttpMethod == "POST") {
                // do nothing
            } else {
                try {
                    string method = context.Request.QueryString["m"].ToString();
                    switch (method) {
                        case "gsc": // get seo detail from comapany_seo_detail
                            //int intCompanyId = Convert.ToInt32(context.Request.QueryString["company_id"]);
                            context.Response.Write(GetCompaniySEODetails());
                            break;
                        default:
                            break;
                    }
                } catch (Exception ex) {
                    clsUtils.LogMessageToFile("ProcessRequest: [" + context.Session["UserId"] + "]" + ex.Message, "handler.seo.log");
                    //throw;
                }
            }
        }
        protected string GetCompaniySEODetails() {
            string jsonObj = null;
            try {
                clsDB dbObj = new clsDB();
                DataSet ds = dbObj.ExecuteSelect("cms_get_company_seo_details", CommandType.StoredProcedure, 180);
                jsonObj = clsUtils.DataTableToJSONString(ds.Tables[0]);
            } catch (Exception ex) {
                clsUtils.LogMessageToFile("GetCompaniySEODetails: " + ex.Message, "handler.seo.log");
                return "{'res':0}";
            }
            return jsonObj;
        }
        public bool IsReusable { get { return false; } }
    }
}
