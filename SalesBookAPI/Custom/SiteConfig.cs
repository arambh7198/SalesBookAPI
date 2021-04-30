using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace SalesBookAPI.Custom
{
    public class SiteConfig
    {
        public static string StyleImagePath = "";
        public static void SetStartupValues()
        {
            DataTable dt = StaticGeneral.GetDataTable("Select * from mindcubesys.dbo.[$CompanyConfig] where CompanyCode = '" +
                MainCompanyCode
                + "' and ConfigName='StyleImagePath'");
            if (dt.Rows.Count > 0)
            {
                StyleImagePath = dt.Rows[0]["ConfigValue"].ToString();
            }

        }
        public static string SiteRootURL
        {
            get
            {
                return ConfigurationManager.AppSettings["SiteRootURL"].ToString();
            }
        }
        public static bool WebSiteIsLive
        {
            get
            {
                string str = ConfigurationManager.AppSettings["WebSiteIsLive"].ToString().ToLower();
                if (str == "yes" || str == "y")
                {
                    return true;
                }
                return false;
            }
        }

        public static string EncryptKey
        {
            get
            {
                return ConfigurationManager.AppSettings["EncryptKey"].ToString();
            }
        }
        public static string AuthServiceHeaderName
        {
            get
            {
                return ConfigurationManager.AppSettings["AuthServiceHeaderName"].ToString();
            }
        }


        public static string LoginKeyName
        {
            get
            {
                return ConfigurationManager.AppSettings["LoginKeyName"].ToString();
            }
        }
        public static string WebSessionID
        {
            get
            {
                return ConfigurationManager.AppSettings["WebSessionID"].ToString();
            }
        }
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            }
        }

        public static string AkashgangaOfficeDBConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["AkashgangaOfficeDBConnection"].ConnectionString;
            }
        }

        public static string PricingServiceURL
        {
            get
            {
                return ConfigurationManager.AppSettings["PricingServiceURL"].ToString();
            }
        }
        public static string AasanProfilePicImgPath
        {
            get
            {
                return ConfigurationManager.AppSettings["AasanProfilePicImgPath"].ToString();
            }
        }

        public static string AasanProfilePicImgFilePath
        {
            get
            {
                return ConfigurationManager.AppSettings["AasanProfilePicImgFilePath"].ToString();
            }
        }

        public static string MainCompanyCode
        {
            get
            {
                return ConfigurationManager.AppSettings["MainCompanyCode"].ToString();
            }
        }
        public static string endPointAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["endPointAddress"].ToString();
            }
        }
        public static string DateFormat()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["DateFormat"]);
        }
        public static string AuthorisedSignFilePath()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["AuthorisedSignFilePath"]);
        }
        public static string CompanyLogoFilePath()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["CompanyLogoFilePath"]);
        }


        #region Message Key
        public static string SMSFeedId
        {
            get
            {
                return ConfigurationManager.AppSettings["SMSFeedId"].ToString();
            }
        }
        public static string SMSSenderID
        {
            get
            {
                return ConfigurationManager.AppSettings["SMSSenderID"].ToString();
            }
        }
        public static string SMSUserName
        {
            get
            {
                return ConfigurationManager.AppSettings["SMSUserName"].ToString();
            }
        }
        public static string SMSPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["SMSPassword"].ToString();
            }
        }
        public static string SMSURL
        {
            get
            {
                return ConfigurationManager.AppSettings["SMSURL"].ToString();
            }
        }
        #endregion

        #region "SMTP Related"
        public static string SMTPServer
        {
            get
            {
                return ConfigurationManager.AppSettings["SMTPHost"].ToString();
            }
        }

        public static string SMTPPORT
        {
            get
            {
                return ConfigurationManager.AppSettings["SMTPPORT"].ToString();
            }
        }

        public static string SMTPSSL
        {
            get
            {
                return ConfigurationManager.AppSettings["SMTPSSL"];
            }
        }

        public static string SMTPPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["SMTPPassword"];
            }
        }

        public static string SMTPFromEmail
        {
            get
            {
                return ConfigurationManager.AppSettings["SMTPFromEmail"];
            }
        }

        public static string SMTPFromEmailName
        {
            get
            {
                return ConfigurationManager.AppSettings["SMTPFromEmailName"];
            }
        }

        public static int SaveQueryTimeOut
        {
            get
            {
                return Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SaveQueryTimeOut"]);
            }
        }
        #endregion

        public static string getServerPath
        {
            get
            {
                return System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            }
        }

        public static string getStaticColumnList
        {
            get
            {
                return "AUTOID,ENTRYCENTERCODE,FINANCIALYEAR,TRANSACTIONTYPE,CRDRENTRY,MEMBERCODE,ACCOUNTNO,CATEGORY,VOUCHERDATE,VOUCHERNO,SESSION,SAMPLENO,PRODUCTCODE,LEDGERCODE,CRDRNO,QTY,RATE,LEDGERAMOUNT,IO,FAT,SNF,LR,CLR,WATER,KGRATE,KGFAT,KGSNF,ISCARDAUTO,ISFATAUTO,ISWEIGHTAUTO,CANNO,ENTRYUSERID,MILKENTRYTYPE,AUTOWEIGHT,AUTOFAT,COLLECTIONTIME,ISLOAN,MILKAMOUNT,MILKSLIPOPEN,MILKSLIPCLOSE,OLDWEIGHT,OLDFAT,OLDSNF,OLDAMOUNT,ISADDEDWATER,EFFECTIVEDATE,LOANAMOUNT,ICODE,BANKCODE,ISAUTOENTRY,GROSSAMOUNT,TAXPERCENT,TAXAMOUNT,TPAYAMOUNT,COMPANYCODE,REMARK,LASTUPDATE,NODISPLAY,ISDPU,DPUID,ISMANUALRATE,ISNOTIFICATIONSENT,ISSMSSENT,SYNCDATETIME,IMPORTDATETIME,ISDELETE,ISEDIT,PROTEIN,MILKQUALITYREMARK,AUTOSNF,ISABNORMALSAMPLE,PENALTYAMOUNT,ISUPDATE,LOCATIONCODE,TEMPERATURE,FREEZINGPOINT,PROTEINLEVEL,ADDEDWATER,SUGAR,BICARBONATE,ADULTERATION";
            }
        }

        public static string getStaticLocalSalesColumnList
        {
            get
            {
                return "AUTOID,LOCATIONCODE,COMPANYCODE,ENTRYDATE,SESSION,CATEGORY,LASTCANNO,LASTCANLITER,LOCALENTRYUSERID,LOCALENTRYTIME,LOCALWEIGHT,LOCALRATE,LOCALAMOUNT,ISLOCALSALEBYCOUPON,SALEGOOD1WEIGHT,SALEGOOD1FAT,SALEGOOD1SNF,SALEGOOD1KGFAT,SALEGOOD1RATE,SALEGOOD1AMOUNT,SALEGOOD2WEIGHT,SALEGOOD2FAT,SALEGOOD2SNF,SALEGOOD2KGFAT,SALEGOOD2RATE,SALEGOOD2AMOUNT,SALESOURWEIGHT,SALESOURFAT,SALESOURSNF,SALESOURKGFAT,SALESOURRATE,SALESOURAMOUNT,SALECURDWEIGHT,SALECURDFAT,SALECURDSNF,SALECURDKGFAT,SALECURDRATE,SALECURDAMOUNT,SALEENTRYTIME,SALEENTRYUSERID,DAIRYAMOUNT,DAIRYKGFATRATE,DAIRYRATE,EXPECTEDDAIRYWEIGHT,EXPECTEDDAIRYAMOUNT,EXPECTEDDAIRYKGFATRATE,EXPECTEDDAIRYRATE,EXPECTEDPROFITAMOUNT,PURCHASEGOODRATE1,PURCHASEGOODRATE2,PURCHASEGOODRATE3,PURCHASECOMMISSION,AUTOWEIGHTPERCENT,AUTOFATPERCENT,MANUALWEIGHTPERCENT,MANUALFATPERCENT,TOTALSAMPLE,TOTALCAN,ISTRUCKSLIPENTRY,MIXEDAUTOID,BILLFROMDATE,BILLTODATE,ENTRYUSERID,DISPATCHVOUCHERNO,DISPATCHNOTECAN,DISPATCHNOTEWEIGHT,DISPATCHNOTEFAT,DISPATCHNOTESNF,DISPATCHNOTEENTRYUSERID,DISPATCHNOTEENTRYTIME,ISMANUALDISPATCHNOTE,ISMIXAMOUNT,NOTES,REMARK,LASTUPDATE,NODISPLAY,SALEGOOD1CAN,SALEGOOD2CAN,SALESOURCAN,SALECURDCAN,SYNCDATETIME,MILKSLIPMESSAGE,ISDELETE,ISEDIT,MILKDISPATCHAUTOID,PURCHASEMILKMEMBERS,PURCHASEMILKWEIGHT,PURCHASEMILKFAT,PURCHASEMILKSNF,PURCHASEMILKAMOUNT,PURCHASETEMPERATURE,PURCHASEFREEZINGPOINT,PURCHASEPROTEINLEVEL,PURCHASEADDEDWATER,PURCHASESUGAR,DISPATCHTEMPERATURE,DISPATCHFREEZINGPOINT,DISPATCHPROTEINLEVEL,DISPATCHADDEDWATER,DISPATCHSUGAR,PURCHASEBICARBONATE,PURCHASEADULTERATION,DISPATCHBICARBONATE,DISPATCHADULTERATION";
            }
        }

        public static string getStaticPayDeductionColumnList
        {
            get
            {
                return "AUTOID,LOCATIONCODE,MEMBERCODE,VOUCHERDATE,VOUCHERNO,DEDUCTIONTYPE,QTY,RATE,AMOUNT";
            }
        }

    }
}