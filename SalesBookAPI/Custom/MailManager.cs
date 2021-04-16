using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Web.Configuration;
using System.Text;
using System.Net.Mime;
using SalesBookAPI.Custom;

namespace SalesBookAPI.Custom
{
    public static class MailManager
    {
        #region "Smtp Client/From Address"
        public static SmtpClient getSmtpClient()
        {
            string strSMtpHost = SiteConfig.SMTPServer;
            string strPort = SiteConfig.SMTPPORT;
            string SSL = SiteConfig.SMTPSSL;
            string UserName = SiteConfig.SMTPFromEmail;
            string Pwd = SiteConfig.SMTPPassword;


            SmtpClient smtp = new SmtpClient(strSMtpHost);

            if (UserName != "")
            {
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential(UserName, Pwd);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = NetworkCred;
            }
            if (SSL == "1")
            {
                smtp.EnableSsl = true;
            }
            else
            {
                smtp.EnableSsl = false;
            }
            if (strPort != "")
            {
                smtp.Port = Convert.ToInt16(strPort);
            }

            return smtp;
            
        }
        #endregion


        #region get Mail Address
        public static MailAddress getFromMailAddress()
        {
            string strEmail = SiteConfig.SMTPFromEmail;
            string strEmailName = SiteConfig.SMTPFromEmailName;
            MailAddress msg = new MailAddress(strEmail, strEmailName);

            return msg;
        }
        #endregion


        #region getMail Message 3 overloads
        private static MailMessage getMailMessage(string strTO, string strSubject, bool IsBodyHtml)
        {
            MailMessage msg = new MailMessage(new MailAddress(SiteConfig.SMTPFromEmail, SiteConfig.SMTPFromEmailName), new MailAddress(strTO));
            msg.IsBodyHtml = IsBodyHtml;
            msg.Subject = strSubject;
            return msg;
        }

        private static MailMessage getMailMessage(string strTO, string strSubject, string strBody, bool IsBodyHtml)
        {
            MailMessage msg = new MailMessage(new MailAddress(SiteConfig.SMTPFromEmail, SiteConfig.SMTPFromEmailName), new MailAddress(strTO));
            msg.IsBodyHtml = IsBodyHtml;
            msg.Body = strBody;
            msg.Subject = strSubject;
            return msg;
        }

        //private static MailMessage getMailMessage(string strTO, string CC, string strSubject, string strBody, bool IsBodyHtml, List<string> Attachments = null)
        //{
        //    MailMessage msg;
        //    if (strTO.Contains(","))
        //    {
        //        string[] strTOs = strTO.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

        //        msg = new MailMessage(new MailAddress(SiteConfig.SMTPFromEmail, SiteConfig.SMTPFromEmailName), new MailAddress(strTOs[0]));

        //        for(int i=1;i<strTOs.Length;i++)
        //        {
        //            msg.To.Add(strTOs[i].Trim());
        //        }
        //    }
        //    else
        //    {
        //        msg = new MailMessage(new MailAddress(SiteConfig.SMTPFromEmail, SiteConfig.SMTPFromEmailName), new MailAddress(strTO));
        //    }
        //    msg.IsBodyHtml = IsBodyHtml;
        //    msg.Subject = strSubject;
        //    msg.Body = strBody;
        //    if (CC.Trim() != "")
        //    {
        //        string[] CCID = CC.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

        //        foreach (string str in CCID)
        //        {
        //            if (str == "") continue;
        //            msg.CC.Add(str.Trim());
        //        }
        //    }
        //    if(Attachments!=null)
        //    {
        //        foreach(string str in Attachments)
        //        {
        //            msg.Attachments.Add(new Attachment(str));
        //        }
        //    }

        //    return msg;
        //}

