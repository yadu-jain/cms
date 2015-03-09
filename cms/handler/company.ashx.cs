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
    //[WebService(Namespace = "http://tempuri.org/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class company : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            context.Response.ContentType = "application/json";

            // checking is user loged in 
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
                        case "sd": // get seo detail from comapany_seo_detail
                            int intCompanyId = Convert.ToInt32(context.Request.QueryString["company_id"]);
                            context.Session["CompanyId"] = intCompanyId;
                            context.Response.Write(GetCompanySEODetails(intCompanyId));
                            break;
                        case "scsd":
                            int intId = Convert.ToInt32(context.Request.QueryString["Id"]);
                            intCompanyId = Convert.ToInt32(context.Request.QueryString["CompanyId"]);
                            string strSEOName = context.Request.QueryString["SEOName"].ToString();
                            string strSeoUrl = context.Request.QueryString["SeoUrl"].ToString();
                            string strWriteUp = context.Request.QueryString["WriteUp"].ToString();
                            string strUserId = context.Session["UserId"].ToString().Trim();
                            context.Response.Write(SaveCompanySEODetails(intId, intCompanyId, strSEOName, strSeoUrl, strWriteUp, strUserId));
                            break;
                        default:
                            break;
                    }
                } catch (Exception ex) {
                    clsUtils.LogMessageToFile("ProcessRequest: [" + context.Session["UserId"] + "]" + ex.Message, "handler.company.log");
                    //throw;
                }
            }
        }
        protected string SaveCompanySEODetails(int intId, int intCompanyId, string strSEOName, string strSeoUrl, string strWriteUp, string strUserId) {
            string jsonObj = null;
            try {
                clsDB dbObj = new clsDB();
                dbObj.AddParameter("Id", intId);
                dbObj.AddParameter("CompanyId", intCompanyId);
                dbObj.AddParameter("SEOCompanyName", strSEOName, 200);
                dbObj.AddParameter("SeoUrl", strSeoUrl, 200);
                dbObj.AddParameter("StaticWriteup", strWriteUp, SqlDbType.Text);
                dbObj.AddParameter("UserId", strUserId, 100);
                DataSet ds = dbObj.ExecuteSelect("cms_iu_company_seo_details", CommandType.StoredProcedure, 180);
                jsonObj = clsUtils.DataTableToJSONString(ds.Tables[0]);
            } catch (Exception ex) {
                clsUtils.LogMessageToFile("GetCompaniySEODetails: [" + intCompanyId + "]" + ex.Message, "handler.company.log");
                return "{'res':0}";
            }
            return jsonObj;
        }
        protected string GetCompanySEODetails(int intCompanyId) {
            string jsonObj = null;
            try {
                clsDB dbObj = new clsDB();
                dbObj.AddParameter("CompanyId", intCompanyId);
                DataSet ds = dbObj.ExecuteSelect("cms_get_company_seo_details", CommandType.StoredProcedure, 180);
                jsonObj = clsUtils.DataTableToJSONString(ds.Tables[0]);
            } catch (Exception ex) {
                clsUtils.LogMessageToFile("GetCompaniySEODetails: [" + intCompanyId + "]" + ex.Message, "handler.company.log");
                return "{'res':0}";
            }
            return jsonObj;
        }
        public bool IsReusable { get { return false; } }
    }
}
