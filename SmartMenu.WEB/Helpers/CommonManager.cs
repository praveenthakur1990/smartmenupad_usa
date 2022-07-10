using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;
using SmartMenu.DAL.Models;
using static SmartMenu.DAL.Enums.EnumHelper;

namespace SmartMenu.WEB.Helpers
{
    public static class CommonManager
    {
        public static List<StateModel> getStateList()
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.Load(HttpContext.Current.Server.MapPath("~/Helpers/xml/states.xml"));
            List<StateModel> objList = new List<DAL.Models.StateModel>();
            System.Xml.XmlNode idNodes = doc.SelectSingleNode("states");
            StateModel obj = null;
            foreach (System.Xml.XmlNode node1 in idNodes.ChildNodes)
            {
                obj = new StateModel();
                obj.Text = node1.Attributes["name"].InnerText;
                obj.Value = node1.Attributes["abbreviation"].InnerText;
                objList.Add(obj);
            }

            return objList;
        }

        public static System.Net.Http.HttpContent CreateHttpContent(object content)
        {
            System.Net.Http.HttpContent httpContent = null;

            if (content != null)
            {
                var ms = new System.IO.MemoryStream();
                SerializeJsonIntoStream(content, ms);
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                httpContent = new System.Net.Http.StreamContent(ms);
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            }

            return httpContent;
        }

        public static void SerializeJsonIntoStream(object value, System.IO.Stream stream)
        {
            using (var sw = new System.IO.StreamWriter(stream, new System.Text.UTF8Encoding(false), 1024, true))
            using (var jtw = new Newtonsoft.Json.JsonTextWriter(sw) { Formatting = Newtonsoft.Json.Formatting.None })
            {
                var js = new Newtonsoft.Json.JsonSerializer();
                js.Serialize(jtw, value);
                jtw.Flush();
            }
        }

        public static string CreatePathIfMissing(string path)
        {
            bool folderExists = System.IO.Directory.Exists(path);
            if (!folderExists)
                System.IO.Directory.CreateDirectory(path);
            return path;
        }

        public static string CreateUniqueFileName(string type)
        {
            return string.Format(@"{0}", DateTime.Now.Ticks + type);
        }

        public static bool IsBase64(this string base64String)
        {
            if (string.IsNullOrEmpty(base64String) || base64String.Length % 4 != 0
               || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
                return false;
            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch (Exception exception)
            {
                // Handle the exception
            }
            return false;
        }
        public static bool IsPhoto(string fileName)
        {
            var list = GetAllPhotosExtensions();
            var filename = fileName.ToLower();
            bool isThere = false;
            foreach (var item in list)
            {
                if (filename.EndsWith(item))
                {
                    isThere = true;
                    break;
                }
            }
            return isThere;
        }

        public static List<string> GetAllPhotosExtensions()
        {
            var list = new List<string>();
            list.Add(".jpg");
            list.Add(".png");
            list.Add(".jpeg");
            return list;
        }

        public static void LogError(MethodBase method, Exception ex, params object[] values)
        {
            string appPhysicalPath = ConfigurationManager.AppSettings["APPPhysicalPath"].ToString();
            ParameterInfo[] parms = method.GetParameters();
            object[] namevalues = new object[2 * parms.Length];
            string msg = "Error in " + method.Name + "(";
            for (int i = 0, j = 0; i < parms.Length; i++, j += 2)
            {
                msg += "{" + j + "}={" + (j + 1) + "}, ";
                namevalues[j] = parms[i].Name;
                if (i < (values == null ? 0 : values.Length)) namevalues[j + 1] = values[i];
            }
            msg += "exception=" + (ex == null ? string.Empty : ex.Message) + ")";
            var paramJson = values == null ? "--" : new JavaScriptSerializer().Serialize(values.FirstOrDefault());
            try
            {
                string fileUploadPath = DirectoryPathEnum.ErrorLogs.ToString() + "/";
                string directoryPath = CreatePathIfMissing(appPhysicalPath + "/" + fileUploadPath);
                string filepath = directoryPath + DateTime.Today.ToString("dd-MM-yy") + ".txt";
                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Dispose();
                }
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    string error = "Log Written Date:" + " " + DateTime.Now.ToString();
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(error);
                    sw.WriteLine(msg.Replace("{0}", "params").Replace("{1}", paramJson));

                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }
    }
}