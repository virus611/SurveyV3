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
 

namespace Business.Respondent
{
    public class BRspdQuery
    {
        /// <summary>
        /// 获取调查列表
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="acode"></param>
        /// <param name="qcode"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public List<RspdBase> getList(string starttime, string endtime,string acode, string qcode, int page, int size)
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;

            IMapping mapping = new SqlMapping(dbstr);
            List<IDataParameter> plist = new List<IDataParameter>();

            string sqlwhere = "";
            if (string.IsNullOrWhiteSpace(starttime) == false && string.IsNullOrWhiteSpace(endtime) == false)
            {
                sqlwhere += @" and b.StartTime_datetime BETWEEN @STARTTIME AND @ENDTIME";
                plist.Add(new SqlParameter("@STARTTIME", SqlDbType.DateTime) { Value = starttime + " 00:00:00.000" });
                plist.Add(new SqlParameter("@ENDTIME", SqlDbType.DateTime) { Value = endtime + " 23:59:59.999" });
            }
            else
            {
                if (string.IsNullOrWhiteSpace(starttime) == false)
                {
                    sqlwhere += @" and b.StartTime_datetime >= @STARTTIME";
                    plist.Add(new SqlParameter("@STARTTIME", SqlDbType.DateTime) { Value = starttime + " 00:00:00.000" });
                }
                if (string.IsNullOrWhiteSpace(endtime) == false)
                {
                    sqlwhere += @" and b.StartTime_datetime <= @ENDTIME";
                    plist.Add(new SqlParameter("@ENDTIME", SqlDbType.DateTime) { Value = endtime + " 23:59:59.999" });
                }
            }
            if(string.IsNullOrWhiteSpace(acode)==false){
                sqlwhere += @" and f.ACode_nvarchar = @ACODE";
                plist.Add(new SqlParameter("@ACODE", SqlDbType.NVarChar) { Value = acode });
            }

            if(string.IsNullOrWhiteSpace(qcode)==false){
                sqlwhere += @" and f.Code_nvarchar like  @QCODE";
                plist.Add(new SqlParameter("@QCODE", "%" + qcode + "%")); 
            }


            string sql = string.Format(@"with VRecord as (select row_number() over(order by b.StartTime_datetime desc) as rNumber,  
  b.* from B_RspdBase b,B_Family f where b.DelFlag_bit=0 {0} and f.RID_nvarchar=b.RID_nvarchar ) 
  select * from  VRecord where rNumber BETWEEN @begin AND @end", sqlwhere);

            plist.Add(new SqlParameter("@begin", SqlDbType.Int) { Value = (page - 1) * size + 1 });
            plist.Add(new SqlParameter("@end", SqlDbType.Int) { Value = page * size });

            List<RspdBase> list = mapping.QueryList<RspdBase>(sql, out errorMsg, plist);
            if (string.IsNullOrWhiteSpace(errorMsg) == false)
            { 
                return null;
            }
            return list;
        }

        /// <summary>
        /// 返回结果的数量
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="acode"></param>
        /// <param name="qcode"></param>
        /// <returns></returns>
        public int getCount(string starttime, string endtime, string acode, string qcode)
        {
            List<IDataParameter> plist = new List<IDataParameter>();

            string sqlwhere = "";
            if (string.IsNullOrWhiteSpace(starttime) == false && string.IsNullOrWhiteSpace(endtime) == false)
            {
                sqlwhere += @" and b.StartTime_datetime BETWEEN @STARTTIME AND @ENDTIME";
                plist.Add(new SqlParameter("@STARTTIME", SqlDbType.DateTime) { Value = starttime + " 00:00:00.000" });
                plist.Add(new SqlParameter("@ENDTIME", SqlDbType.DateTime) { Value = endtime + " 23:59:59.999" });
            }
            else
            {
                if (string.IsNullOrWhiteSpace(starttime) == false)
                {
                    sqlwhere += @" and b.StartTime_datetime >= @STARTTIME";
                    plist.Add(new SqlParameter("@STARTTIME", SqlDbType.DateTime) { Value = starttime + " 00:00:00.000" });
                }
                if (string.IsNullOrWhiteSpace(endtime) == false)
                {
                    sqlwhere += @" and b.StartTime_datetime <= @ENDTIME";
                    plist.Add(new SqlParameter("@ENDTIME", SqlDbType.DateTime) { Value = endtime + " 23:59:59.999" });
                }
            }
            if (string.IsNullOrWhiteSpace(acode) == false)
            {
                sqlwhere += @" and f.ACode_nvarchar = @ACODE";
                plist.Add(new SqlParameter("@ACODE", SqlDbType.NVarChar) { Value = acode });
            }

            if (string.IsNullOrWhiteSpace(qcode) == false)
            {
                sqlwhere += @" and f.Code_nvarchar like  @QCODE";
                plist.Add(new SqlParameter("@QCODE", "%" + qcode + "%"));
            }
            string sql = string.Format(@"select isnull(count(1),0) from B_RspdBase b,B_Family f where b.DelFlag_bit=0 {0} and f.RID_nvarchar=b.RID_nvarchar  ", sqlwhere);

            int c = new BQuery().queryScalarInt(sql, plist);
            return c;
        }


        /// <summary>
        /// 删除记录，并重置调查
        /// </summary>
        /// <param name="rid"></param>
        /// <returns></returns>
        public bool delRecord(string rid)
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;

            IMapping mapping = new SqlMapping(dbstr);

