using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using System.IO;
using System.Configuration;
using System.Drawing;

namespace cms
{
    public class cms : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            // checking is user loged in 
            try {
                object objSession = context.Session["IsLoggedIn"].ToString();
                if (objSession == null) { context.Response.Redirect("Default.aspx"); }
            } catch (Exception ex) { context.Response.Redirect("cms_login.aspx"); }

            if (context.Request.HttpMethod == "POST" && context.Request["pt"] == "logo") {
                var r = new System.Collections.Generic.List<ViewDataUploadFilesResult>();
                JavaScriptSerializer js = new JavaScriptSerializer();

                HttpPostedFile hpf = context.Request.Files[0] as HttpPostedFile;
                string FileName = string.Empty;
                if (HttpContext.Current.Request.Browser.Browser.ToUpper() == "IE")
                {
                    string[] files = hpf.FileName.Split(new char[] { '\\' });
                    FileName = files[files.Length - 1];
                }
                else
                    FileName = hpf.FileName;

                int intCompanyId = Convert.ToInt32(context.Session["company_id"]);
                string strAddress = context.Request["taAddress"].ToString();
                int intPinNo = Convert.ToInt32(context.Request["txtPinNo"]);
                //string strPhoneNo = context.Request[""].ToString();

                //string strEmail = context.Request["txtEmail"].ToString();
                //string strOwner = context.Request["txtOwner"].ToString();
                //string strOwnerContact = context.Request["txtOwnerContact"].ToString();
                //string strManager = context.Request["txtManager"].ToString();
                //string strManagerContact = context.Request["txtManagerContact"].ToString();

                //int intArea = Convert.ToInt32(context.Request["ddArea"]);
                int intCity = Convert.ToInt32(context.Request["ddCity"]);
                int intState = Convert.ToInt32(context.Request["ddState"]);
                string strUserId = context.Session["UserId"].ToString();
                //string strUserId = "test";

                string thumUrl = SaveCompanyLogo(intCompanyId, strAddress, intPinNo, intCity, intState, strUserId, hpf);

                r.Add(new ViewDataUploadFilesResult()
                {
                    Thumbnail_url = thumUrl,
                    Name = FileName,
                    Length = hpf.ContentLength,
                    Type = hpf.ContentType
                });
                var uploadedFiles = new { files = r.ToArray() };
                var jsonObj = js.Serialize(uploadedFiles);
                context.Response.Write(jsonObj.ToString());
            }
            else if (context.Request.HttpMethod == "POST") {
                var r = new System.Collections.Generic.List<ViewDataUploadFilesResult>();
                JavaScriptSerializer js = new JavaScriptSerializer();
                foreach (string file in context.Request.Files) {
                    HttpPostedFile hpf = context.Request.Files[file] as HttpPostedFile;
                    string FileName = string.Empty;
                    if (HttpContext.Current.Request.Browser.Browser.ToUpper() == "IE") {
                        string[] files = hpf.FileName.Split(new char[] { '\\' });
                        FileName = files[files.Length - 1];
                    } else
                        FileName = hpf.FileName;

                    if (hpf.ContentLength == 0)
                        continue;

                    string strBusNo = context.Request["hdnBusNo"].Replace('_', ' ').Trim().ToString();
                    //int intTripId = 0;// Convert.ToInt32(context.Request["hdnTripId"].ToString());
                    int intCompanyId = Convert.ToInt32(context.Request["hdnCompanyId"].ToString());

                    string strUserId = context.Session["UserId"].ToString();
                    //string thumUrl = SaveUploadedImage(strUserId, strBusNos, intTripId, intCompanyId, hpf);

                    //thumUrl = SaveUploadImageAtOnce(strUserId, strBusNos, intTripId, intCompanyId, hpf);

                    Dictionary<string, object> dctParams = new Dictionary<string, object>();
                    dctParams.Add("user", strUserId);
                    dctParams.Add("busNo", strBusNo);
                    //dctParams.Add("tripId", intTripId);
                    dctParams.Add("companyId", intCompanyId);

                    // Resize images at once
                    Dictionary<string, MemoryStream> lstMemStream = ResizeImageAtOnce(hpf);

                    // Save, Upload and Updae Images at once
                    dctParams.Add("lstMS", lstMemStream);
                    string thumUrl = SaveImagesAtOnce(dctParams);

                    // delete cache after adding new image
                    string strTripId = context.Session["TripId"].ToString();
                    string strCompanyId = context.Session["CompanyId"].ToString();
                    DeleteFromCache(strCompanyId, strTripId);

                    r.Add(new ViewDataUploadFilesResult() {
                        Thumbnail_url = thumUrl,
                        Name = FileName,
                        Length = hpf.ContentLength,
                        Type = hpf.ContentType
                    });
                    var uploadedFiles = new { files = r.ToArray() };
                    var jsonObj = js.Serialize(uploadedFiles);
                    context.Response.Write(jsonObj.ToString());
                }
            } else {
                try {
                    string method = context.Request.QueryString["m"].ToString();
                    switch (method) {
                        case "gpt": // buses: Get Bus Pics Thumb
                            int intCompanyId = Convert.ToInt32(context.Request.QueryString["company_id"].ToString());
                            context.Session["CompanyId"] = intCompanyId;
                            string strBusNo = context.Request.QueryString["bus_no"].Replace('_', ' ').ToString().Trim();
                            context.Response.Write(GetBusPicThumbs(intCompanyId, strBusNo));
                            break;
                        case "dbp": // buses: Delete bus pics
                            //Convert.ToInt32(context.Request.QueryString["company_id"].ToString());
                            string strPicid = context.Request.QueryString["pic_Ids"].ToString();
                            string strUserId = context.Session["UserId"].ToString();
                            // delete pic data from cache
                            string strTripId = context.Session["TripId"].ToString();
                            string strCompanyId = context.Session["CompanyId"].ToString();
                            DeleteFromCache(strCompanyId, strTripId);
                            
                            context.Response.Write(DeleteBusPics(strPicid, strUserId));
                            break;
                        case "sbt": // buses: Save bus Tag
                            //Convert.ToInt32(context.Request.QueryString["company_id"].ToString());
                            strPicid = context.Request.QueryString["save_data"].ToString();
                            strPicid = strPicid.Replace("\"", "").Replace("{", "").Replace("}", "").Trim();
                            strUserId = context.Session["UserId"].ToString().Trim();
                            context.Response.Write(SaveBusPicsTag(strPicid, strUserId));
                            break;
                        case "abn": // buses: activate bus no
                            strBusNo = context.Request.QueryString["bus_no"].ToString().Replace('_', ' ');
                            int intTripId = Convert.ToInt32(context.Session["TripId"]);
                            intCompanyId = Convert.ToInt32(context.Session["CompanyId"]);
                            strUserId = context.Session["UserId"].ToString();
                            context.Response.Write(ActivateBus(intCompanyId, intTripId, strBusNo, strUserId));
                            break;
                        case "gpd": // company: Get All Provider List 
                            context.Response.Write(GetProvidersDropDown());
                            break;
                        case "gpc": // company: Get Provider's Companies
                            int intProviderId = Convert.ToInt32(context.Request.QueryString["provider_id"]);
                            context.Response.Write(GetProvidersCompanies(intProviderId));
                            break;
                        case "gci": // company: Get Provider's Companies
                            intCompanyId = Convert.ToInt32(context.Request.QueryString["company_id"]);
                            // save companyId in session so it can use further
                            context.Session["company_id"] = intCompanyId.ToString();
                            context.Response.Write(GetCompanyInfo(intCompanyId));
                            break;
                        case "gcb": // company: Get Company's Branches
                            intCompanyId = Convert.ToInt32(context.Request.QueryString["company_id"]);
                            context.Response.Write(GetCompanyBranches(intCompanyId));
                            break;
                        case "gacs": // company: Get area city state list
                            context.Response.Write(GetAreaCityStateList());
                            break;
                        case "scb": // company: Save Company Branch
                            int intBranchId = Convert.ToInt32(context.Request.QueryString["Id"]);
                            string strBranchName = context.Request.QueryString["Name"].Trim().ToString();
                            string strBranchAddress = context.Request.QueryString["Add"].ToString();
                            intCompanyId = Convert.ToInt32(context.Session["company_id"]);
                            int intCityId = Convert.ToInt32(context.Request.QueryString["Ct"]);
                            int intStateId = Convert.ToInt32(context.Request.QueryString["St"]);
                            int intPinNo = Convert.ToInt32(context.Request.QueryString["Pin"]);
                            string strBranchContact = context.Request.QueryString["Cont"].Trim().ToString();
                            //string strEmail = context.Request.QueryString["Email"].ToString();
                            //string strManager = context.Request.QueryString["Man"].ToString();
                            //string strManagerContact = context.Request.QueryString["MCont"].ToString();
                            strUserId = context.Session["UserId"].ToString();
                            //strUserId = "test";
                            context.Response.Write(SaveCompanyBranch(intBranchId, strBranchName, strBranchAddress, intCompanyId, intCityId, intStateId, intPinNo, strBranchContact, strUserId));
                            break;
                        default:
                            break;
                    }
                } catch (Exception ex) {
                   clsUtils.LogMessageToFile("ProcessRequest: " + ex.Message, "");
                }
            }
        }
        protected void DeleteFromCache(string strCompanyId, string strTripId) {
            try {
                string strSizes = ConfigurationManager.AppSettings["CMS_Bus_Image_Size"].ToString();
                string[] arrSizes = strSizes.Split(',');
                
                for (int j = 0; j < arrSizes.Length; j++) {
                    string strSize = arrSizes[j].Replace('x', '_');
                    string strCacheKey = "bpl_" + strCompanyId + "_" + strTripId + "_" + strSize;
                    clsUtils.DeletePicFromCache(strCacheKey);
                }
            }
            catch (Exception ex) {
                clsUtils.LogMessageToFile("DeleteFromCache: [" + strCompanyId + "][" + strTripId + "]" + ex.Message, "");
            }
        }
        protected string ActivateBus(int intCompanyId, int intTripId, string strBusNo, string strUserId) {
            try {
                JavaScriptSerializer js = new JavaScriptSerializer();
                clsDB dbObj = new clsDB();
                dbObj.AddParameter("CompanyId", intCompanyId);
                dbObj.AddParameter("TripId", intTripId);
                dbObj.AddParameter("BusNo", strBusNo, 50);
                dbObj.AddParameter("LastUpdatedBy", strUserId, 50);
                int ds = dbObj.ExecuteDML("cms_activate_trip_bus", CommandType.StoredProcedure, 180);
            } catch (Exception ex) {
                clsUtils.LogMessageToFile("ActivateBus: [" + intCompanyId + "][" + intTripId + "][" + strBusNo + "][" + strUserId + "]" + ex.Message, "");
                return "{\"res\":0}";
            }
            return "{\"res\":1}"; 
        }
        protected string SaveCompanyLogo(int intCompanyId, string strAddress, int intPinNo, /*string strPhoneNo, string strEmail, string strOwner, string strOwnerContact, string strManager, string strManagerContact, int intArea,*/ int intCity, int intState, string strUserId, HttpPostedFile hpf) {
            string strThumbUrl = null;
            try {
                string strSizes = ConfigurationManager.AppSettings["Company_Logo_Size"].ToString();

                string[] arrSizes = strSizes.Split('x');
                Size sSize = new Size(Convert.ToInt32(arrSizes[0]), Convert.ToInt32(arrSizes[1]));
                MemoryStream msImage = new MemoryStream();

                int sourceWidth = System.Drawing.Image.FromStream(hpf.InputStream).Width;
                int sourceHeight = System.Drawing.Image.FromStream(hpf.InputStream).Height;

                if (sSize.Width < sourceWidth || sSize.Height < sourceHeight)
                    msImage = clsUtils.ResizeImageInJpeg(hpf.InputStream, sSize);
                else
                    msImage = clsUtils.ResizeImageInJpeg(hpf.InputStream, new Size(sourceWidth, sourceHeight));
                //msImage = (MemoryStream)hpf.InputStream;

                string strSize = sSize.Width.ToString() + 'x' + sSize.Height.ToString();
                string strLink = "";

                int intLinkId = 0;
                clsDB dbObj = new clsDB();
                dbObj.AddParameter("ID", 0);
                dbObj.AddParameter("companyId", intCompanyId);
                dbObj.AddParameter("companyAddress", strAddress, 500);
                dbObj.AddParameter("pinNo", intPinNo);
                //dbObj.AddParameter("phoneNo", strPhoneNo, 50);

                //dbObj.AddParameter("comapanyEmail", strEmail, 100);
                //dbObj.AddParameter("ownerName", strOwner, 100);
                //dbObj.AddParameter("ownerContact", strOwnerContact, 100);
                //dbObj.AddParameter("managerName", strManager, 100);
                //dbObj.AddParameter("managerContact", strManagerContact, 2000);

                //dbObj.AddParameter("areaId", intArea);
                dbObj.AddParameter("cityId", intCity);
                dbObj.AddParameter("stateId", intState);
                dbObj.AddParameter("createdBy", strUserId, 100);
                //dbObj.AddParameter("SIZE", strSize, 50);
                DataSet ds = dbObj.ExecuteSelect("cms_add_company_info", CommandType.StoredProcedure, 180);
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                    intLinkId = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]);
                else
                    throw new System.Exception("Failed to get Link id !");
                string strFileName = intLinkId + "_" + intCompanyId + ".Jpeg";
                AWS_S3 apiS3 = new AWS_S3();
                string strBucket = ConfigurationManager.AppSettings["Company_Logo_Bucket"].ToString();
                string strFolder = ConfigurationManager.AppSettings["Company_Logo_Folder"].ToString();
                strLink = apiS3.uploadFile(msImage, strBucket, strFolder, strFileName, new Dictionary<string, string>());
                dbObj = new clsDB();
                dbObj.AddParameter("ID", intLinkId);
                dbObj.AddParameter("logoLink", strLink, 500);
                dbObj.ExecuteDML("cms_add_company_info", CommandType.StoredProcedure, 180);
            } catch (Exception ex) { }
            return strThumbUrl;
        }
        protected string SaveCompanyBranch(int intBranchId, string strBranchName, string strBranchAddress, int intCompanyId, int intCityId, int intStateId, int intPinNo, string strBranchContact,/* string strEmail, string strManager, string strManagerContact,*/ string strUserId) {
            try {
                clsDB dbObj = new clsDB();
                dbObj.AddParameter("branchId", intBranchId);
                dbObj.AddParameter("branchName", strBranchName, 100);
                dbObj.AddParameter("branchAddress", strBranchAddress, 500);
                dbObj.AddParameter("pinNo", intPinNo);
                dbObj.AddParameter("phoneNo", strBranchContact, 100);
                //dbObj.AddParameter("branchEmail", strEmail, 100);
                //dbObj.AddParameter("managerName", strManager, 100);
                //dbObj.AddParameter("managerContact", strManagerContact, 2000);
                dbObj.AddParameter("companyId", intCompanyId);
                dbObj.AddParameter("cityId", intCityId);
                dbObj.AddParameter("stateId", intStateId);
                if (intBranchId == 0) { dbObj.AddParameter("createdBy", strUserId, 100); }
                else { dbObj.AddParameter("lastUpdatedBy", strUserId, 100); }
                dbObj.ExecuteDML("cms_add_update_company_branch", CommandType.StoredProcedure, 180);
            } catch (Exception ex) {
                clsUtils.LogMessageToFile("SaveCompanyBranch: [" + intBranchId + "][" + strBranchName + "][" + strBranchAddress + "][" + intCompanyId + "][" + intCityId + "][" + intStateId + "][" + intPinNo + "][" + strBranchContact + "][" + strUserId + "]" + ex.Message, "");
                return "{\"res\":0}";
            }
            return "{\"res\":1,\"company_id\":" + intCompanyId + "}";
        }
        protected string GetAreaCityStateList() {
            JavaScriptSerializer jsonObj = new JavaScriptSerializer();
            var lst = new Dictionary<string, object>();
            try {
                clsDB dbObj = new clsDB();
                DataSet ds = dbObj.ExecuteSelect("cms_state_city_area_list", CommandType.StoredProcedure, 180);
                string strStates = clsUtils.DataTableToJSONString(ds.Tables[0]);
                string strCities = clsUtils.DataTableToJSONString(ds.Tables[1]);
                lst.Add("states", strStates);
                lst.Add("cities", strCities);
            } catch (Exception ex) {
                clsUtils.LogMessageToFile("GetAreaCityStateList: " + ex.Message, "");
            }
            return jsonObj.Serialize(lst);
        }
        protected string GetCompanyBranches(int intCompanyId) {
            string jsonObj = null;
            try {
                clsDB dbObj = new clsDB();
                dbObj.AddParameter("companyId", intCompanyId);
                DataSet ds = dbObj.ExecuteSelect("cms_get_company_branches", CommandType.StoredProcedure, 180);
                if (ds.Tables[0].Rows.Count > 0)
                    jsonObj = clsUtils.DataTableToJSONString(ds.Tables[0]);
                else
                    return @"{""sEcho"": 1,""iTotalRecords"": ""0"",""iTotalDisplayRecords"": ""0"",""aaData"": []}";
            } catch (Exception ex) {
                clsUtils.LogMessageToFile("GetCompanyBranches: [" + intCompanyId + "]" + ex.Message, "");
            }
            return jsonObj;
        }
        protected string GetCompanyInfo(int intCompanyId) {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string jsonObj = null;
            try {
                clsDB dbObj = new clsDB();
                dbObj.AddParameter("companyId", intCompanyId);
                DataSet ds = dbObj.ExecuteSelect("cms_get_cms_companies", CommandType.StoredProcedure, 180);
                if (ds.Tables[0].Rows.Count > 0)
                    jsonObj = clsUtils.DataTableToJSONString(ds.Tables[0]);
                else
                    jsonObj = "{\"res\":0}";
            } catch (Exception ex) {
                clsUtils.LogMessageToFile("GetCompanyInfo: [" + intCompanyId + "]" + ex.Message, "");
            }
            return serializer.Serialize(jsonObj);
        }
        protected string GetProvidersCompanies(int intProviderId) {
            string jsonObj = "";
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                clsDB dbObj = new clsDB();
                dbObj.AddParameter("providerId", intProviderId);
                DataSet dsCompanies = dbObj.ExecuteSelect("cms_get_providers_companies", CommandType.StoredProcedure, 180);

                if (dsCompanies != null && dsCompanies.Tables[0].Rows.Count > 0)
                    foreach (DataRow drCompany in dsCompanies.Tables[0].Rows)
                        jsonObj += "<option value='" + drCompany["company_id"].ToString() + "'>" + drCompany["company_name"].ToString() + "</option>";
                else
                    jsonObj = "<option value='-1'>Active Company Not available.</option>";
            } catch (Exception ex) {
                clsUtils.LogMessageToFile("GetProvidersCompanies: [" + intProviderId + "]" + ex.Message, "");
            }
            return js.Serialize(jsonObj);
        }
        protected string GetProvidersDropDown() {
            string jsonObj = "";
            JavaScriptSerializer js = new JavaScriptSerializer();
            try {
                clsDB dbObj = new clsDB();
                DataSet dsProviders = dbObj.ExecuteSelect("cms_get_providers_list", CommandType.StoredProcedure, 180);

                if (dsProviders != null && dsProviders.Tables[0].Rows.Count > 0) {
                    foreach (DataRow drProvider in dsProviders.Tables[0].Rows)
                        jsonObj += "<option value='" + drProvider["provider_id"].ToString() + "'>" + drProvider["provider_name"].ToString() + "</option>";
                }
            } catch (Exception ex) {
                clsUtils.LogMessageToFile("GetProvidersDropDown: " + ex.Message, "");
            }
            return js.Serialize(jsonObj);
        }
        protected string SaveBusPicsTag(string strPIcIds, string strUserId) {
            try {
                clsDB dbObj = new clsDB();
                dbObj.AddParameter("PicIds", strPIcIds, 5000);
                dbObj.AddParameter("LastUpdatedBy", strUserId, 50);
                dbObj.ExecuteDML("cms_update_bus_pic_tag", CommandType.StoredProcedure, 180);
            } catch (Exception ex) {
                clsUtils.LogMessageToFile("SaveBusPicsTag: [" + strPIcIds + "][" + strUserId + "]" + ex.Message, "");
                return "{\"res\":0}";
            }
            return "{\"res\":1}"; 
        }
        protected string DeleteBusPics(string strPIcIds, string strUserId) {
            try {
                clsDB dbObj = new clsDB();
                dbObj.AddParameter("PicIds", strPIcIds, 5000);
                dbObj.AddParameter("LastUpdatedBy", strUserId, 50);
                dbObj.ExecuteDML("cms_delete_bus_pics", CommandType.StoredProcedure, 180);
            } catch (Exception ex) {
                clsUtils.LogMessageToFile("DeleteBusPics: [" + strPIcIds + "][" + strUserId + "]" + ex.Message, "");
                return "{\"res\":0}"; 
            }
            return "{\"res\":1}"; 
        }
        protected string SaveImagesAtOnce(Dictionary<string, object> dctParams) {
            string strThumbUrl = "http://s3-ap-southeast-1.amazonaws.com/ty-buspics/prod/";
            try {
                object obj = null;
                clsDB dbObj = new clsDB();
                dbObj.AddParameter("Id", -1);
                
                dctParams.TryGetValue("busNo", out obj);
                string strBusNo = obj.ToString();
                dbObj.AddParameter("BusNo", strBusNo, 50);
                
                dctParams.TryGetValue("companyId", out obj);
                int intCompanyId = Convert.ToInt16(obj);
                dbObj.AddParameter("CompanyId", intCompanyId);

                dctParams.TryGetValue("user", out obj);
                dbObj.AddParameter("AddedBy", obj.ToString(), 50);

                //dbObj.AddParameter("Size", "1024x768", 50);
                //List<string> keyList = new List<string>(dctMS.Keys);
                //string strSizes = String.Join("^", keyList.ToArray());
                //dbObj.AddParameter("SIZE", strSizes, 50);
                int intLinkId;

                DataSet ds = dbObj.ExecuteSelect("cms_add_bus_pic_with_all_sizes", CommandType.StoredProcedure, 180);
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                    intLinkId = Convert.ToInt32(ds.Tables[0].Rows[0]["id"]);
                else
                    throw new System.Exception("Failed to get Link id !");
                string strFileName = strBusNo.Replace(' ','_') + "_" + intCompanyId +"_"+ intLinkId + "_";
                string strOFileName = strFileName + "1024x768.jpeg";

                Dictionary<string, MemoryStream> dctMS = new Dictionary<string, MemoryStream>();
                dctParams.TryGetValue("lstMS", out obj);
                dctMS = (Dictionary<string, MemoryStream>)obj;

                string strSizes = ConfigurationManager.AppSettings["CMS_Bus_Image_Size"].ToString();
                string[] arrSizes = strSizes.Split(',');
                string strLink, strJSONAllLinks = "{";
                
                for (int j = 0; j < arrSizes.Length; j++) {
                    AWS_S3 apiS3 = new AWS_S3();
                    MemoryStream ms = new MemoryStream();
                    
                    string[] arrPosition = arrSizes[j].Split('x');
                    string strS3Size = arrSizes[j].ToString();
                    
                    if (arrPosition[arrPosition.Length - 1].ToString() == "WM") {
                        apiS3 = new AWS_S3();
                        //strFileName += strS3Size + ".jpeg";
                        ms = new MemoryStream();
                        strJSONAllLinks += "\"" + strS3Size + "\":\"" + strFileName + strS3Size + ".jpeg" + "\",";
                        dctMS.TryGetValue(strS3Size, out ms);
                        strLink = apiS3.uploadFileAdvacned(ms, strFileName + strS3Size + ".jpeg", new Dictionary<string, string>());
                        //arrSizes[j].Substring(arrSizes[j].Length - 3, 3);
                        strS3Size = arrSizes[j].Replace("xWM", "");
                    }

                    apiS3 = new AWS_S3();
                    ms = new MemoryStream();
                    strJSONAllLinks += "\"" + strS3Size + "\":\"" + strFileName + strS3Size + ".jpeg" + "\",";
                    dctMS.TryGetValue(strS3Size, out ms);
                    strLink = apiS3.uploadFileAdvacned(ms, strFileName + strS3Size + ".jpeg", new Dictionary<string, string>());
                    //arrSizes[j].Substring(arrSizes[j].Length - 3, 3);
                }
                strJSONAllLinks = strJSONAllLinks.TrimEnd(',');
                strJSONAllLinks += "}";
                
                dbObj = new clsDB();
                dbObj.AddParameter("Id", intLinkId);
                dbObj.AddParameter("OLink", strOFileName, 50);
                dbObj.AddParameter("Links", strJSONAllLinks, 1000);
                dbObj.ExecuteDML("cms_add_bus_pic_with_all_sizes", CommandType.StoredProcedure, 180);
                strThumbUrl += strOFileName.Replace("1024x768.jpeg", "192x144.jpeg");
            } catch (Exception ex) {
                dctParams.Remove("lstMS");
                clsUtils.LogMessageToFile("SaveImagesAtOnce: [" + dctParams.Keys + "][" + dctParams.Values + "]" + ex.Message, "");
            }
            return strThumbUrl;
        }
        protected Dictionary<string, MemoryStream> ResizeImageAtOnce(HttpPostedFile hpf) {
            Dictionary<string, MemoryStream> dctMemStream = new Dictionary<string, MemoryStream>();
            try {
                string strSizes = ConfigurationManager.AppSettings["CMS_Bus_Image_Size"].ToString();
                string[] arrSizes = strSizes.Split(',');
                for (int j = 0; j < arrSizes.Length; j++) {
                    string[] arrPosition = arrSizes[j].Split('x');
                    Size sSize = new Size(Convert.ToInt32(arrPosition[0]), Convert.ToInt32(arrPosition[1]));
                    MemoryStream msImage = new MemoryStream();
                    msImage = clsUtils.ResizeImageInJpeg(hpf.InputStream, sSize);
                    string strSize = sSize.Width.ToString() + "x" + sSize.Height.ToString();
                    dctMemStream.Add(strSize, msImage);
                    if (arrPosition[arrPosition.Length - 1].ToString() == "WM") {
                        MemoryStream msWaterMarked = new MemoryStream();
                        string strWMImage = ConfigurationManager.AppSettings["Default_WM_Image"].ToString();
                        string strWaterMarkedImageWithPath = HttpContext.Current.Server.MapPath(strWMImage);
                        msWaterMarked = clsUtils.AddWaterMarkInRightBottomInJpeg(msImage, strWaterMarkedImageWithPath);
                        strSize = strSize + "xWM";
                        dctMemStream.Add(strSize, msWaterMarked);
                    }
                }
            } catch (Exception ex) {
                //throw new Exception("Pics not uploaded succesfull please retry again.");
                clsUtils.LogMessageToFile("ResizeImageAtOnce: [" + hpf.FileName + "][" + hpf.ContentType + "][" + hpf.ContentLength + "]" + ex.Message, "");
            }
            return dctMemStream;
        }
        protected string GetBusPicThumbs(int intCompanyId, string strBusNo) {
            string jsonObj = "";
            JavaScriptSerializer js = new JavaScriptSerializer();
            try {
                clsDB dbObj = new clsDB();
                dbObj.AddParameter("CompanyId", intCompanyId);
                dbObj.AddParameter("BusNo", strBusNo, 50);
                DataSet ds = dbObj.ExecuteSelect("cms_get_bus_pic_thumb", CommandType.StoredProcedure, 180);
                jsonObj = clsUtils.DataTableToJSONString(ds.Tables[0]);
            } catch (Exception ex) {
                clsUtils.LogMessageToFile("GetBusPicThumbs: [" + intCompanyId + "][" + strBusNo + "]" + ex.Message, ""); 
                return "{\"res\":0}";
            }
            return js.Serialize(jsonObj);
        }
        public bool IsReusable { get { return false; } }
    }

    partial class ViewDataUploadFilesResult {
        public string strThumbnail_url;
        public string Thumbnail_url {
            get { return strThumbnail_url; }
            set { strThumbnail_url = value; }
        }
        public string strName;
        public string Name {
            get { return strName; }
            set { strName = value; }
        }
        public int intLength;
        public int Length {
            get { return intLength; }
            set { intLength = value; }
        }
        public string strType;
        public string Type {
            get { return strType; }
            set { strType = value; }
        }
    }
}
