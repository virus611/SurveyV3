using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.VO;
using Model.ORMModel;

using ORM;
using Library;
using DBHelper;
using System.Data;
using System.Data.SqlClient;
using Business.VO;

namespace Business.Mobile
{
    public class BUpload
    {

        BQuery bquery = new BQuery();

        private string longToStr(long t)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区 
            DateTime dt = startTime.AddMilliseconds(t);
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public bool saveUpload(RspdVO dict, out string rid, out string msg)
        {
            rid = string.Empty;
            msg = string.Empty;

            BaseInfoVO baseinfo = dict.baseinfo;
            if (baseinfo == null)
            {
                msg = "请求数据不完整";
                return false;
            }
            List<AnswerVO> answers = dict.answers;
            if (answers == null || answers.Count == 0)
            {
                msg = "请求数据不完整";
                return false;
            }
            List<FamilyMemberVO> family = dict.family;
            List<TimeVO> timelist = dict.timelist;
            string code = baseinfo.code;

            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);
            List<IDataParameter> plist = new List<IDataParameter>();

            plist.Add(new SqlParameter("@CODE", SqlDbType.NVarChar) { Value = code });
            string sql = @"select * from B_Family where DelFlag_bit=0 and  Code_nvarchar=@CODE";
            List<Family> flist = mapping.QueryList<Family>(sql, out errorMsg, plist);
            if (string.IsNullOrWhiteSpace(errorMsg) == false || flist == null || flist.Count == 0)
            {
                msg = "查询家庭数据出错";
                return false;
            }
            if (flist.Count > 1)
            {
                msg = "数据出错,该编码有多个家庭对应";
                return false;
            }

            rid = Tools.createID();

            Family fob = flist[0];
            List<FamilyMember> memberlist = new List<FamilyMember>();
            if (family != null && family.Count > 0)
            {
                //FamilyMember.objects.filter(strCode=code).delete()
                foreach (FamilyMemberVO item in family)
                {
                    FamilyMember sub = new FamilyMember();
                    sub.ID = Tools.createID();
                    sub.FID = fob.FID;
                    sub.Code = code;
                    sub.Name = item.name;
                    sub.Age = item.age;
                    sub.Sex = item.sex;
                    memberlist.Add(sub);
                }
            }
            RspdBase ob = new RspdBase();
            ob.RID = rid;
            ob.Name = baseinfo.name.Replace("\n","").Replace("\r","").Trim();
            ob.Sex = baseinfo.sex;
            ob.Age = baseinfo.age;
            ob.Code = code;
            ob.KISH = baseinfo.kish;
            ob.Phone = baseinfo.phone;
            ob.SID = baseinfo.sid;
            ob.SName = baseinfo.sname;
            ob.StartTime = DateTime.Parse(baseinfo.start);
            ob.EndTime = DateTime.Parse(baseinfo.end);
            ob.Second = baseinfo.second;
            ob.Address = baseinfo.address;
            ob.FamilyCount = baseinfo.familycount;
            ob.DelFlag = false;

            string ginput = "";


            List<RspdAnswer> qlist = new List<RspdAnswer>();
            foreach (AnswerVO item in answers)
            {
                string tmpqno = item.qno;
                if (tmpqno == "f02")
                {
                    //处理日期
                    string[] tmps = item.result.Split('-');
                    if (tmps.Length > 1)
                    {
                        //month
                        RspdAnswer ansdate = new RspdAnswer();
                        ansdate.ID = Tools.createID();
                        ansdate.Code = code;
                        ansdate.RID = rid;
                        ansdate.QNO = "field2";
                        if (tmps[1].StartsWith("0"))
                        {
                            ansdate.Item = tmps[1].Replace("0", "");
                        }
                        else
                        {
                            ansdate.Item = tmps[1];
                        }
                        ansdate.DelFlag = false;
                        qlist.Add(ansdate);


                        //year
                        RspdAnswer ansdate2 = new RspdAnswer();
                        ansdate2.ID = Tools.createID();
                        ansdate2.Code = code;
                        ansdate2.RID = rid;
                        ansdate2.QNO = tmpqno;
                        ansdate2.Item = tmps[0];
                        ansdate2.DelFlag = false;
                        qlist.Add(ansdate2);
                    }
                    else
                    {
                        //year
                        RspdAnswer ansdate2 = new RspdAnswer();
                        ansdate2.ID = Tools.createID();
                        ansdate2.Code = code;
                        ansdate2.RID = rid;
                        ansdate2.QNO = tmpqno;
                        ansdate2.Item = tmps[0];
                        ansdate2.DelFlag = false;
                        qlist.Add(ansdate2);
                    }
                }
                else
                {
                    RspdAnswer ans = new RspdAnswer();
                    ans.ID = Tools.createID();
                    ans.Code = code;
                    ans.RID = rid;
                    ans.QNO = item.qno;
                    ans.Item = item.result;
                    ans.DelFlag = false;
                    qlist.Add(ans);

                    if (ans.QNO == "f06")
                    {
                        ginput = ans.Item;
                    }
                }
            }

