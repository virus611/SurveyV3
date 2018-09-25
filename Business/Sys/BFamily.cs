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
    public class BFamily
    {
        /// <summary>
        /// 批量删除家庭
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool delFamily(List<string> list)
        {
            if (list == null || list.Count == 0)
            {
                return true;
            }

            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;

            IMapping mapping = new SqlMapping(dbstr);

            List<IDataParameter> plist = new List<IDataParameter>();
            string sql = "";
           

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

                foreach (string id in list)
                {
                    //家庭主表
                  
                    plist.Add(new SqlParameter("@FID", SqlDbType.NVarChar) { Value = id });
                    plist.Add(new SqlParameter("@GID", SqlDbType.NVarChar) { Value = Tools.createID() });
                    plist.Add(new SqlParameter("@DelTime", SqlDbType.DateTime) { Value = DateTime.Now });
                    db.AddParameters(plist, true);

                    sql = "update B_Family set DelFlag_bit=1 where FID_nvarchar=@FID";
                    db.RunSqlNonQuery(sql, out errorMsg);
                    if (string.IsNullOrWhiteSpace(errorMsg) == false)
                    {
                        db.RollbackTrans(out errorMsg);//回滚 
                        return false;
                    }


                    //家庭成员无视
                    /*
                    sql = "update   B_FamilyMember set DelFlag_bit=1,DelTime_datetime=@DelTime where FID_nvarchar=@FID";
                    db.RunSqlNonQuery(sql, out errorMsg);
                    if (string.IsNullOrWhiteSpace(errorMsg) == false)
                    {
                        db.RollbackTrans(out errorMsg);//回滚 
                        return false;
                    }
                     **/


                    //调查代码（2个）
                    sql = "update B_ResultFamily  set DelFlag_bit=1,DelTime_datetime=@DelTime,GID_nvarchar=@GID where FID_nvarchar=@FID";
                    db.RunSqlNonQuery(sql, out errorMsg);
                    if (string.IsNullOrWhiteSpace(errorMsg) == false)
                    {
                        db.RollbackTrans(out errorMsg);//回滚 
                        return false;
                    }

                    sql = "update B_ResultPerson  set DelFlag_bit=1,DelTime_datetime=@DelTime,GID_nvarchar=@GID where FID_nvarchar=@FID";
                    db.RunSqlNonQuery(sql, out errorMsg);
                    if (string.IsNullOrWhiteSpace(errorMsg) == false)
                    {
                        db.RollbackTrans(out errorMsg);//回滚 
                        return false;
                    }
                }
                

                //其它操作
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
        /// 修改保存
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool saveFamily(List<FamilyVO> list)
        {
            if (list == null || list.Count == 0)
            {
                return true;
            }
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);

          
            List<Family> editList = new List<Family>();
            foreach (FamilyVO vo in list)
            {
                Family bean = new Family();
                bean.FID = vo.fid;
                bean.Name = vo.name;
                bean.Address = vo.address; 
                editList.Add(bean);
              
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

                int r = 0; 
                if (editList.Count > 0)
                {
                    r = mapping.Edit<Family>(editList, out errorMsg);
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


        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool importFamily(List<FamilyVO> list)
        {
            if (list == null || list.Count == 0)
            {
                return true;
            }
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);


            List<Family> addList = new List<Family>();
            foreach (FamilyVO vo in list)
            {
                Family bean = new Family();
                bean.FID = Tools.createID(); 
                bean.KISH = vo.kish;
                bean.ACode=vo.acode;
                bean.TCode=vo.tcode;
                bean.VCode=vo.vcode;
                bean.FCode=vo.fcode;
                bean.Code=string.Format("{0}{1}{2}{3}",vo.acode,vo.tcode,vo.vcode,vo.fcode);
                bean.Name=vo.name;
                bean.Address=vo.address;
                bean.RID="";
                bean.LastCode="";
                bean.LastText="";
                bean.G01="";
                bean.DelFlag = false;
                addList.Add(bean);

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

                int r = 0;
                if (addList.Count > 0)
                {
                    r = mapping.Add<Family>(addList, out errorMsg);
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


        /// <summary>
        /// 返回家庭成员（字段已转义）
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable getMemberList(string code)
        {
            List<IDataParameter> plist = new List<IDataParameter>();
            string sql = @"select m.ID_nvarchar as [sid],m.Name_nvarchar as [name],m.Sex_int as [sex],m.Age_int as [age]
            from B_FamilyMember m,B_Family f where f.Code_nvarchar=@CODE and m.FID_nvarchar=f.FID_nvarchar";
            plist.Add(new SqlParameter("@CODE", SqlDbType.NVarChar) { Value = code });

            DataTable dt = new BQuery().getDataTable(sql, plist);
            return dt;
        }


        /// <summary>
        /// 查询家庭列表
        /// </summary>
        /// <param name="acode"></param>
        /// <param name="tcode"></param>
        /// <param name="vcode"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public List<FamilyVO> getFamilyList(string acode, string tcode, string vcode, int page, int size)
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);

            string sql = @"select * from B_Family where DelFlag_bit=0 and ACode_nvarchar=@ACODE and TCode_nvarchar=@TCODE and VCode_nvarchar=@VCODE  order by Address_nvarchar";
            List<IDataParameter> plist = new List<IDataParameter>();
            plist.Add(new SqlParameter("@ACODE", SqlDbType.NVarChar) { Value = acode });
            plist.Add(new SqlParameter("@TCODE", SqlDbType.NVarChar) { Value = tcode });
            plist.Add(new SqlParameter("@VCODE", SqlDbType.NVarChar) { Value = vcode });

            List<Family> list = mapping.QueryList<Family>(sql, out errorMsg,plist);
            if (string.IsNullOrWhiteSpace(errorMsg) == false || list == null || list.Count == 0)
            {
                return null;
            }

            List<FamilyVO> re = new List<FamilyVO>();
            foreach (Family item in list)
            {
                FamilyVO sub = new FamilyVO();
                sub.fid=item.FID;
                sub.name=item.Name;
                sub.code=item.Code;
                sub.address=item.Address;
                sub.kish=item.KISH;
                sub.rid=item.RID;
                sub.g1=item.G01;
                sub.lastcode=item.LastCode;
                sub.lasttext=item.LastText; 
                re.Add(sub);
            } 
            return re;
        }

        public  int getFamilyListCount(string acode, string tcode, string vcode)
        {
            
            string sql = @"select isnull(count(1),0) from B_Family where DelFlag_bit=0 and ACode_nvarchar=@ACODE and TCode_nvarchar=@TCODE and VCode_nvarchar=@VCODE  ";
            List<IDataParameter> plist = new List<IDataParameter>();
            plist.Add(new SqlParameter("@ACODE", SqlDbType.NVarChar) { Value = acode });
            plist.Add(new SqlParameter("@TCODE", SqlDbType.NVarChar) { Value = tcode });
            plist.Add(new SqlParameter("@VCODE", SqlDbType.NVarChar) { Value = vcode });

            int c = new BQuery().queryScalarInt(sql, plist);
            return c;
        }



        /// <summary>
        /// 重置某些家庭的调查结果
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool resetRids(List<string> list)
        {
            if (list == null || list.Count == 0)
            {
                return true;
            }
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;

            IMapping mapping = new SqlMapping(dbstr);

            List<IDataParameter> plist = new List<IDataParameter>();
            string sql = "";
            int r = 0;

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

                foreach (string id in list)
                {
                    //家庭主表
                    plist.Clear();
                    plist.Add(new SqlParameter("@FID", SqlDbType.NVarChar) { Value = id });
                    plist.Add(new SqlParameter("@DelTime", SqlDbType.DateTime) { Value = DateTime.Now });
                    plist.Add(new SqlParameter("@GID", SqlDbType.NVarChar) { Value = Tools.createID() });
                    db.AddParameters(plist, true);

                    sql = "update B_Family set RID_nvarchar='',G01_nvarchar='',LastCode_nvarchar='',LastText_nvarchar='' where FID_nvarchar=@FID";
                    r=db.RunSqlNonQuery(sql, out errorMsg);
                    if (string.IsNullOrWhiteSpace(errorMsg) == false||r==0)
                    {
                        db.RollbackTrans(out errorMsg);//回滚 
                        return false;
                    }
                     
                    //调查代码（2个）
                    sql = "update B_ResultFamily  set DelFlag_bit=1,DelTime_datetime=@DelTime,GID_nvarchar=@GID where FID_nvarchar=@FID";
                    db.RunSqlNonQuery(sql, out errorMsg);
                    if (string.IsNullOrWhiteSpace(errorMsg) == false)
                    {
                        db.RollbackTrans(out errorMsg);//回滚 
                        return false;
                    }

                    sql = "update B_ResultPerson  set DelFlag_bit=1,DelTime_datetime=@DelTime,GID_nvarchar=@GID where FID_nvarchar=@FID";
                    db.RunSqlNonQuery(sql, out errorMsg);
                    if (string.IsNullOrWhiteSpace(errorMsg) == false)
                    {
                        db.RollbackTrans(out errorMsg);//回滚 
                        return false;
                    }
                }


                //其它操作
                db.CommitTrans(out errorMsg);//提交事务
                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                {
                    db.RollbackTrans(out errorMsg);//回滚
                    return false;
                }
            }
            return true;
        }



        public List<FamilyVO> getMobileFamilyList(string[] vids )
        {
            if (vids == null || vids.Length == 0)
            {
                return null;
            }

            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);
            List<IDataParameter> plist = new List<IDataParameter>();


            string sqlwhere=" and v.VID_nvarchar in (";
            string sb = "";
            for (int i = 0; i < vids.Length; i++)
            {
                sb += ",@T" + i.ToString();
                plist.Add(new SqlParameter("@T" + i.ToString(), SqlDbType.NVarChar) { Value = vids[i] });
            } 
            sqlwhere += sb.Substring(1) + ") ";
             
            string sql = string.Format(@"select a.Code_nvarchar as [acode],t.Code_nvarchar as [tcode],v.Code_nvarchar as [vcode] from S_Area a,S_Town t,S_Village v
 where 1=1 {0}  and v.TID_nvarchar=t.TID_nvarchar and t.AID_nvarchar=a.AID_nvarchar", sqlwhere);
            DataTable dt = new BQuery().getDataTable(sql,plist);

            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            List<FamilyVO> re = new List<FamilyVO>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sql = @"select f.* from B_Family f where DelFlag_bit=0 and  ACode_nvarchar=@ACODE and TCode_nvarchar=@TCODE and VCode_nvarchar=@VCODE  order by f.Code_nvarchar";
                plist.Clear();
                plist.Add(new SqlParameter("@ACODE", SqlDbType.NVarChar) { Value = (string)dt.Rows[i]["acode"] });
                plist.Add(new SqlParameter("@TCODE", SqlDbType.NVarChar) { Value = (string)dt.Rows[i]["tcode"] });
                plist.Add(new SqlParameter("@VCODE", SqlDbType.NVarChar) { Value = (string)dt.Rows[i]["vcode"] });

                List<Family> list = mapping.QueryList<Family>(sql, out errorMsg,plist);
                if (string.IsNullOrWhiteSpace(errorMsg) == false )
                {
                    return null;
                }
                if (list == null || list.Count == 0)
                {
                    continue;
                } 

                foreach (Family item in list)
                {
                    FamilyVO sub = new FamilyVO();
                    sub.fid = item.FID;
                    sub.name = item.Name;
                    sub.code = item.Code;
                    sub.address = item.Address;
                    sub.kish = item.KISH;
                    sub.rid = item.RID; 
                    sub.lastcode = item.LastCode;
                    sub.lasttext = item.LastText;
                    re.Add(sub);
                }
            } 
            return re;
        }


        /// <summary>
        /// 保存家庭成员
        /// </summary>
        /// <param name="code"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool saveMember(string code, List<FamilyMemberVO> list)
        {
            if (list == null || list.Count == 0)
            {
                return true;
            }
            //先通过code拿家庭信息

            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);
            List<IDataParameter> plist = new List<IDataParameter>();
            string sql = @"select * from B_Family where Code_nvarchar=@CODE order by DelFlag_bit";
            plist.Add(new SqlParameter("@CODE", SqlDbType.NVarChar) { Value = code });

            Family fobj = mapping.Query<Family>(sql, out errorMsg, plist);
            if (string.IsNullOrWhiteSpace(errorMsg) == false || fobj == null)
            {
                return false;
            }
             
            List<FamilyMember> bluklist = new List<FamilyMember>();
            foreach (FamilyMemberVO vo in list)
            {
                FamilyMember mm = new FamilyMember();
                mm.ID = Tools.createID();
                mm.Code = code;
                mm.FID = fobj.FID;
                mm.Name = vo.name;
                mm.Age = vo.age;
                mm.Sex = vo.sex;
                bluklist.Add(mm);
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

                //先删除code下的
                sql = @"delete from B_FamilyMember where Code_nvarchar=@CODE";
                db.AddParameters(plist, true);
                int r = db.RunSqlNonQuery(sql, out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                {
                    db.RollbackTrans(out errorMsg);//回滚 
                    return false;
                }

                mapping = new SqlDBMapping(db);
                r = mapping.Add<FamilyMember>(bluklist, out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                {
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

        /// <summary>
        /// 读取家庭成员列表
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<FamilyMemberVO> loadMember(string code)
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);
            List<IDataParameter> plist = new List<IDataParameter>();
            string sql = @"select m.* from B_Family f, B_FamilyMember m where f.DelFlag_bit=0 and f.Code_nvarchar=@CODE and f.FID_nvarchar=m.FID_nvarchar";
            plist.Add(new SqlParameter("@CODE", SqlDbType.NVarChar) { Value = code });

            List<FamilyMember> list = mapping.QueryList <FamilyMember>(sql, out errorMsg, plist);
            if (string.IsNullOrWhiteSpace(errorMsg) == false||list==null||list.Count==0)
            { 
                return null;
            }

            List<FamilyMemberVO> re = new List<FamilyMemberVO>();
            foreach (FamilyMember bean in list)
            {
                FamilyMemberVO vo = new FamilyMemberVO();
                vo.sex = bean.Sex.Value;
                vo.name = bean.Name;
                vo.age = bean.Age.Value; 
                re.Add(vo);
            }
            return re;
        }
    }
}
