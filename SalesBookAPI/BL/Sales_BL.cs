using Microsoft.Reporting.WebForms;
using Newtonsoft.Json.Linq;
using SalesBookAPI.Custom;
using SalesBookAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace SalesBookAPI.BL
{
    public class Sales_BL
    {
        IFormatProvider culter = new CultureInfo("en-US");
        string strDateFormat = SiteConfig.DateFormat();
        public string getUserReportPDF(JObject data, Token t)
        {

            try
            {
                string filePath = "";
                string filename = "";

                filename = "SalesInvoice" + "_" + data["InvoiceNo"].ToString().Replace("/", "_") + "_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".pdf";


                if (data["Code"] != null && data["Code"].ToString() != "")
                {
                    string strSubCat = data["Code"].ToString().Replace("[", "").Replace("]", "").Replace(" ", "");
                    strSubCat = strSubCat.Replace("\r\n", "");
                    data["Code"] = strSubCat.Replace("\"", "");
                }
                else
                {
                    data["Code"] = "0";
                }

                Dictionary<string, object> IncludeParam = new Dictionary<string, object>();
                IncludeParam.Add("Code", data["Code"].ToString());

                DataTable dtData = StaticGeneral.GetDataTable("pRpt_Sales", IncludeParam, true, 120);

                if (dtData.Rows.Count == 0 || dtData == null)
                {
                    return "0";
                }

                Warning[] warnings;
                string[] streamids;
                string mimeType, encoding, filenameExtension;

                byte[] array;

                ReportParameter Rp1 = new ReportParameter("Code", data["Code"].ToString());

                using (ReportViewer rptvw = new ReportViewer())
                {
                    rptvw.LocalReport.EnableExternalImages = true;

                    rptvw.ProcessingMode = ProcessingMode.Local;
                    rptvw.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/Reports/SalesBookInvoice.rdlc");
                    rptvw.LocalReport.DataSources.Clear();

                    rptvw.LocalReport.SetParameters(Rp1);

                    ReportDataSource rds = new ReportDataSource("DataSet1", dtData);
                    rptvw.LocalReport.DataSources.Add(rds);

                    array = rptvw.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

                }

                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = string.Format(filename),
                    Inline = true,
                };

                var result = new HttpResponseMessage(HttpStatusCode.OK);
                Stream stream = new MemoryStream(array);
                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/excel");


                //-------Save file 

                string SubDir = HttpContext.Current.Server.MapPath("~/download");

                if (!Directory.Exists(SubDir))
                {
                    Directory.CreateDirectory(SubDir);
                }

                //FileStream fstream = File.Create(HttpContext.Current.Server.MapPath("~/DownLoad/" + filePath), array.Length);
                FileStream fstream = File.Create(SubDir + "/" + filename, array.Length);
                fstream.Write(array, 0, array.Length);
                fstream.Close();

                filePath = filename;
                //-------End of Save file

                result.Dispose();
                return filePath;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string getUserReportExcel(JObject data, Token t)
        {

            try
            {
                string filePath = "";
                string filename = "";

                filename = "SalesInvoice" + "_" + data["InvoiceNo"].ToString().Replace("/", "_") + "_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

                if (data["Code"] != null && data["Code"].ToString() != "")
                {
                    string strSubCat = data["Code"].ToString().Replace("[", "").Replace("]", "").Replace(" ", "");
                    strSubCat = strSubCat.Replace("\r\n", "");
                    data["Code"] = strSubCat.Replace("\"", "");
                }
                else
                {
                    data["Code"] = "0";
                }

                Dictionary<string, object> IncludeParam = new Dictionary<string, object>();
                IncludeParam.Add("Code", data["Code"].ToString());

                DataTable dtData = StaticGeneral.GetDataTable("pRpt_Sales", IncludeParam, true, 120);

                if (dtData.Rows.Count == 0 || dtData == null)
                {
                    return "0";
                }

                Warning[] warnings;
                string[] streamids;
                string mimeType, encoding, filenameExtension;

                byte[] array;

                ReportParameter Rp1 = new ReportParameter("Code", data["Code"].ToString());

                using (ReportViewer rptvw = new ReportViewer())
                {
                    rptvw.ProcessingMode = ProcessingMode.Local;
                    rptvw.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/Reports/SalesBookInvoice.rdlc");
                    rptvw.LocalReport.DataSources.Clear();

                    rptvw.LocalReport.SetParameters(Rp1);

                    ReportDataSource rds = new ReportDataSource("DataSet1", dtData);
                    rptvw.LocalReport.DataSources.Add(rds);

                    array = rptvw.LocalReport.Render("Excel", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

                }

                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = string.Format(filename),
                    Inline = true,
                };

                var result = new HttpResponseMessage(HttpStatusCode.OK);
                Stream stream = new MemoryStream(array);
                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");
                //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/excel");


                //-------Save file 

                string SubDir = HttpContext.Current.Server.MapPath("~/download");

                if (!Directory.Exists(SubDir))
                {
                    Directory.CreateDirectory(SubDir);
                }

                //FileStream fstream = File.Create(HttpContext.Current.Server.MapPath("~/DownLoad/" + filePath), array.Length);
                FileStream fstream = File.Create(SubDir + "/" + filename, array.Length);
                fstream.Write(array, 0, array.Length);
                fstream.Close();

                filePath = filename;
                //-------End of Save file

                return filePath;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}