            ResultFamily rfobj = null;
            ResultPerson rpobj = null;
            bool editfamily = false;

            if (string.IsNullOrWhiteSpace(fob.LastCode)||fob.LastCode!="21")
            {
                rfobj = new ResultFamily();
                rfobj.ID = Tools.createID();
                rfobj.FID = fob.FID;
                rfobj.Time = DateTime.Now;
                rfobj.Text = "完成";
                rfobj.Result = "11";
                rfobj.DelFlag = false;
                rfobj.Code = fob.Code;

                rpobj = new ResultPerson();
                rpobj.ID = Tools.createID();
                rpobj.FID = fob.FID;
                rpobj.Time = DateTime.Now;
                rpobj.Text = "完成";
                rpobj.Result = "21";
                rpobj.DelFlag = false;
                rpobj.Code = fob.Code;

                editfamily = true;
                fob.RID = rid;
                fob.G01 = ginput;
                fob.LastText = "完成";
                fob.LastCode = "21";
            }

            List<RspdTime> tlist = new List<RspdTime>();
            if (timelist != null && timelist.Count > 0)
            {
                foreach (TimeVO tv in timelist)
                {
                    RspdTime rt = new RspdTime();
                    rt.ID = Tools.createID();
                    rt.RID = rid;
                    rt.Start = tv.isstart == 1;
                    rt.Time = longToStr(tv.stime);
                    tlist.Add(rt);
                }
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

                
                mapping = new SqlDBMapping(db);
                //主记录
                long d1;
                int r = mapping.Add<RspdBase>(ob, out d1, out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                {
                    msg = "保存答题结果失败";
                    db.RollbackTrans(out errorMsg);//回滚
                    return false;
                }
                //答案
                if (qlist.Count > 0)
                {
                    r = mapping.Add<RspdAnswer>(qlist, out errorMsg);
                    if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                    {
                        msg = "保存答案失败";
                        db.RollbackTrans(out errorMsg);//回滚 
                        return false;
                    }
                }
                

                //家庭记录
                if (editfamily)
                {
                    r = mapping.Edit<Family>(fob, out errorMsg);
                    if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                    {
                        msg = "更新家庭信息失败";
                        db.RollbackTrans(out errorMsg);//回滚
                        return false;
                    }
                }

                //家庭成员
                if (memberlist.Count > 0)
                {
                    sql = @"delete from B_FamilyMember where Code_nvarchar=@CODE";
                    db.AddParameters(plist, true);

                    db.RunSqlNonQuery(sql, out errorMsg);
                    if (string.IsNullOrWhiteSpace(errorMsg) == false)
                    {
                        msg = "更新家庭成员失败";
                        db.RollbackTrans(out errorMsg);//回滚 
                        return false;
                    }

                    r = mapping.Add<FamilyMember>(memberlist, out errorMsg);
                    if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                    {
                        msg = "更新家庭成员失败";
                        db.RollbackTrans(out errorMsg);//回滚 
                        return false;
                    }

                }
              

                //代码记录
                long d = 0;

                if (rpobj != null)
                {  r = mapping.Add<ResultPerson>(rpobj, out d, out errorMsg);
                    if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                    {
                        msg = "保存答案失败";
                        db.RollbackTrans(out errorMsg);//回滚 
                        return false;
                    }
                }

                if (rfobj != null)
                {
                    r = mapping.Add<ResultFamily>(rfobj, out d, out errorMsg);
                    if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                    {
                        msg = "保存答案失败";
                        db.RollbackTrans(out errorMsg);//回滚 
                        return false;
                    }
                }

                //时间
                if (tlist.Count > 0)
                {
                    mapping.Add<RspdTime>(tlist, out errorMsg);
                    if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                    {
                        msg = "记录调查时间出错";
                        db.RollbackTrans(out errorMsg);//回滚 
                        return false;
                    }
                }

                db.CommitTrans(out errorMsg);//提交事务
                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                {
                    msg = "保存失败";
                    db.RollbackTrans(out errorMsg);//回滚
                    return false;
                }
            }
            return true;
        }



