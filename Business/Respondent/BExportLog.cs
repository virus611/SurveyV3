using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ORM;
using Library;
using DBHelper;
using System.Data;
using System.Data.SqlClient; 

using Model.ORMModel;

namespace Business.Respondent
{
   public class BExportLog
    {
       public bool addObj(string rid, string start, string end, string acode,string type)
       {

           string dbstr = Tools.GetECConnStr();
           string errorMsg = string.Empty;
           IMapping mapping = new SqlMapping(dbstr);

           ExportLog obj = new ExportLog();
           obj.ID = rid;
           obj.Start = start;
           obj.End = end;
           obj.CreateTime = DateTime.Now;
           obj.ACode = acode;
           obj.State = "正在生成";
           obj.Url = "";
           obj.Type = type;
           obj.Second = "";

           long d;
           int r = mapping.Add<ExportLog>(obj, out d, out errorMsg);
           if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
           {
               return false;
           }
           return true;

       }

       public bool updateState(string rid, string url, string state,string second)
       {
           string dbstr = Tools.GetECConnStr();
           string errorMsg = string.Empty;
           IMapping mapping = new SqlMapping(dbstr);

           ExportLog obj = new ExportLog();
           obj.ID = rid;
           obj = mapping.Query<ExportLog>(obj,  out errorMsg);
           if (string.IsNullOrWhiteSpace(errorMsg) == false || obj == null)
           {
               return false;
           }
           obj.Url = url;
           obj.State = state;
           obj.Second = second;

           int r = mapping.Edit<ExportLog>(obj,   out errorMsg);
           if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
           {
               return false;
           }
           return true;
       }

       public List<ExportLog> getList(string acode,int page, int size)
       {
           string dbstr = Tools.GetECConnStr();
           string errorMsg = string.Empty;
           IMapping mapping = new SqlMapping(dbstr);
 
           List<IDataParameter> plist = new List<IDataParameter>();
           string sqlwhere = "";
           
               sqlwhere += " and ACode_nvarchar=@ACODE";
               plist.Add(new SqlParameter("@ACODE", SqlDbType.NVarChar) { Value = acode });
          
        

           string sql =string.Format( @"with VRecord as (select row_number() over(order by CreateTime_datetime desc) as rNumber,* from S_ExportLog  where 1=1 {0}  ) 
 select * from  VRecord where rNumber BETWEEN @begin AND @end",sqlwhere);
           plist.Add(new SqlParameter("@begin", SqlDbType.Int) { Value = (page - 1) * size + 1 });
           plist.Add(new SqlParameter("@end", SqlDbType.Int) { Value = page * size });


           List<ExportLog> list = mapping.QueryList<ExportLog>(sql, out errorMsg, plist);
           if (string.IsNullOrWhiteSpace(errorMsg) == false )
           {
               return null;
           }
           return list;
       }


       public int getCount(string acode )
       {
           List<IDataParameter> plist = new List<IDataParameter>();
           string sqlwhere = "";
           
               sqlwhere += " and ACode_nvarchar=@ACODE";
               plist.Add(new SqlParameter("@ACODE", SqlDbType.NVarChar) { Value = acode });
           

           string sql = string.Format( @" select  isnull(count(1),0) from S_ExportLog  where 1=1 {0}",sqlwhere);
           int c = new BQuery().queryScalarInt(sql, plist);
           return c;
       }
    }
}
