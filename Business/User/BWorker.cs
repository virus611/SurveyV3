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
using Business.Sys;

namespace Business.User
{
    public class BWorker
    {
        public List<WorkerVO> getList(string acode)
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);

            string sql = @"select   * from U_Worker  where ACode_nvarchar = @ACODE";
            List<IDataParameter> plist = new List<IDataParameter>();
            plist.Add(new SqlParameter("@ACODE", SqlDbType.NVarChar) { Value = acode });

            List<Worker> list = mapping.QueryList<Worker>(sql, out errorMsg, plist);
            if (string.IsNullOrWhiteSpace(errorMsg) == false || list ==null || list.Count==0)
            {
                return null;
            }

            List<WorkerVO> re = new List<WorkerVO>();
            foreach (Worker bean in list)
            {
                WorkerVO vo = new WorkerVO();
                vo.sid=bean.SID;
                vo.sname=bean.Name;
                vo.pwd=bean.Pwd;
                vo.vname=bean.VNames;
                 
                re.Add(vo);
            }

            return re;
        }

        public  WorkerVO  getObjByName(string name)
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);

            string sql = @"select   * from U_Worker  where Name_nvarchar = @Name";
            List<IDataParameter> plist = new List<IDataParameter>();
            plist.Add(new SqlParameter("@Name", SqlDbType.NVarChar) { Value = name });

            Worker bean= mapping.Query<Worker>(sql, out errorMsg, plist);
            if (string.IsNullOrWhiteSpace(errorMsg) == false || bean == null  )
            {
                return null;
            }

           
                WorkerVO vo = new WorkerVO();
                vo.sid = bean.SID;
                vo.sname = bean.Name;
                vo.pwd = bean.Pwd;
                vo.vname = bean.VNames;
                vo.vids = bean.VIDS;
                return vo;
        }

        public WorkerVO getWorker(string sid)
        {
              string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);

            Worker obj = new Worker();
            obj.SID = sid;
            obj = mapping.Query<Worker>(obj, out errorMsg);
            if (string.IsNullOrWhiteSpace(errorMsg) == false || obj == null  )
            {
                return null;
            }

            WorkerVO re = new WorkerVO();
            re.sid = sid;
            re.sname=obj.Name;
            re.pwd=obj.Pwd;
            re.acode=obj.ACode; 
            return re;
        }

        public string saveWorker(string sid, string sname, string pwd, string acode)
        {

            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);

            if (string.IsNullOrWhiteSpace(sid)==true)
            {
                //新增帐号
                string sql = @"select  isnull(count(1),0)  from U_Worker  where Name_nvarchar = @Name";
                List<IDataParameter> plist = new List<IDataParameter>();
                plist.Add(new SqlParameter("@Name", SqlDbType.NVarChar) { Value = sname });

                int cnt = new BQuery().queryScalarInt(sql, plist);
                if (cnt > 0)
                {
                    return "登录帐号已存在，请使用其它的帐号";
                }
            }
 
            Worker obj = new Worker();
            obj.Name = sname;
            obj.Pwd = pwd;

            bool isAdd = false;
            if (string.IsNullOrWhiteSpace(sid))
            {
                obj.ACode = acode;
                obj.SID = Tools.createID();
                isAdd = true;
            }
            else
            {
                obj.SID = sid;
                isAdd = false;
            }

            int r = 0;
            if (isAdd)
            {
                long d;
                r = mapping.Add<Worker>(obj, out d, out errorMsg);
            }
            else
            {
                r = mapping.Edit<Worker>(obj, out errorMsg);
            }

            if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
            {
                return "保存失败";
            }
            return "";
             
        }


        public bool delWorker(string sid)
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            List<IDataParameter> plist = new List<IDataParameter>();
            plist.Add(new SqlParameter("@SID", SqlDbType.NVarChar) { Value = sid });
            string sql = "delete from U_Worker where SID_nvarchar=@SID";

            bool flag = new BQuery().executeSQL(sql, plist);
            return flag;
        }

        public bool updateVName(string sid, string ids, string names)
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);

            Worker obj = new Worker();
            obj.SID = sid;
            obj = mapping.Query<Worker>(obj, out errorMsg);
            if (string.IsNullOrWhiteSpace(errorMsg) == false || obj == null)
            {
                return false;
            }

            obj.VIDS = ids;
            obj.VNames = names;

            int r = mapping.Edit<Worker>(obj, out errorMsg);
            if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
            {
                return false;
            }
            return true;
        }

        public List<CheckTreeVO> getTreeList(string sid, string acode)
        {
            List<CheckTreeVO> re = new List<CheckTreeVO>();
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);

            //1.先找更新的对象
               Worker wobj = new Worker();
            wobj.SID = sid;
            wobj = mapping.Query<Worker>(wobj, out errorMsg);
            if (string.IsNullOrWhiteSpace(errorMsg) == false || wobj == null  )
            {
                return re;
            }
          
            //2.拿出acode下的乡镇
            string sql = "select t.* from S_Town t, S_Area a where a.code_nvarchar=@ACODE and t.AID_nvarchar=a.AID_nvarchar";
            List<IDataParameter> plist = new List<IDataParameter>();
            plist.Add(new SqlParameter("@ACODE", SqlDbType.NVarChar) { Value = acode });
            List<Town> townlist = mapping.QueryList<Town>(sql, out errorMsg, plist);

            if (string.IsNullOrWhiteSpace(errorMsg) == false || townlist == null || townlist.Count==0)
            {
                return re;
            }

            //生成树 
            List<string> idarr=new List<string>();
            if (wobj.VIDS != null)
            {
                idarr.AddRange(wobj.VIDS.Split(','));

            }
       
            BArea ba=new BArea();
            foreach (Town town in townlist)
            {
                List<VillageVO> vlist=ba.getVillageList(town.TID);
                if(vlist==null||vlist.Count==0){
                    continue;
                } 
                bool selectAllflag=true;

                List<CheckTreeVO> sublist=new List<CheckTreeVO>();
                foreach(VillageVO vv in vlist){
                    CheckTreeVO sub=new CheckTreeVO();
                    sub.id=vv.vid;
                    sub.text=vv.name;
                    if(idarr.Contains(vv.vid)){
                        sub.select=true;
                    }else{
                        sub.select=false;
                        selectAllflag=false;
                    }
                    sublist.Add(sub);
                }

               CheckTreeVO root = new CheckTreeVO();
                root.id = "";
                root.text = town.Name;
                root.select=selectAllflag;
                root.children=sublist;
                re.Add(root);
            } 
            return re;
        }
    }
}