        private static MailMessage getMailMessage(string strTO, string CC, string strSubject, string strBody, bool IsBodyHtml, Dictionary<string, string> Attachments = null)
        {
            MailMessage msg;
            if (strTO.Contains(","))
            {
                string[] strTOs = strTO.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                msg = new MailMessage(new MailAddress(SiteConfig.SMTPFromEmail, SiteConfig.SMTPFromEmailName), new MailAddress(strTOs[0]));

                for (int i = 1; i < strTOs.Length; i++)
                {
                    msg.To.Add(strTOs[i].Trim());
                }
            }
            else
            {
                msg = new MailMessage(new MailAddress(SiteConfig.SMTPFromEmail, SiteConfig.SMTPFromEmailName), new MailAddress(strTO));
            }
            msg.IsBodyHtml = IsBodyHtml;
            msg.Subject = strSubject;
            msg.Body = strBody;
            if (CC.Trim() != "")
            {
                string[] CCID = CC.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string str in CCID)
                {
                    if (str == "") continue;
                    msg.CC.Add(str.Trim());
                }
            }
        
            
            return msg;
        }

        private static MailMessage getMailMessage(string strTO, string CC, string strSubject, string strBody, bool IsBodyHtml,
            string ActivationKey, string CustCode,
            Dictionary<string, string> Attachments = null)
        {
            MailMessage msg;
            if (strTO.Contains(","))
            {
                string[] strTOs = strTO.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                msg = new MailMessage(new MailAddress(SiteConfig.SMTPFromEmail, SiteConfig.SMTPFromEmailName), new MailAddress(strTOs[0]));

                for (int i = 1; i < strTOs.Length; i++)
                {
                    msg.To.Add(strTOs[i].Trim());
                }
            }
            else
            {
                msg = new MailMessage(new MailAddress(SiteConfig.SMTPFromEmail, SiteConfig.SMTPFromEmailName), new MailAddress(strTO));
            }
            msg.IsBodyHtml = IsBodyHtml;
            msg.Subject = strSubject;
            msg.Body = strBody;
            if (CC.Trim() != "")
            {
                string[] CCID = CC.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string str in CCID)
                {
                    if (str == "") continue;
                    msg.CC.Add(str.Trim());
                }
            }
            if (Attachments != null)
            {
                foreach (var item in Attachments)
                {
                    Attachment attachment = new Attachment(item.Value);
                    if (item.Key != null && item.Key != "")
                    {
                        attachment.Name = item.Key;
                    }
                    msg.Attachments.Add(attachment);
                }
            }
            
            return msg;
        }

        #endregion


        #region SendMail 3 overloads
        public static void SendMail(string strTO, string strSubject, bool IsBodyHtml)
        {
            try
            {
                SendMail(getSmtpClient(), getMailMessage(strTO, strSubject, IsBodyHtml));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void SendMail(string strTO, string strSubject, string strBody, bool IsBodyHtml = true)
        {
            try
            {
                SendMail(getSmtpClient(), getMailMessage(strTO, strSubject, strBody, IsBodyHtml));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void SendMail(string strTO, string CC, string strSubject, string strBody, bool IsBodyHtml, Dictionary<string, string> Attachments = null)
        {
            try
            {
                SendMail(getSmtpClient(), getMailMessage(strTO, CC, strSubject, strBody, IsBodyHtml, Attachments));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SendForMail(string strTO, string CC, string strSubject, string strBody, bool IsBodyHtml, string ActivationKey, string CustCode, Dictionary<string, string> Attachments = null)
        {
            try
            {
                SendMail(getSmtpClient(), getMailMessage(strTO, CC, strSubject, strBody, IsBodyHtml, ActivationKey, CustCode, Attachments));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void SendMail(SmtpClient smtp, MailMessage msg)
        {
            try
            {
                smtp.Send(msg);
            }
            catch (SmtpFailedRecipientException smex)
            {
                // StaticMethods.LogException("MailManager", "SendMail", msg.ToString(), smex.ToString());
                //smex.StatusCode =   SmtpStatusCode.
                //smex.
                // DAL.logException(smex, "SendMail");
                throw smex;
            }
            catch (SmtpException smex)
            {
                //   StaticMethods.LogException("MailManager", "SendMail", msg.ToString(), smex.ToString());
                //  DAL.logException(smex, "SendMail");
                throw smex;
            }
            catch (Exception ex)
            {
                //   DAL.logException(ex, "SendMail");
                // StaticMethods.LogException("MailManager", "SendMail", msg.ToString(), ex.ToString());
                throw ex;
            }
        }
        #endregion

    }
}