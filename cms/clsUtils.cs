using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Web;
using System.Web.Script.Serialization;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Configuration;
using BeIT.MemCached;

namespace cms
{
    public class clsUtils {

        public static string DataTableToJSONString(DataTable dt) {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = null;

            foreach (DataRow dr in dt.Rows) {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns) {
                    row.Add(col.ColumnName.Trim(), dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

        public static MemoryStream ResizeImageInJpeg(Stream stream, Size size) {
            MemoryStream msStream = new MemoryStream();
            try {
                int sourceWidth = System.Drawing.Image.FromStream(stream).Width;
                int sourceHeight = System.Drawing.Image.FromStream(stream).Height;
                float nPercent = 0;
                float nPercentW = 0;
                float nPercentH = 0;

                nPercentW = ((float)size.Width / (float)sourceWidth);
                nPercentH = ((float)size.Height / (float)sourceHeight);

                if (nPercentH < nPercentW)
                    nPercent = nPercentH;
                else
                    nPercent = nPercentW;

                int destWidth = (int)(sourceWidth * nPercent);
                int destHeight = (int)(sourceHeight * nPercent);

                Bitmap bmp = new Bitmap(destWidth, destHeight);
                Graphics graphic = Graphics.FromImage((System.Drawing.Image)bmp);
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;

                graphic.DrawImage(System.Drawing.Image.FromStream(stream), 0, 0, destWidth, destHeight);
                graphic.Dispose();

                bmp.Save(msStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            } catch (Exception ex) {
                clsUtils.LogMessageToFile("ResizeImageInJpeg:["+size.ToString()+"] " + ex.Message, "");
            }
            return msStream;
        }

        public static MemoryStream AddWaterMarkInRightBottomInJpeg(MemoryStream msUploadedImage, string strWMImage) {
            MemoryStream msWatermarkedImage = new MemoryStream();
            try {
                Bitmap bmpStamp = new Bitmap(strWMImage);
                Bitmap bmpUpload = new Bitmap(msUploadedImage, false);
                Graphics graphicsObj = Graphics.FromImage(bmpUpload);
                Point postionWaterMark = new Point((bmpUpload.Width - (38 + bmpStamp.Width)), (bmpUpload.Height - (14 + bmpStamp.Height)));
                graphicsObj.DrawImage(bmpStamp, postionWaterMark);
                bmpUpload.Save(msWatermarkedImage, System.Drawing.Imaging.ImageFormat.Jpeg);
            } catch (Exception ex) {
                clsUtils.LogMessageToFile("AddWaterMarkInRightBottomInJpeg:[" + strWMImage + "] " + ex.Message, "");
            }
            return msWatermarkedImage;
        }

        public static void LogMessageToFile(string msg, string strFileName) {
            StreamWriter sw = null;
            string strPath = "";
            
            if(strFileName.Trim().Length == 0)
                strPath = System.Web.HttpContext.Current.Server.MapPath("LOG.txt");
            else
                strPath = System.Web.HttpContext.Current.Server.MapPath(strFileName);

            sw = File.AppendText(strPath);
            try {
                string logLine = System.String.Format("{0:G}: {1}.", System.DateTime.Now, msg);
                sw.WriteLine(logLine);
            } finally { sw.Close(); }
        }

        public static void DeletePicFromCache(string strCacheKey) {
            try {
                string strHost = System.Configuration.ConfigurationManager.AppSettings["memcacheHost"];
                string strPort = System.Configuration.ConfigurationManager.AppSettings["memcachePort"];
                
                if (!MemcachedClient.Exists("gdscache")) { MemcachedClient.Setup("gdscache", new string[] { strHost + ":" + strPort }); }
                MemcachedClient objCache = MemcachedClient.GetInstance("gdscache");
                
                //object obj = objCache.Get("bpl_3930_1984_512_384_WM");
                objCache.Delete(strCacheKey);
            }
            catch (Exception ex) {
                LogMessageToFile("DeletePicFromCache: [" + strCacheKey + "]" + ex.Message, "");
            }
        }
    }
}