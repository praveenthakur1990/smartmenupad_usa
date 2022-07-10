using SmartMenu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;
using static SmartMenu.DAL.Enums.EnumHelper;

namespace SmartMenu.DAL.Common
{
    public static class CommonManager
    {
        public static List<TenantsVM> getTenantConnection(string userId, string email)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                List<TenantsVM> objList = new List<TenantsVM>();
                TenantsVM objTenants = null;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPGetTenantConnection] @UserId, @Email", connection))
                    {
                        command.Parameters.AddWithValue("@UserId", (userId == null ? string.Empty : userId));
                        command.Parameters.AddWithValue("@Email", (email == null ? string.Empty : email));
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                objTenants = new TenantsVM();
                                objTenants.Id = Convert.ToInt32(reader["Id"]);
                                objTenants.UserId = reader["UserId"].ToString();
                                objTenants.TenantName = reader["TenantName"].ToString();
                                objTenants.TenantSchema = reader["TenantSchema"].ToString();
                                objTenants.TenantConnection = reader["TenantConnection"].ToString();
                                objTenants.TenantDomain = reader["TenantDomain"].ToString();
                                objTenants.CreatedBy = reader["CreatedBy"] is DBNull ? string.Empty : reader["CreatedBy"].ToString();
                                objTenants.RoleName = reader["RoleName"] is DBNull ? string.Empty : reader["RoleName"].ToString();
                                objTenants.AddedByName = reader["AddedByName"] is DBNull ? string.Empty : reader["AddedByName"].ToString();
                                objList.Add(objTenants);
                            }
                        }
                    }
                    connection.Close();
                    return objList;
                }
                catch (Exception ex)
                {
                    return new List<TenantsVM>();
                }
            }
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

        public static int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static List<ItemQty> GetItemQtyList()
        {
            List<ItemQty> objList = new List<ItemQty>();
            for (int i = 0; i < 100; i++)
            {
                if (i == 0)
                {
                    ItemQty obj = new ItemQty();
                    obj.Name = "Remove";
                    obj.Value = "0";
                    objList.Add(obj);
                }
                else
                {
                    ItemQty obj = new ItemQty();
                    obj.Name = i.ToString();
                    obj.Value = i.ToString();
                    objList.Add(obj);
                }

            }
            return objList;
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

            //System.Diagnostics.Debug.WriteLine(string.Format(msg, json));
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

        public static string CreatePathIfMissing(string path)
        {
            bool folderExists = System.IO.Directory.Exists(path);
            if (!folderExists)
                System.IO.Directory.CreateDirectory(path);
            return path;
        }


    }

    public class ItemQty
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
