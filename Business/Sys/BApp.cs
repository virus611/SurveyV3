using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.ORMModel;

using ORM;
using Library;
using DBHelper;
using System.Data;
using System.Data.SqlClient;
using Business.VO;
namespace Business.Sys
{
    public  class BApp
    {

        public List<AppVO> getList()
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);
            string sql = @"select * from S_AppVersion order by current_int desc";
            List<AppVersion> list = mapping.QueryList<AppVersion>(sql, out errorMsg);
            if (string.IsNullOrWhiteSpace(errorMsg) == false || list == null || list.Count == 0)
            {
                return null;
            }

            List<AppVO> re = new List<AppVO>();
            foreach (AppVersion bean in list)
            {
                AppVO av = new AppVO();
                av.sid = bean.ID;
                av.name = bean.Name;
                av.least = bean.Least.Value;
                av.current = bean.Current.Value;
                av.time = bean.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                av.url = bean.Url;
                re.Add(av);
            }
            return re;
        }

        public bool addObj(string name,int least,int current,string url)
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);

            AppVersion app=new AppVersion();
            app.ID=Tools.createID();
            app.Name=name;
            app.Least=least;
            app.Current=current;
            app.CreateTime=DateTime.Now;
            app.Url=url;

            long d;
            int r = mapping.Add<AppVersion>(app,out d, out errorMsg);
            if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
            { 
                return false;
            } 
            return true;
        }

        public bool delObj(string sid)
        {
           
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);

            List<IDataParameter> plist = new List<IDataParameter>();
            plist.Add(new SqlParameter("@ID", SqlDbType.NVarChar) { Value = sid });
            string sql = "delete from S_AppVersion where ID_nvarchar=@ID";


            using (IDataBase db = new SqlDBHelper(dbstr, out errorMsg))
            {
                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                    return false;
                db.OpenConn(out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                    return false;

                db.AddParameters(plist, true);
                int r = db.RunSqlNonQuery(sql, out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public AppVO getLastObj()
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);
            
            string sql = @"select top 1 * from S_AppVersion order by current_int desc";
            AppVersion bean = mapping.Query<AppVersion>(sql, out errorMsg);
            if (string.IsNullOrWhiteSpace(errorMsg) == false || bean == null)
            {
                return null;
            }
            AppVO av = new AppVO();
            av.sid = bean.ID;
            av.name = bean.Name;
            av.least = bean.Least.Value;
            av.current = bean.Current.Value;
            av.time = bean.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
            av.url = bean.Url;
            return av;
        }
    }
}