            List<IDataParameter> plist = new List<IDataParameter>();
            plist.Add(new SqlParameter("@RID", SqlDbType.NVarChar) { Value = rid });

 
            //肯定有记录
            string sql = @"select * from B_Family where  RID_nvarchar=@RID "; 
            Family  fobj = mapping.Query<Family>(sql, out errorMsg,plist);
            if (string.IsNullOrWhiteSpace(errorMsg) == false)
            {
                return false;
            }


            using (IDataBase db = new SqlDBHelper(dbstr, out errorMsg))
            {
                if (!string.IsNullOrWhiteSpace(errorMsg))
                    return false;
                db.OpenConn(out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                {
                    return false;
                }
                db.OpenTrans(out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                {
                    return false;
                }

                //主记录
                sql = @"update  B_RspdBase set DelFlag_bit=1 where RID_nvarchar=@RID";
                db.AddParameters(plist, true);
                int r = db.RunSqlNonQuery(sql, out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                {
                    db.RollbackTrans(out errorMsg);//回滚 
                    return false;
                }

                //子记录无视

                //家庭表（一般来说，有）
                if (fobj != null)
                {
                    plist.Clear();
                    plist.Add(new SqlParameter("@FID", SqlDbType.NVarChar) { Value = fobj.FID });
                    plist.Add(new SqlParameter("@DTime", SqlDbType.DateTime) { Value = DateTime.Now });
                    plist.Add(new SqlParameter("@GID", SqlDbType.NVarChar) { Value = Tools.createID()});
                    db.AddParameters(plist, true);


                    sql = @"update B_Family  set RID_nvarchar='',G01_nvarchar='',LastCode_nvarchar='',LastText_nvarchar='' where FID_nvarchar=@FID";
                     r = db.RunSqlNonQuery(sql, out errorMsg);
                    if (string.IsNullOrWhiteSpace(errorMsg) == false)
                    {
                        db.RollbackTrans(out errorMsg);//回滚 
                        return false;
                    }

                    //调查代码（2个）
                    
                    sql = "update B_ResultFamily set DelFlag_bit=1,DelTime_datetime=@DTime,GID_nvarchar=@GID where FID_nvarchar=@FID";
                    db.RunSqlNonQuery(sql, out errorMsg);
                    if (string.IsNullOrWhiteSpace(errorMsg) == false)
                    {
                        db.RollbackTrans(out errorMsg);//回滚 
                        return false;
                    }

                    sql = "update B_ResultPerson  set DelFlag_bit=1,DelTime_datetime=@DTime,GID_nvarchar=@GID where FID_nvarchar=@FID";
                    db.RunSqlNonQuery(sql, out errorMsg);
                    if (string.IsNullOrWhiteSpace(errorMsg) == false)
                    {
                        db.RollbackTrans(out errorMsg);//回滚 
                        return false;
                    }
                }

                db.CommitTrans(out errorMsg);//提交事务
                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                {
                    db.RollbackTrans(out errorMsg);//回滚
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 修改受访记录，只有姓名、电话
        /// </summary>
        /// <param name="rid"></param>
        /// <param name="name">姓名</param>
        /// <param name="phone">电话</param>
        /// <returns></returns>
        public bool editRecord(string rid, string oldname, string name, string phone)
        {
            List<IDataParameter> plist = new List<IDataParameter>();
            plist.Add(new SqlParameter("@RID", SqlDbType.NVarChar) { Value = rid });

            BQuery bq = new BQuery();
            List<string> sqlist = new List<string>();
            string sql;
            if (name != oldname)
            {
                //先拿家庭
                sql = @"select FID_nvarchar from B_Family where RID_nvarchar=@RID";

                DataTable dt = bq.getDataTable(sql, plist);
                if (dt == null || dt.Rows.Count == 0)
                {
                    return false;
                }
                string fid = dt.Rows[0]["FID_nvarchar"].ToString();

                plist.Add(new SqlParameter("@OLDNAME", SqlDbType.NVarChar) { Value = oldname });
                plist.Add(new SqlParameter("@FID", SqlDbType.NVarChar) { Value = fid });
                sql = @"update B_FamilyMember set name_nvarchar=@Name where FID_nvarchar=@FID and Name_nvarchar=@OLDNAME";
                sqlist.Add(sql);
            }

            sql = @"update B_RspdBase set Name_nvarchar=@Name,Phone_nvarchar=@Phone where RID_nvarchar=@RID";
            sqlist.Add(sql);

            plist.Add(new SqlParameter("@Name", SqlDbType.NVarChar) { Value = name });
            plist.Add(new SqlParameter("@Phone", SqlDbType.NVarChar) { Value = phone }); 
             
            return bq.executeSQLList(sqlist, plist);
         
        }

        /// <summary>
        /// 导出调查历史
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="acode"></param>
        /// <param name="qcode"></param>
        /// <returns></returns>
        public DataTable getExportRspdBase(string starttime, string endtime, string acode, string qcode)
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;

            IMapping mapping = new SqlMapping(dbstr);
            List<IDataParameter> plist = new List<IDataParameter>();

            string sqlwhere = "";
            if (string.IsNullOrWhiteSpace(starttime) == false && string.IsNullOrWhiteSpace(endtime) == false)
            {
                sqlwhere += @" and b.StartTime_datetime BETWEEN @STARTTIME AND @ENDTIME";
                plist.Add(new SqlParameter("@STARTTIME", SqlDbType.DateTime) { Value = starttime + " 00:00:00.000" });
                plist.Add(new SqlParameter("@ENDTIME", SqlDbType.DateTime) { Value = endtime + " 23:59:59.999" });
            }
            else
            {
                if (string.IsNullOrWhiteSpace(starttime) == false)
                {
                    sqlwhere += @" and b.StartTime_datetime >= @STARTTIME";
                    plist.Add(new SqlParameter("@STARTTIME", SqlDbType.DateTime) { Value = starttime + " 00:00:00.000" });
                }
                if (string.IsNullOrWhiteSpace(endtime) == false)
                {
                    sqlwhere += @" and b.StartTime_datetime <= @ENDTIME";
                    plist.Add(new SqlParameter("@ENDTIME", SqlDbType.DateTime) { Value = endtime + " 23:59:59.999" });
                }
            }
            if (string.IsNullOrWhiteSpace(acode) == false)
            {
                sqlwhere += @" and f.ACode_nvarchar = @ACODE";
                plist.Add(new SqlParameter("@ACODE", SqlDbType.NVarChar) { Value = acode });
            }

            if (string.IsNullOrWhiteSpace(qcode) == false)
            {
                sqlwhere += @" and f.Code_nvarchar like  @QCODE";
                plist.Add(new SqlParameter("@QCODE", "%" + qcode + "%"));
            }


            string sql = string.Format(@"select b.Code_nvarchar,b.RID_nvarchar from B_RspdBase b,B_Family f where b.DelFlag_bit=0 {0} and f.RID_nvarchar=b.RID_nvarchar order by b.Code_nvarchar", sqlwhere);
            List<RspdBase> list = mapping.QueryList<RspdBase>(sql, out errorMsg, plist);
            if(string.IsNullOrWhiteSpace(errorMsg)==false||list==null||list.Count==0){
                return null;
            }

            //构造返回的table
            DataTable dt = new DataTable();
            List<string> keys = new List<string>();
            keys.Add("field1"); 
            int i;
            for (i = 1; i < 11; i++)
            {
                string k = string.Format("a{0:00}", i);
                keys.Add(k);
            }

            for (i = 1; i < 27; i++)
            {
                string k = string.Format("b{0:00}", i); 
                keys.Add(k);
            }

            for (i = 1; i < 17; i++)
            {
                string k = string.Format("c{0:00}", i);
                keys.Add(k);
            }
            for (i = 1; i < 5; i++)
            {
                string k = string.Format("d{0:00}", i);
                keys.Add(k);
            }

            keys.Add("f01");
            keys.Add("f02");
            keys.Add("field2");
            keys.Add("f03");
            keys.Add("f04");
            keys.Add("f05");
            keys.Add("f06");
            keys.Add("f07");
            keys.Add("f08");
            keys.Add("f09");
            keys.Add("f10");
            keys.Add("f11");
            keys.Add("f12");
            keys.Add("f13");
            keys.Add("g01");

            foreach(string s in keys){
                dt.Columns.Add(s, typeof(String));
            } 
            Dictionary<string, string> cache = new Dictionary<string, string>();


            using (IDataBase db = new SqlDBHelper(dbstr, out errorMsg))
            {
                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                    return null;
                db.OpenConn(out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                    return null;
                mapping=new SqlDBMapping(db);

               
                foreach (RspdBase rb in list)
                {
                    plist.Clear();
                    cache.Clear();

                    plist.Add(new SqlParameter("@RID", SqlDbType.NVarChar) { Value = rb.RID});
                    sql = "select * from B_RspdAnswer where RID_nvarchar=@RID";
                    List<RspdAnswer> alist = mapping.QueryList<RspdAnswer>(sql, out errorMsg, plist);
                    if (string.IsNullOrWhiteSpace(errorMsg)==false)
                    {
                        return null;
                    }
                    DataRow dr = dt.NewRow();
                    dr["field1"] = rb.Code;
                    if (alist != null && alist.Count > 0)
                    {
                        //构造选项
                        foreach (RspdAnswer aa in alist)
                        { 
                            cache[aa.QNO.ToLower()] = aa.Item;
                        }
                        //从cache中取值
                        foreach (string key in keys)
                        {
                            if (key != "field1")
                            {
                                if (cache.ContainsKey(key))
                                {
                                    string m = cache[key];
                                    dr[key] = m;
                                }
                                else
                                {
                                    dr[key] = "";
                                }
                            }
                           
                        }
                    } 
                    dt.Rows.Add(dr); 
                }
            } 
             
            return dt;
        }


        public DataTable getExportRspdBaseForArea(string starttime, string endtime,out string second)
        {
            second = "";
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;

            IMapping mapping = new SqlMapping(dbstr);
            List<IDataParameter> plist = new List<IDataParameter>();

            //1.先拿地区列表
            List<Area> arealist = mapping.QueryList<Area>("select * from s_area", out errorMsg);
            if (string.IsNullOrWhiteSpace(errorMsg) == false || arealist == null || arealist.Count == 0)
            {
                return null;
            }

            string sqlwhere = "";
            if (string.IsNullOrWhiteSpace(starttime) == false && string.IsNullOrWhiteSpace(endtime) == false)
            {
                sqlwhere += @" and b.StartTime_datetime BETWEEN @STARTTIME AND @ENDTIME";
                plist.Add(new SqlParameter("@STARTTIME", SqlDbType.DateTime) { Value = starttime + " 00:00:00.000" });
                plist.Add(new SqlParameter("@ENDTIME", SqlDbType.DateTime) { Value = endtime + " 23:59:59.999" });
            }
            else
            {
                if (string.IsNullOrWhiteSpace(starttime) == false)
                {
                    sqlwhere += @" and b.StartTime_datetime >= @STARTTIME";
                    plist.Add(new SqlParameter("@STARTTIME", SqlDbType.DateTime) { Value = starttime + " 00:00:00.000" });
                }
                if (string.IsNullOrWhiteSpace(endtime) == false)
                {
                    sqlwhere += @" and b.StartTime_datetime <= @ENDTIME";
                    plist.Add(new SqlParameter("@ENDTIME", SqlDbType.DateTime) { Value = endtime + " 23:59:59.999" });
                }
            }
            DateTime dt1 = DateTime.Now;
            //2.构造返回的table
            DataTable dt = new DataTable();
            List<string> keys = new List<string>();
            keys.Add("field1");
            int i;
            for (i = 1; i < 11; i++)
            {
                string k = string.Format("a{0:00}", i);
                keys.Add(k);
            }

            for (i = 1; i < 27; i++)
            {
                string k = string.Format("b{0:00}", i);
                keys.Add(k);
            }

            for (i = 1; i < 17; i++)
            {
                string k = string.Format("c{0:00}", i);
                keys.Add(k);
            }
            for (i = 1; i < 5; i++)
            {
                string k = string.Format("d{0:00}", i);
                keys.Add(k);
            }

            keys.Add("f01");
            keys.Add("f02");
            keys.Add("field2");
            keys.Add("f03");
            keys.Add("f04");
            keys.Add("f05");
            keys.Add("f06");
            keys.Add("f07");
            keys.Add("f08");
            keys.Add("f09");
            keys.Add("f10");
            keys.Add("f11");
            keys.Add("f12");
            keys.Add("f13");
            keys.Add("g01");

            foreach (string s in keys)
            {
                dt.Columns.Add(s, typeof(String));
            }
            Dictionary<string, string> cache = new Dictionary<string, string>();


            string sql;


            //3.循环地区
            for (int o=0; o < arealist.Count; o++)
            {
                Area x = arealist[o];
                //3.1拿相关的主记录
                sql = string.Format(@"select b.Code_nvarchar,b.RID_nvarchar from B_RspdBase b,B_Family f where b.DelFlag_bit=0 {0} and f.ACode_nvarchar='{1}' and f.RID_nvarchar=b.RID_nvarchar order by b.Code_nvarchar", sqlwhere, x.Code);
                List<RspdBase> list = mapping.QueryList<RspdBase>(sql, out errorMsg, plist);
                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                {
                    //数据库出错
                    return null;
                }

                if (list == null || list.Count == 0)
                {
                    continue;
                }

                //3.2拿相关的答案项
                sql = string.Format(@"select a.* from B_RspdAnswer a, B_RspdBase b,B_Family f where b.DelFlag_bit=0 {0}
   and f.ACode_nvarchar='{1}' and f.RID_nvarchar=b.RID_nvarchar and a.RID_nvarchar=b.RID_nvarchar order by b.Code_nvarchar,a.Qno_nvarchar", sqlwhere, x.Code);
                List<RspdAnswer> aAllList = mapping.QueryList<RspdAnswer>(sql, out errorMsg, plist);
                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                {
                    //数据库出错
                    return null;
                }
                 
                foreach (RspdBase rb in list)
                { 
                    DataRow dr = dt.NewRow();
                    dr["field1"] = rb.Code;

                    cache.Clear();
                    List<RspdAnswer> alist = aAllList.FindAll(xa => xa.RID == rb.RID);
                    if (alist != null && alist.Count > 0)
                    {
                        //构造选项
                        foreach (RspdAnswer aa in alist)
                        {
                            cache[aa.QNO.ToLower()] = aa.Item;
                        }
                        //从cache中取值
                        foreach (string key in keys)
                        {
                            if (key != "field1")
                            {
                                if (cache.ContainsKey(key))
                                {
                                    string m = cache[key];
                                    dr[key] = m;
                                }
                                else
                                {
                                    dr[key] = "";
                                }
                            }

                        }
                    }
                    dt.Rows.Add(dr);
                } 
            }
            DateTime dt2 = DateTime.Now;
            TimeSpan ts1 = new TimeSpan(dt1.Ticks);
            TimeSpan ts2 = new TimeSpan(dt2.Ticks);
            TimeSpan ts3 = ts1.Subtract(ts2).Duration();
            second = string.Format("{0}", ts3.Seconds); 

            return dt;
        }


        /// <summary>
        /// 获取答案列表
        /// </summary>
        /// <param name="rid"></param>
        /// <returns></returns>
        public List<RspdAnswerVO> getRspdAnswerList(string rid)
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;

            IMapping mapping = new SqlMapping(dbstr);
            List<IDataParameter> plist = new List<IDataParameter>();
            plist.Add(new SqlParameter("@RID", SqlDbType.NVarChar) { Value = rid });
            string sql = "select * from B_RspdAnswer where RID_nvarchar=@RID order by QNO_nvarchar";

            List<RspdAnswer> alist = mapping.QueryList<RspdAnswer>(sql, out errorMsg, plist);
            if (string.IsNullOrWhiteSpace(errorMsg) == false)
            {
                return null;
            }

            List<RspdAnswerVO> re = new List<RspdAnswerVO>();
            foreach(RspdAnswer item in alist){
                RspdAnswerVO avo=new RspdAnswerVO();
                avo.sid=item.ID;
                if(item.QNO == "field2" ){
                    avo.qno="f02field2";
                }else if(item.QNO=="aa"){
                    avo.qno="f03aa";
                }else{
                     avo.qno=item.QNO;
                }
                avo.answer=item.Item;
                re.Add(avo);
            }
            re.Sort();
            return re;
        }

        public List<RspdAnswerVO> getRspdAnswerListF(string rid)
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;

            IMapping mapping = new SqlMapping(dbstr);
            List<IDataParameter> plist = new List<IDataParameter>();
            plist.Add(new SqlParameter("@RID", SqlDbType.NVarChar) { Value = rid });
             
            string sql = "select * from B_RspdAnswer where RID_nvarchar=@RID order by QNO_nvarchar";

            List<RspdAnswer> alist = mapping.QueryList<RspdAnswer>(sql, out errorMsg, plist);
            if (string.IsNullOrWhiteSpace(errorMsg) == false)
            {
                return null;
            }

            List<RspdAnswerVO> re = new List<RspdAnswerVO>();
            foreach (RspdAnswer item in alist)
            {
                RspdAnswerVO avo = new RspdAnswerVO();
                avo.sid = item.ID;
                if (item.QNO == "field2")
                {
                    avo.qno = "f02field2";
                }
                else if (item.QNO == "aa")
                {
                    avo.qno = "f03aa";
                }
                else if(item.QNO.ToLower().StartsWith("f"))
                {
                    avo.qno = item.QNO;
                }
                else
                {
                    continue;
                }
                avo.answer = item.Item;
                re.Add(avo);
            }
            re.Sort();
            return re;
        }

        /// <summary>
        /// 生成调查人员调查的记录
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="acode"></param>
        /// <returns></returns>
        public DataTable getExportRspdWorker(string starttime, string endtime, string acode)
        {

            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;

            IMapping mapping = new SqlMapping(dbstr);
            List<IDataParameter> plist = new List<IDataParameter>();

            string sqlwhere = "";
            if (string.IsNullOrWhiteSpace(starttime) == false && string.IsNullOrWhiteSpace(endtime) == false)
            {
                sqlwhere += @" and b.StartTime_datetime BETWEEN @STARTTIME AND @ENDTIME";
                plist.Add(new SqlParameter("@STARTTIME", SqlDbType.DateTime) { Value = starttime + " 00:00:00.000" });
                plist.Add(new SqlParameter("@ENDTIME", SqlDbType.DateTime) { Value = endtime + " 23:59:59.999" });
            }
            else
            {
                if (string.IsNullOrWhiteSpace(starttime) == false)
                {
                    sqlwhere += @" and b.StartTime_datetime >= @STARTTIME";
                    plist.Add(new SqlParameter("@STARTTIME", SqlDbType.DateTime) { Value = starttime + " 00:00:00.000" });
                }
                if (string.IsNullOrWhiteSpace(endtime) == false)
                {
                    sqlwhere += @" and b.StartTime_datetime <= @ENDTIME";
                    plist.Add(new SqlParameter("@ENDTIME", SqlDbType.DateTime) { Value = endtime + " 23:59:59.999" });
                }
            }
            if (string.IsNullOrWhiteSpace(acode) == false && acode != "admin")
            {
                sqlwhere += @" and f.ACode_nvarchar = @ACODE";
                plist.Add(new SqlParameter("@ACODE", SqlDbType.NVarChar) { Value = acode });
            }


            string sql = string.Format(@"select b.* from B_RspdBase b,B_Family f where b.DelFlag_bit=0 {0} and f.RID_nvarchar=b.RID_nvarchar order by b.StartTime_datetime", sqlwhere);
            List<RspdBase> list = mapping.QueryList<RspdBase>(sql, out errorMsg, plist);
            if (string.IsNullOrWhiteSpace(errorMsg) == false || list == null || list.Count == 0)
            {
                return null;
            }

            //构造返回的table
            DataTable dt = new DataTable();

            dt.Columns.Add("问卷编码", typeof(String));
            dt.Columns.Add("调查时间", typeof(String));
            dt.Columns.Add("受访者姓名", typeof(String));
            dt.Columns.Add("家庭地址", typeof(String));
            dt.Columns.Add("调查人员", typeof(String));

            foreach (RspdBase rb in list)
            {
                DataRow dr = dt.NewRow();
                dr[0] = rb.Code;
                dr[1] = rb.StartTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                dr[2] = rb.Name;
                dr[3] = rb.Address;
                dr[4] = rb.SName;
                dt.Rows.Add(dr);
            }

            return dt;
        }


        /// <summary>
        /// 用于答案的显示
        /// </summary>
        /// <param name="rid"></param>
        /// <returns></returns>
        public Dictionary<string, object> getRspdInfoJson(string rid)
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;

            IMapping mapping = new SqlMapping(dbstr);
            List<IDataParameter> plist = new List<IDataParameter>();
            plist.Add(new SqlParameter("@RID", SqlDbType.NVarChar) { Value = rid });
            string sql = "select * from B_RspdAnswer where RID_nvarchar=@RID";
             List<RspdAnswer> alist = mapping.QueryList<RspdAnswer>(sql, out errorMsg, plist);
            if (string.IsNullOrWhiteSpace(errorMsg) == false)
            {
                return null;
            }

            //其它题的返回，是个字符串数组
            List<string> clist=new List<string>();
            Dictionary<string,string> infolist=new Dictionary<string,string>();
            foreach(RspdAnswer item in alist){
                string qno=item.QNO.ToLower();
                if(qno.StartsWith("f")==false && qno!="aa" && qno !="field2"){
                    string ms=item.Item;
                    if(ms.Length==1){
                        clist.Add(string.Format("{0}_{1}",qno.ToUpper(),ms));
                    }else{
                        for(int i=0;i<ms.Length;i++){
                            clist.Add(string.Format("{0}_{1}",qno.ToUpper(),ms[i]));
                        }
                    } 
                }else{
                    infolist[qno]=item.Item;
                }
            }

            Dictionary<string,string> infoQquestionDict =new Dictionary<string,string>();
            //取得基本信息题目的答案
            sql=@"select * from Q_Info order by QNO_nvarchar";
            List<Info> tmpf=mapping.QueryList<Info>(sql,out errorMsg);
             if (string.IsNullOrWhiteSpace(errorMsg) == false||tmpf==null||tmpf.Count==0)
            {
                return null;
            }
            foreach(Info ivo in tmpf){
                string qno=ivo.QNO.ToLower();
                infoQquestionDict[qno]=ivo.Answer;
            }

            //信息字典表
            Dictionary<string,string> reinfo=new Dictionary<string,string>();
            string aa = "";
            if (infolist.ContainsKey("aa"))
            {
                aa = infolist["aa"];
            }
           

            // 处理 f02 field2 和 aa 
            string field2 = "";
            if (infolist.ContainsKey("field2"))
            {
                field2 = infolist["field2"];
            }
            
            if(field2.Length==1){
                field2="0"+field2;
            }
            string _f02 = "";
            if (infolist.ContainsKey("f02"))
            {
                _f02=infolist["f02"];
            } 
            reinfo["f02"]=_f02+"-"+field2;
    
            //f07 f08  f11 处理
            string f07 = "";
            if (infolist.ContainsKey("f07"))
            {
                f07 = infolist["f07"];
            }
            reinfo["f07"] = f07;

            string f08 = "";
            if (infolist.ContainsKey("f08"))
            {
                f08 = infolist["f08"];
            }
            reinfo["f08"] = f08;

            string f11 = "";
            if (infolist.ContainsKey("f11"))
            {
                f11 = infolist["f11"];
            }
            reinfo["f11"] = f11;
    
             List<string> selectlist=new List<string>();
             foreach(KeyValuePair<string, string> entry in infolist){
                 if(entry.Key == "f02" || entry.Key == "f07" ||entry.Key =="f08" || entry.Key == "f11" ||entry.Key=="field2" || entry.Key == "aa"){
                     continue;
                 }
                 if(entry.Key=="f03"){
                     if(string.IsNullOrWhiteSpace(aa)==false){
                         reinfo[entry.Key]=aa;
                         continue;
                     }
                 }
                 selectlist.Clear();
                 string[] answers=infoQquestionDict[entry.Key].Split(',');
                 foreach(char letter in entry.Value){
                     try { 
                     int select_pos= Int16.Parse(letter.ToString())-1;
                     selectlist.Add(answers[select_pos]);
                     }
                     catch (Exception)
                     {

                     }
                 }
                 if(selectlist.Count>0){
                     reinfo[entry.Key]=String.Join(",",selectlist);
                 }else{
                     reinfo[entry.Key]="";
                 }
             }

            Dictionary<string,string> reinfoUp=new Dictionary<string,string>();
            foreach(KeyValuePair<string, string> entry in reinfo){
                reinfoUp[entry.Key.ToUpper()]=entry.Value;
            }
            
            Dictionary<string,object> reobj=new Dictionary<string,object>();
            reobj["arr"]=clist;
            reobj["info"]=reinfoUp;
            return reobj;
   
        }

       

        /// <summary>
        /// 批量保存答案的修改
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool rspdAnswerSave(List<RspdAnswerVO> list) 
        {
            if (list == null || list.Count == 0)
            {
                return true;
            }
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;

            
            List<RspdAnswer> editlist = new List<RspdAnswer>();
            foreach (RspdAnswerVO vo in list)
            {
                RspdAnswer bean = new RspdAnswer();
                bean.ID=vo.sid;
                bean.Item=vo.answer; 
                editlist.Add(bean);
            }

            IMapping mapping = new SqlMapping(dbstr);
            mapping.Edit<RspdAnswer>(editlist, out errorMsg);
            if (string.IsNullOrWhiteSpace(errorMsg)==false)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 导入状态码表
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public DataTable getExportRspdCount(  )
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;

            IMapping mapping = new SqlMapping(dbstr);
            List<IDataParameter> plist = new List<IDataParameter>();

            string sqlwhere = "";
            /*
            if (string.IsNullOrWhiteSpace(starttime) == false && string.IsNullOrWhiteSpace(endtime) == false)
            {
                sqlwhere += @" and StartTime_datetime BETWEEN @STARTTIME AND @ENDTIME";
                plist.Add(new SqlParameter("@STARTTIME", SqlDbType.DateTime) { Value = starttime + " 00:00:00.000" });
                plist.Add(new SqlParameter("@ENDTIME", SqlDbType.DateTime) { Value = endtime + " 23:59:59.999" });
            }
            else
            {
                if (string.IsNullOrWhiteSpace(starttime) == false)
                {
                    sqlwhere += @" and StartTime_datetime >= @STARTTIME";
                    plist.Add(new SqlParameter("@STARTTIME", SqlDbType.DateTime) { Value = starttime + " 00:00:00.000" });
                }
                if (string.IsNullOrWhiteSpace(endtime) == false)
                {
                    sqlwhere += @" and StartTime_datetime <= @ENDTIME";
                    plist.Add(new SqlParameter("@ENDTIME", SqlDbType.DateTime) { Value = endtime + " 23:59:59.999" });
                }
            }
             */

            string sql = string.Format(@" 
  select  f.FID_nvarchar as [FID],f.RID_nvarchar as [RID],f.FCode_nvarchar as [FCode],
  f.Code_nvarchar as [Code],f.Name_nvarchar as [HName] ,f.KISH_nvarchar as [KISH]
  ,b.Name_nvarchar as [Name],f.LastCode_nvarchar as [LastCode],b.Phone_nvarchar as [Phone]
     from B_Family f
     left join
    (
     select RID_nvarchar ,Name_nvarchar,Phone_nvarchar from B_RspdBase
     where DelFlag_bit=0 {0}
    )b
    on  f.RID_nvarchar=b.RID_nvarchar
    where f.RID_nvarchar is not null and f.RID_nvarchar!=''  ", sqlwhere);
            
            BQuery bq = new BQuery();
            DataTable dtfamily = bq.getDataTable(sql, plist);

             

            //构造返回的table
            DataTable dt = new DataTable();
            dt.Columns.Add("FCode", typeof(String));
            dt.Columns.Add("HName", typeof(String));
            dt.Columns.Add("KISH", typeof(String));

            dt.Columns.Add("F1C", typeof(String));
            dt.Columns.Add("F1T", typeof(String));
            dt.Columns.Add("F2C", typeof(String));
            dt.Columns.Add("F2T", typeof(String));
            dt.Columns.Add("F3C", typeof(String));
            dt.Columns.Add("F3T", typeof(String));

            dt.Columns.Add("Name", typeof(String));

            dt.Columns.Add("P1C", typeof(String));
            dt.Columns.Add("P1T", typeof(String));
            dt.Columns.Add("P2C", typeof(String));
            dt.Columns.Add("P2T", typeof(String));
            dt.Columns.Add("P3C", typeof(String));
            dt.Columns.Add("P3T", typeof(String));

            dt.Columns.Add("LastCode", typeof(String));
            dt.Columns.Add("Code", typeof(String));
            dt.Columns.Add("Phone", typeof(String));

            if (dtfamily == null || dtfamily.Rows.Count == 0)
            {
                return dt;
            }

            using (IDataBase db = new SqlDBHelper(dbstr, out errorMsg))
            {
                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                    return null;
                db.OpenConn(out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                    return null;
                mapping = new SqlDBMapping(db);
                int c1 = 3;
                int c2 = 10;
                for (int i = 0; i < dtfamily.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["FCode"] = dtfamily.Rows[i]["FCode"];
                    dr["HName"] = dtfamily.Rows[i]["HName"];
                    dr["KISH"] = dtfamily.Rows[i]["KISH"];
                    dr["Name"] = dtfamily.Rows[i]["Name"];
                    dr["LastCode"] = dtfamily.Rows[i]["LastCode"];
                    dr["Code"] = dtfamily.Rows[i]["Code"];
                    dr["Phone"] = dtfamily.Rows[i]["Phone"];

                    plist.Clear();
                    plist.Add(new SqlParameter("@FID", SqlDbType.NVarChar) { Value = dtfamily.Rows[i]["FID"].ToString() });


                    sql = @"select distinct top 3 Result_nvarchar,Text_nvarchar,Time_datetime from b_resultFamily where DelFlag_bit=0 and FID_nvarchar=@FID order by Time_datetime"; 
                     c1 = 3;
                     List<ResultFamily> flist = mapping.QueryList<ResultFamily>(sql, out errorMsg, plist);
                     if (string.IsNullOrWhiteSpace(errorMsg) == false || flist == null || flist.Count == 0)
                     {
                         //do nothing
                     }
                     else
                     {
                         foreach (ResultFamily rf in flist)
                         {

                             dr[c1]=rf.Time.Value.ToString("yyyy-MM-dd HH:mm:ss");
                             c1++;
                             dr[c1]=rf.Result;
                             c1++;
                             if(c1>=8){
                                 break;
                             }
                         }
                     }



                     sql = @"select distinct top 3 Result_nvarchar,Text_nvarchar,Time_datetime from b_resultPerson where DelFlag_bit=0 and FID_nvarchar=@FID order by Time_datetime"; 
                    c2 = 10;
                    List<ResultPerson> personlist = mapping.QueryList<ResultPerson>(sql, out errorMsg, plist);
                    if (string.IsNullOrWhiteSpace(errorMsg) == false || personlist == null || personlist.Count == 0)
                    {
                        //do nothing
                    }
                    else
                    {
                        foreach (ResultPerson pf in personlist)
                        {

                            dr[c2] = pf.Time.Value.ToString("yyyy-MM-dd HH:mm:ss");
                            c2++;
                            dr[c2] = pf.Result;
                            c2++;
                            if (c2 >= 15)
                            {
                                break;
                            }
                        }
                    }

                    dt.Rows.Add(dr);
                }
                 
            }

            return dt;
        }


        /// <summary>
        /// 新的导出状态码
        /// </summary>
        /// <returns></returns>
        public DataTable getExportRspdCountNew(out string second)
        {
            second = "";
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;

            IMapping mapping = new SqlMapping(dbstr);

            //1.先拿地区列表
            List<Area> arealist = mapping.QueryList<Area>("select * from s_area", out errorMsg);
            if (string.IsNullOrWhiteSpace(errorMsg) == false || arealist == null || arealist.Count == 0)
            {
                return null;
            }

            DateTime dt1 = DateTime.Now;
            //构造返回的table
            DataTable dt = new DataTable();
            dt.Columns.Add("FCode", typeof(String));
            dt.Columns.Add("HName", typeof(String));
            dt.Columns.Add("KISH", typeof(String));

            dt.Columns.Add("F1C", typeof(String));
            dt.Columns.Add("F1T", typeof(String));
            dt.Columns.Add("F2C", typeof(String));
            dt.Columns.Add("F2T", typeof(String));
            dt.Columns.Add("F3C", typeof(String));
            dt.Columns.Add("F3T", typeof(String));

            dt.Columns.Add("Name", typeof(String));

            dt.Columns.Add("P1C", typeof(String));
            dt.Columns.Add("P1T", typeof(String));
            dt.Columns.Add("P2C", typeof(String));
            dt.Columns.Add("P2T", typeof(String));
            dt.Columns.Add("P3C", typeof(String));
            dt.Columns.Add("P3T", typeof(String));

            dt.Columns.Add("LastCode", typeof(String));
            dt.Columns.Add("Code", typeof(String));
            dt.Columns.Add("Phone", typeof(String));

            string sql;
            BQuery bq = new BQuery();
            //循环地区

            for (int o = 0; o < arealist.Count; o++)
            {
                Area x = arealist[o];
                sql = string.Format(@" 
  select  f.FID_nvarchar as [FID],f.RID_nvarchar as [RID],f.FCode_nvarchar as [FCode],
  f.Code_nvarchar as [Code],f.Name_nvarchar as [HName] ,f.KISH_nvarchar as [KISH]
  ,b.Name_nvarchar as [Name],f.LastCode_nvarchar as [LastCode],b.Phone_nvarchar as [Phone]
     from B_Family f
     left join
    (
     select RID_nvarchar ,Name_nvarchar,Phone_nvarchar from B_RspdBase
     where DelFlag_bit=0
    )b
    on  f.RID_nvarchar=b.RID_nvarchar
    where f.ACode_nvarchar='{0}' and f.RID_nvarchar is not null and f.RID_nvarchar!=''  ", x.Code);


                DataTable dtfamily = bq.getDataTable(sql, null);

                if (dtfamily == null || dtfamily.Rows.Count == 0)
                {
                    continue;
                }


                sql = string.Format(@"select b.FID_nvarchar, b.Result_nvarchar,b.Text_nvarchar,b.Time_datetime from b_resultFamily b,B_Family f where b.DelFlag_bit=0 and f.ACode_nvarchar='{0}' and b.FID_nvarchar=f.FID_nvarchar  order by b.FID_nvarchar,b.Time_datetime", x.Code);
                List<ResultFamily> fAllList = mapping.QueryList<ResultFamily>(sql, out errorMsg);
                if (fAllList == null)
                {
                    fAllList = new List<ResultFamily>();
                }

                sql = string.Format(@"select b.FID_nvarchar, b.Result_nvarchar,b.Text_nvarchar,b.Time_datetime from b_resultPerson b,B_Family f where b.DelFlag_bit=0 and f.ACode_nvarchar='{0}' and b.FID_nvarchar=f.FID_nvarchar  order by b.FID_nvarchar,b.Time_datetime", x.Code);
                List<ResultPerson> pAllList = mapping.QueryList<ResultPerson>(sql, out errorMsg);
                if (pAllList == null)
                {
                    pAllList = new List<ResultPerson>();
                }

                int c1 = 3;
                int c2 = 10;
                for (int i = 0; i < dtfamily.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    string fid=dtfamily.Rows[i]["FID"].ToString();

                    dr["FCode"] = dtfamily.Rows[i]["FCode"];
                    dr["HName"] = dtfamily.Rows[i]["HName"];
                    dr["KISH"] = dtfamily.Rows[i]["KISH"];
                    dr["Name"] = dtfamily.Rows[i]["Name"];
                    dr["LastCode"] = dtfamily.Rows[i]["LastCode"];
                    dr["Code"] = dtfamily.Rows[i]["Code"];
                    dr["Phone"] = dtfamily.Rows[i]["Phone"];

                     
                    c1 = 3;
                    List<ResultFamily> flist = fAllList.FindAll(xk => xk.FID == fid);
                    if (  flist != null && flist.Count > 0)
                    { 
                        foreach (ResultFamily rf in flist)
                        { 
                            dr[c1] = rf.Time.Value.ToString("yyyy-MM-dd HH:mm:ss");
                            c1++;
                            dr[c1] = rf.Result;
                            c1++;
                            if (c1 >= 8)
                            {
                                break;
                            }
                        }
                    }
                    fAllList.RemoveAll(xk => xk.FID == fid);
                     
                    c2 = 10;
                    List<ResultPerson> personlist = pAllList.FindAll(xk => xk.FID == fid);
                    if (personlist != null && personlist.Count > 0)
                    { 
                        foreach (ResultPerson pf in personlist)
                        {

                            dr[c2] = pf.Time.Value.ToString("yyyy-MM-dd HH:mm:ss");
                            c2++;
                            dr[c2] = pf.Result;
                            c2++;
                            if (c2 >= 15)
                            {
                                break;
                            }
                        }
                    }
                    pAllList.RemoveAll(xk => xk.FID == fid);
                    dt.Rows.Add(dr);
                }
            }

            DateTime dt2 = DateTime.Now;

            TimeSpan ts1 = new TimeSpan(dt1.Ticks);
            TimeSpan ts2 = new TimeSpan(dt2.Ticks);
            TimeSpan ts3 = ts1.Subtract(ts2).Duration(); 
            second = string.Format("{0}",ts3.Seconds); 
            return dt;
        }

        /// <summary>
        /// 更新受访者姓名
        /// </summary>
        /// <param name="rid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool updateName(string rid, string name)
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;

            IMapping mapping = new SqlMapping(dbstr);

            RspdBase obj = new RspdBase();
            obj.RID = rid;
            obj = mapping.Query<RspdBase>(obj, out errorMsg);
            if (string.IsNullOrWhiteSpace(errorMsg) == false || obj == null  )
            {
                return false;
            }

            obj.Name = name;
            int r = mapping.Edit<RspdBase>(obj, out errorMsg);
            if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
            {
                return false;
            }

            return true;
        }
    }
}
