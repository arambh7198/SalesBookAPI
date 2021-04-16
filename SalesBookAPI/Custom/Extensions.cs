using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace SalesBookAPI.Custom
{
    public static class Extensions
    {
        public static JObject toJObjectWithRelations(this DataSet ds, List<string> TablesToInclude)
        {
            List<Dictionary<string, object>> Rtn = new List<Dictionary<string, object>>();

            Dictionary<string, object> Rtn1 = new Dictionary<string, object>();

            //Loop on Tables
            foreach ( string str in TablesToInclude)
            {
                DataTable dt = ds.Tables[str];
                
                List<Dictionary<string, object>> dataRows = new List<Dictionary<string, object>>();
                dt.Rows.Cast<DataRow>().ToList().ForEach(dataRow =>
                {
                    var row = new Dictionary<string, object>();
                    dt.Columns.Cast<DataColumn>().ToList().ForEach(column =>
                    {
                        row.Add(column.ColumnName, dataRow[column]);
                    });

                    foreach (DataRelation dr in dt.ChildRelations)
                    {
                        var ChildRows = new List<Dictionary<string, object>>();
                        
                        dataRow.GetChildRows(dr).ToList().ForEach(child => {
                            var ChildRow = new Dictionary<string, object>();

                            child.Table.Columns.Cast<DataColumn>().ToList().ForEach(column =>
                            {
                                ChildRow.Add(column.ColumnName, child[column]);
                            });

                            ChildRows.Add(ChildRow);
                        });

                        row.Add(dr.RelationName, ChildRows);
                    }
                    
                    dataRows.Add(row);
                });

                Rtn1.Add(dt.TableName, dataRows);

                var finalTable = new Dictionary<string, object>();
                finalTable.Add(dt.TableName, dataRows);

                Rtn.Add(finalTable);
            }

            return JObject.FromObject(Rtn1);

            //return JArray.FromObject(Rtn);            
        }

        public static JArray ToJArray(this DataTable dt)
        {
            //return new JavaScriptSerializer().Serialize(GetRowToDictionary(dt));

            //Dictionary<string, object> d = new Dictionary<string, object>();

            //d.Add(dt.TableName, GetRowToDictionary(dt));

            var x = GetRowToDictionary(dt);

            return JArray.FromObject(x);

        }

        public static JArray ToJArray(this DataSet ds)
        {            
           List<   Dictionary<string, object> > d = new List<Dictionary<string, object>>();
            foreach (DataTable table in ds.Tables)
            {
                Dictionary<string, object> d1 = new Dictionary<string, object>();

                d1.Add(table.TableName, GetRowToDictionary(table));

                d.Add(d1 );
            }
            
            return JArray.FromObject(d);
        }

        public static string ToJSON(this DataTable dt)
        {
            //return new JavaScriptSerializer().Serialize(GetRowToDictionary(dt));

            Dictionary<string, object> d = new Dictionary<string, object>();

            d.Add(dt.TableName, GetRowToDictionary(dt));

            var serializer = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
            return serializer.Serialize(d);

        }

        private static List<Dictionary<string, object>> GetRowToDictionary(DataTable dt)
        {
            List<Dictionary<string, object>> dataRows = new List<Dictionary<string, object>>();
            dt.Rows.Cast<DataRow>().ToList().ForEach(dataRow =>
            {
                var row = new Dictionary<string, object>();
                dt.Columns.Cast<DataColumn>().ToList().ForEach(column =>
                {
                    row.Add(column.ColumnName, dataRow[column]);
                });
                dataRows.Add(row);
            });
            return dataRows;
        }

        public static string ToJSON(this DataSet data)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            foreach (DataTable table in data.Tables)
            {
                d.Add(table.TableName, GetRowToDictionary(table));
            }
            var serializer = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
            return serializer.Serialize(d);

            //return new JavaScriptSerializer().Serialize(d);
        }
    }
}