        public bool saveState(string code, string result, out string msg)
        {
            msg = string.Empty;

            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);
            List<IDataParameter> plist = new List<IDataParameter>();

            plist.Add(new SqlParameter("@CODE", SqlDbType.NVarChar) { Value = code });
            string sql = @"select * from B_Family where DelFlag_bit=0 and Code_nvarchar=@CODE";
            List<Family> flist = mapping.QueryList<Family>(sql, out errorMsg, plist);
            if (string.IsNullOrWhiteSpace(errorMsg) == false || flist == null || flist.Count == 0)
            {
                msg = "查询家庭数据出错";
                return false;
            }
            if (flist.Count > 1)
            {
                msg = "数据出错,该编码有多个家庭对应";
                return false;
            }
              
            Family fob = flist[0];
    

            bool flag = false;
            if (result == "12")
            {
                flag = save12(fob);
            }
            else if (result == "14")
            {
                flag = save14(fob);
            }
            else if (result == "15")
            {
                flag = save15(fob);
            }
            else if (result == "16")
            {
                flag = save16(fob);
            }
            else if (result == "23")
            {
                flag = save23(fob);
            }
            else if (result == "24")
            {
                flag = save24(fob);
            }
            else if (result == "25")
            {
                flag = save25(fob);
            }
            else if (result == "27")
            {
                flag = save27(fob);
            }
            if (flag == false)
            {
                msg = "保存失败";
                return false;
            }
            else
            {
                return true;
            }
        }



