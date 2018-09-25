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


namespace Business.User
{
    public class BAreaManager
    {
        public AreaManager getAreaManager(string username, string pwd)
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);

            string sql = @"select top 1 * from U_AreaManager  where Name_nvarchar = @Name";
            List<IDataParameter> plist = new List<IDataParameter>();
            plist.Add(new SqlParameter("@Name", SqlDbType.NVarChar) { Value = username });

            AreaManager obj = mapping.Query<AreaManager>(sql, out errorMsg, plist);
            if (string.IsNullOrWhiteSpace(errorMsg)==false || obj==null)
            {
                return null;
            }
            if (pwd != obj.Pwd)
            {
                return null;
            }
            return obj;

        }

        public List<AreaManagerVO> getList()
        {
          
            string sql = @"select m.SID_nvarchar as [sid],m.Name_nvarchar as [name],m.Pwd_nvarchar as [pwd],s.Name_nvarchar as [code] from U_AreaManager m,S_Area s where m.ACode_nvarchar=s.Code_nvarchar ";
            List<AreaManagerVO> re = new List<AreaManagerVO>();
            DataTable dt = new BQuery().getDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    AreaManagerVO vo = new AreaManagerVO();
                    vo.sid = (string)dt.Rows[i]["sid"];
                    vo.name = (string)dt.Rows[i]["name"];
                    vo.code = (string)dt.Rows[i]["code"];
                    vo.pwd = (string)dt.Rows[i]["pwd"];

                    re.Add(vo);
                }
            }
            return re;
        }


        public  AreaManagerVO  getObj(string sid)
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);

            AreaManager obj=new AreaManager();
            obj.SID=sid;
            obj = mapping.Query<AreaManager>(obj, out errorMsg);
             
            if (string.IsNullOrWhiteSpace(errorMsg) == false || obj == null)
            {
                return null;
            }

            AreaManagerVO vo = new AreaManagerVO();
            vo.sid = obj.SID;
            vo.name = obj.Name;
            vo.code = obj.ACode;
            vo.pwd = obj.Pwd;

            return vo; 
        }

        public bool saveAreaManager(string sid, string name, string pwd, string code)
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);

            bool isAdd = false;

            AreaManager obj = new AreaManager();
            obj.Name = name;
            obj.Pwd = pwd;
            obj.ACode = code;

            if (string.IsNullOrWhiteSpace(sid))
            {
                isAdd = true;
                obj.SID = Tools.createID();
            }
            else
            {
                obj.SID = sid;
            }

            int r = 0;
            if (isAdd)
            {
                long d;
                r = mapping.Add<AreaManager>(obj, out d, out errorMsg);
            }
            else
            {
                r = mapping.Edit<AreaManager>(obj,  out errorMsg);
            }
           
            if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
            { 
                return false;
            }
            return true;

        }

        public bool delAreaManager(string sid)
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            List<IDataParameter> plist = new List<IDataParameter>();
            plist.Add(new SqlParameter("@SID", SqlDbType.NVarChar) { Value = sid });
            string sql = "delete from U_AreaManager where SID_nvarchar=@SID";

            bool flag = new BQuery().executeSQL(sql, plist);
            return flag;
        }
    }
}
