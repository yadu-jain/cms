using System;
//using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;
using System.Web.Script.Serialization;

namespace cms.handler
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class trip : IHttpHandler, IRequiresSessionState
    {

        string LogFile = "handler_trip_logs.log";
        
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            // checking is user loged in 
            try {
                object objSession = context.Session["IsLoggedIn"].ToString();
                if (objSession == null) { context.Response.Redirect("Default.aspx"); }
            } catch (Exception ex) { context.Response.Redirect("cms_login.aspx"); }

            if (context.Request.HttpMethod == "POST") {
                // do nothing
            }
            else {
                try {
                    string method = context.Request.QueryString["m"].ToString();
                    switch (method) {
                        case "gal": // trip: Get all Companies List
                            context.Response.Write(GetAmenityList());
                            break;
                        case "gcl": // trip: Get all Companies List
                            string strCompanyName = context.Request.QueryString["company_name"].ToString();
                            context.Response.Write(GetCompanies(strCompanyName));
                            break;
                        case "gct": // trip: Get Company's Trip list
                            int intCompanyId = Convert.ToInt32(context.Request.QueryString["company_id"].ToString());
                            context.Session["CompanyId"] = intCompanyId;
                            context.Response.Write(GetCompanyTrips(intCompanyId));
                            break;
                        case "tab": // trip: Get active bus of company's trip
                            intCompanyId = Convert.ToInt32(context.Session["CompanyId"]);//3902;// Convert.ToInt32(context.Request.QueryString["company_id"]);
                            int intTripId = Convert.ToInt32(context.Request.QueryString["trip_id"]);
                            //context.Session["TripId"] = intTripId;
                            context.Response.Write(GetTripActiveBus(intTripId, intCompanyId));
                            break;
                        case "gtf": // trip: Get Selected Trips features
                            intCompanyId = Convert.ToInt32(context.Session["CompanyId"]);//3902;// Convert.ToInt32(context.Request.QueryString["company_id"]);
                            intTripId = Convert.ToInt32(context.Request.QueryString["trip_id"]);
                            context.Session["TripId"] = intTripId;
                            context.Response.Write(GetTripFeatues(intTripId, intCompanyId));
                            break;
                        case "gbd": // trip: Get Bus nos list
                            intTripId = Convert.ToInt32(context.Request.QueryString["trip_id"]);
                            context.Session["TripId"] = intTripId;
                            intCompanyId = Convert.ToInt32(context.Session["CompanyId"]);
                            context.Response.Write(GetTripBusDetail(intTripId, intCompanyId));
                            break;
                        case "utf": // trip: Update Trip features
                            intTripId = Convert.ToInt32(context.Session["TripId"]);
                            intCompanyId = Convert.ToInt32(context.Session["CompanyId"]);
                            context.Session["TripId"] = intTripId;
                            string strFeatures = context.Request.QueryString["features"].ToString();
                            string strUserId = context.Session["UserId"].ToString();
                            context.Response.Write(UpdateTripFeatures(strUserId, intTripId, intCompanyId, strFeatures));
                            break;
                        case "mbt": // trip: add/map new bus no
                            intTripId = Convert.ToInt32(context.Session["TripId"]);
                            intCompanyId = Convert.ToInt32(context.Session["CompanyId"]);
                            string strBusNo = context.Request.QueryString["bus_no"].ToString();
                            strBusNo = strBusNo.Replace('_', ' ');
                            strUserId = context.Session["UserId"].ToString();
                            context.Response.Write(AddMapBusNo(strUserId, strBusNo, intTripId, intCompanyId));
                            break;
                        default:
                            // do nothing yet
                            break;
                    }
                } catch (System.Exception ex) {
                    clsUtils.LogMessageToFile("ProcessRequest: [" + context.Session["UserId"] + "]" + ex.Message, LogFile);
                }
            }
        }

        protected string GetAmenityList() {
            string retJSON = null;
            try {
                clsDB dbObj = new clsDB();
                DataSet ds = dbObj.ExecuteSelect("cms_get_amenities", CommandType.StoredProcedure, 180);
                retJSON = clsUtils.DataTableToJSONString(ds.Tables[0]);
            } catch (Exception ex) {
                clsUtils.LogMessageToFile("GetAmenityList: " + ex.Message, LogFile);
            }
            return retJSON;
        }

        protected string GetCompanies(string strCompanyName) {
            string retJSON = null;
            try {
                clsDB dbObj = new clsDB();
                dbObj.AddParameter("CompanyName", strCompanyName, 50);
                //dbObj.AddParameter("PageNumber", 1);
                //dbObj.AddParameter("PageSize", 1000);
                DataSet ds = dbObj.ExecuteSelect("cms_get_companies_page_wise", CommandType.StoredProcedure, 180);
                retJSON = clsUtils.DataTableToJSONString(ds.Tables[0]);
            }
            catch (Exception ex) {
                clsUtils.LogMessageToFile("GetCompanies: [" + strCompanyName + "]" + ex.Message, LogFile);
            }
            return retJSON;
        }

        protected string GetCompanyTrips(int intCompanyId) {
            string retJSON = null;
            try {
                clsDB dbObj = new clsDB();
                dbObj.AddParameter("CompanyId", intCompanyId);
                //dbObj.AddParameter("PageNumber", 1);
                //dbObj.AddParameter("PageSize", 1000);
                DataSet ds = dbObj.ExecuteSelect("cms_detailed_trip_info_sWaRtHi", CommandType.StoredProcedure, 180);
                retJSON = clsUtils.DataTableToJSONString(ds.Tables[0]);
            } catch (Exception ex) {
                clsUtils.LogMessageToFile("GetCompanyTrips: [" + intCompanyId + "]" + ex.Message, LogFile);
            }
            return retJSON;
        }

        protected string GetTripActiveBus(int intTripId, int intCompanyId) {
            string retJSON = "";
            try {
                JavaScriptSerializer js = new JavaScriptSerializer();
                clsDB dbObj = new clsDB();
                dbObj.AddParameter("CompanyId", intCompanyId);
                dbObj.AddParameter("TripId", intTripId);
                dbObj.AddParameter("For", "ty", 20);
                DataSet ds = dbObj.ExecuteSelect("cms_get_serve_bus_no_for_pic", CommandType.StoredProcedure, 180);
                string keyValues = clsUtils.DataTableToJSONString(ds.Tables[0]);
                retJSON = js.Serialize(keyValues).ToString();
            } catch (Exception ex) {
                clsUtils.LogMessageToFile("GetTripActiveBus: [" + intTripId + "][" + intCompanyId + "]" + ex.Message, LogFile);
            }
            return retJSON;
        }

        protected string GetTripFeatues(int intTripId, int intCompanyId) {
            string retJSON = "";
            try {
                JavaScriptSerializer js = new JavaScriptSerializer();
                clsDB dbObj = new clsDB();
                dbObj.AddParameter("CompanyId", intCompanyId);
                dbObj.AddParameter("TripId", intTripId);
                DataSet ds = dbObj.ExecuteSelect("cms_get_trip_amenities_sWaRtHi", CommandType.StoredProcedure, 180);
                string keyValues = clsUtils.DataTableToJSONString(ds.Tables[0]);
                retJSON = js.Serialize(keyValues).ToString();
            } catch (Exception ex) {
                clsUtils.LogMessageToFile("GetTripFeatues: [" + intTripId + "][" + intCompanyId + "]" + ex.Message, LogFile);
            }
            return retJSON;
        }

        protected string GetTripBusDetail(int intTripId, int intCompanyId) {
            string jsonObj = "";
            JavaScriptSerializer retJSON = new JavaScriptSerializer();
            try {
                clsDB dbObj = new clsDB();
                dbObj.AddParameter("CompanyId", intCompanyId);
                dbObj.AddParameter("TripId", intTripId);
                DataSet dsBusNos = dbObj.ExecuteSelect("cms_get_comapnay_and_trips_buses", CommandType.StoredProcedure, 180);
                // Creating Bus List with checkbox
                string strBusList = clsUtils.DataTableToJSONString(dsBusNos.Tables[0]);
                string strTripBuses = clsUtils.DataTableToJSONString(dsBusNos.Tables[1]);
                jsonObj = "{\"CompanyBuses\":" + strBusList + ",\"TripBuses\":" + strTripBuses + ",\"CompanyId\":" + intCompanyId + "}";
            } catch (Exception ex) {
                clsUtils.LogMessageToFile("GetTripBusDetail: [" + intTripId + "][" + intCompanyId + "]" + ex.Message, LogFile);
            }
            return retJSON.Serialize(jsonObj);
        }

        protected string UpdateTripFeatures(string strUserId, int intTripId, int intCompanyId, string strFeatures) {
            try {
                clsDB dbObj = new clsDB();
                dbObj.AddParameter("TripId", intTripId);
                dbObj.AddParameter("CompanyId", intCompanyId);
                dbObj.AddParameter("Amenities", strFeatures, 100);
                dbObj.AddParameter("LastUpdatedBy", strUserId, 50);
                dbObj.ExecuteDML("cms_update_trip_amenities_sWaRtHi", CommandType.StoredProcedure, 180);
            } catch (System.Exception ex) {
                clsUtils.LogMessageToFile("UpdateTripFeatures: [" + strUserId + "][" + intTripId + "][" + intCompanyId + "][" + strFeatures + "]" + ex.Message, LogFile);
            }
            return "{\"res\":1}";
        }

        protected string AddMapBusNo(string strUserId, string strBusNo, int intTripId, int intCompanyId)
        {
            try {
                clsDB dbObj = new clsDB();
                dbObj.AddParameter("TripId", intTripId);
                dbObj.AddParameter("BusNo", strBusNo, 50);
                dbObj.AddParameter("AddedBy", strUserId, 50);
                dbObj.AddParameter("CompanyId", intCompanyId);
                dbObj.ExecuteDML("cms_add_and_map_bus", CommandType.StoredProcedure, 180);
            } catch (Exception ex) {
                clsUtils.LogMessageToFile("AddMapBusNo: [" + strUserId + "][" + strBusNo + "][" + intTripId + "][" + intCompanyId + "]" + ex.Message, LogFile);
                return "{\"res\":0}";
            }
            return "{\"res\":1}";
        }

        public bool IsReusable { get { return false; } }
    }
}