        private bool save12(Family fob)
        {
           

            ResultFamily rfobj = new ResultFamily();
            rfobj.ID = Tools.createID();
            rfobj.FID = fob.FID;
            rfobj.Time = DateTime.Now;
            rfobj.Text = "拒绝";
            rfobj.Result = "12";
            rfobj.DelFlag = false;
            rfobj.Code = fob.Code;

          
                fob.LastCode = "12";
                fob.LastText = "拒绝";
                fob.RID = "拒绝"; 
            
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;

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
                long d;
                IMapping mapping = new SqlDBMapping(db);
                int r = mapping.Add<ResultFamily>(rfobj, out d, out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                {
                    db.RollbackTrans(out errorMsg);//回滚
                    return false;
                }

          
                    r = mapping.Edit<Family>(fob, out errorMsg);
                    if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                    {
                        db.RollbackTrans(out errorMsg);//回滚
                        return false;
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


        private bool save14(Family fob)
        {
            List<IDataParameter> plist = new List<IDataParameter>();
            string sql = @"select isnull(count(1),0) from B_ResultFamily where DelFlag_bit=0 and FID_nvarchar=@FID";
            plist.Add(new SqlParameter("@FID", SqlDbType.NVarChar) { Value = fob.FID });
            int cnt = bquery.queryScalarInt(sql, plist);

            ResultFamily rfobj = new ResultFamily();
            rfobj.ID = Tools.createID();
            rfobj.FID = fob.FID;
            rfobj.Time = DateTime.Now;
            rfobj.Text = "不在家";
            rfobj.Result = "14";
            rfobj.DelFlag = false;
            rfobj.Code = fob.Code;

            bool edit = false;
            if (cnt >= 2)
            {
                edit = true;
                fob.LastCode = "14";
                fob.LastText = "不在家";
                fob.RID = "不在家"; 
            }

            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;

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
                long d;
                IMapping mapping = new SqlDBMapping(db);
                int r = mapping.Add<ResultFamily>(rfobj, out d, out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                {
                    db.RollbackTrans(out errorMsg);//回滚
                    return false;
                }

                if (edit == true)
                {
                    r = mapping.Edit<Family>(fob, out errorMsg);
                    if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
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

        private bool save15(Family fob)
        {

            ResultFamily rfobj = new ResultFamily();
            rfobj.ID = Tools.createID();
            rfobj.FID = fob.FID;
            rfobj.Time = DateTime.Now;
            rfobj.Text = "无人居住/空房/已无此家庭/不是家庭";
            rfobj.Result = "15";
            rfobj.DelFlag = false;
            rfobj.Code = fob.Code;


            fob.LastCode = "15";
            fob.LastText = "无人居住/空房/已无此家庭/不是家庭";
            fob.RID = "无人居住"; 

            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;

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
                long d;
                IMapping mapping = new SqlDBMapping(db);
                int r = mapping.Add<ResultFamily>(rfobj, out d, out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                {
                    db.RollbackTrans(out errorMsg);//回滚
                    return false;
                }


                r = mapping.Edit<Family>(fob, out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                {
                    db.RollbackTrans(out errorMsg);//回滚
                    return false;
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

        private bool save16(Family fob)
        {

            ResultFamily rfobj = new ResultFamily();
            rfobj.ID = Tools.createID();
            rfobj.FID = fob.FID;
            rfobj.Time = DateTime.Now;
            rfobj.Text = "其它";
            rfobj.Result = "16";
            rfobj.DelFlag = false;
            rfobj.Code = fob.Code;

            fob.LastCode = "16";
            fob.LastText = "其它";
            fob.RID = "其它"; 

            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;

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
                long d;
                IMapping mapping = new SqlDBMapping(db);
                int r = mapping.Add<ResultFamily>(rfobj, out d, out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                {
                    db.RollbackTrans(out errorMsg);//回滚
                    return false;
                }


                r = mapping.Edit<Family>(fob, out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                {
                    db.RollbackTrans(out errorMsg);//回滚
                    return false;
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

        private bool save23(Family fob)
        {


            ResultPerson rpobj = new ResultPerson();
            rpobj.ID = Tools.createID();
            rpobj.FID = fob.FID;
            rpobj.Time = DateTime.Now;
            rpobj.Text = "拒绝";
            rpobj.Result = "23";
            rpobj.DelFlag = false;
            rpobj.Code = fob.Code;

            ResultFamily rfobj = new ResultFamily();
            rfobj.ID = Tools.createID();
            rfobj.FID = fob.FID;
            rfobj.Time = DateTime.Now;
            rfobj.Text = "拒绝";
            rfobj.Result = "12";
            rfobj.DelFlag = false;
            rfobj.Code = fob.Code;

            fob.LastCode = "23";
            fob.LastText = "拒绝";
            fob.RID = "拒绝"; 

            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;

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
                long d;
                IMapping mapping = new SqlDBMapping(db);
                int r = mapping.Add<ResultFamily>(rfobj, out d, out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                {
                    db.RollbackTrans(out errorMsg);//回滚
                    return false;
                }
                r = mapping.Add<ResultPerson>(rpobj, out d, out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                {
                    db.RollbackTrans(out errorMsg);//回滚
                    return false;
                }

                r = mapping.Edit<Family>(fob, out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                {
                    db.RollbackTrans(out errorMsg);//回滚
                    return false;
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

        private bool save24(Family fob)
        {
          

            ResultPerson rpobj = new ResultPerson();
            rpobj.ID = Tools.createID();
            rpobj.FID = fob.FID;
            rpobj.Time = DateTime.Now;
            rpobj.Text = "无能力回答";
            rpobj.Result = "24";
            rpobj.DelFlag = false;
            rpobj.Code = fob.Code;

            ResultFamily rfobj = new ResultFamily();
            rfobj.ID = Tools.createID();
            rfobj.FID = fob.FID;
            rfobj.Time = DateTime.Now;
            rfobj.Text = "无能力回答";
            rfobj.Result = "13";
            rfobj.DelFlag = false;
            rfobj.Code = fob.Code;

            fob.LastCode = "24";
            fob.LastText = "无能力回答";
            fob.RID = "无能力回答"; 

            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;

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
                long d;
                IMapping mapping = new SqlDBMapping(db);
                int r = mapping.Add<ResultFamily>(rfobj, out d, out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                {
                    db.RollbackTrans(out errorMsg);//回滚
                    return false;
                }
                r = mapping.Add<ResultPerson>(rpobj, out d, out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                {
                    db.RollbackTrans(out errorMsg);//回滚
                    return false;
                }

                r = mapping.Edit<Family>(fob, out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                {
                    db.RollbackTrans(out errorMsg);//回滚
                    return false;
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

        private bool save25(Family fob)
        {
            List<IDataParameter> plist = new List<IDataParameter>();
            string sql = @"select isnull(count(1),0) from B_ResultPerson where DelFlag_bit=0 and FID_nvarchar=@FID";
            plist.Add(new SqlParameter("@FID", SqlDbType.NVarChar) { Value = fob.FID });
            int cnt = bquery.queryScalarInt(sql, plist);

            ResultPerson rpobj = new ResultPerson();
            rpobj.ID = Tools.createID();
            rpobj.FID = fob.FID;
            rpobj.Time = DateTime.Now;
            rpobj.Text = "不在家";
            rpobj.Result = "25";
            rpobj.DelFlag = false;
            rpobj.Code = fob.Code;

            ResultFamily rfobj = new ResultFamily();
            rfobj.ID = Tools.createID();
            rfobj.FID = fob.FID;
            rfobj.Time = DateTime.Now;
            rfobj.Text = "不在家";
            rfobj.Result = "14";
            rfobj.DelFlag = false;
            rfobj.Code = fob.Code;

            bool edit = false;
            if (cnt >= 2)
            {
                edit = true;
                fob.LastCode = "25";
                fob.LastText = "不在家";
                fob.RID = "不在家"; 
            }

            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;

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
                long d;
                IMapping mapping = new SqlDBMapping(db);
                int r = mapping.Add<ResultFamily>(rfobj, out d, out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                {
                    db.RollbackTrans(out errorMsg);//回滚
                    return false;
                }

                r = mapping.Add<ResultPerson>(rpobj, out d, out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                {
                    db.RollbackTrans(out errorMsg);//回滚
                    return false;
                }


                if (edit == true)
                {
                    r = mapping.Edit<Family>(fob, out errorMsg);
                    if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
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
                return true;
            }
        }

        private bool save27(Family fob)
        {

          

            ResultPerson rpobj = new ResultPerson();
            rpobj.ID = Tools.createID();
            rpobj.FID = fob.FID;
            rpobj.Time = DateTime.Now;
            rpobj.Text = "其它";
            rpobj.Result = "27";
            rpobj.DelFlag = false;
            rpobj.Code = fob.Code;

            ResultFamily rfobj = new ResultFamily();
            rfobj.ID = Tools.createID();
            rfobj.FID = fob.FID;
            rfobj.Time = DateTime.Now;
            rfobj.Text = "其它";
            rfobj.Result = "16";
            rfobj.DelFlag = false;
            rfobj.Code = fob.Code;

         
                fob.LastCode = "27";
                fob.LastText = "其它";
                fob.RID = "其它";
              

            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;

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
                long d;
                IMapping mapping = new SqlDBMapping(db);
                int r = mapping.Add<ResultFamily>(rfobj, out d, out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                {
                    db.RollbackTrans(out errorMsg);//回滚
                    return false;
                }

                r = mapping.Add<ResultPerson>(rpobj, out d, out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                {
                    db.RollbackTrans(out errorMsg);//回滚
                    return false;
                }

               
                    r = mapping.Edit<Family>(fob, out errorMsg);
                    if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                    {
                        db.RollbackTrans(out errorMsg);//回滚
                        return false;
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
         
    }
}
