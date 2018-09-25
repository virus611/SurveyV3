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


namespace Business.Question
{
    public class BSceneSubQuestion
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<SceneSubVO> getList(string pid)
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);

            string sql = @"select * from Q_ScenceSub where PID_nvarchar=@PID order by QNO_nvarchar";
            List<IDataParameter> plist = new List<IDataParameter>();
            plist.Add(new SqlParameter("@PID", SqlDbType.NVarChar) { Value = pid });

            List<ScenceSub> list = mapping.QueryList<ScenceSub>(sql, out errorMsg, plist);
            if (string.IsNullOrWhiteSpace(errorMsg) == false || list == null || list.Count == 0)
            {
                return null;
            }

            List<SceneSubVO> re = new List<SceneSubVO>();
            foreach (ScenceSub item in list)
            {
                SceneSubVO sub = new SceneSubVO();
                sub.qid = item.QID;
                sub.qno = item.QNO;
                sub.question = item.Question;
                sub.type = item.Type.Value;
                sub.a1 = item.A1;
                sub.a2 = item.A2;
                sub.a3 = item.A3;
                sub.a4 = item.A4;
                sub.a5 = item.A5;
                re.Add(sub);
            }

            return re;
        }


        public List<SceneSubVO> getDetailList( )
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);

            string sql = @"select * from Q_Scence order by QID_nvarchar";
            List<Scence> mainlist = mapping.QueryList<Scence>(sql, out errorMsg);
            if (string.IsNullOrWhiteSpace(errorMsg) == false || mainlist == null || mainlist.Count == 0)
            {
                return null;
            }

            List<SceneSubVO> re = new List<SceneSubVO>();
            foreach (Scence sc in mainlist)
            {
                sql = @"select * from Q_ScenceSub where PID_nvarchar=@PID order by QNO_nvarchar";
                List<IDataParameter> plist = new List<IDataParameter>();
                plist.Add(new SqlParameter("@PID", SqlDbType.NVarChar) { Value = sc.QID });

                List<ScenceSub> list = mapping.QueryList<ScenceSub>(sql, out errorMsg, plist);
                if (string.IsNullOrWhiteSpace(errorMsg) == false || list == null || list.Count == 0)
                {
                    continue;
                }
                 
                foreach (ScenceSub item in list)
                {
                    SceneSubVO sub = new SceneSubVO();
                    sub.qid = item.QID;
                    sub.qno = item.QNO;
                    sub.question = sc.Question;
                    sub.subquestion = item.Question; 
                    sub.type = item.Type.Value;
                    sub.a1 = item.A1;
                    sub.a2 = item.A2;
                    sub.a3 = item.A3;
                    sub.a4 = item.A4;
                    sub.a5 = item.A5;
                    re.Add(sub);
                } 
            } 
            return re;
        }
        /// <summary>
        /// 新增和修改
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool save(string pid, List<SceneSubVO> list)
        {
            if (list == null || list.Count == 0)
            {
                return true;
            }
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);

            List<ScenceSub> addList = new List<ScenceSub>();
            List<ScenceSub> editList = new List<ScenceSub>();
            foreach (SceneSubVO vo in list)
            {
                ScenceSub bean = new ScenceSub();
                bean.QNO = vo.qno;
                bean.Question = vo.question;
                bean.Type = vo.type;
                bean.A1 = vo.a1;
                bean.A2 = vo.a2;
                bean.A3 = vo.a3;
                bean.A4 = vo.a4;
                bean.A5 = vo.a5;
                bean.PID = pid;

                if (string.IsNullOrWhiteSpace(vo.qid))
                {
                    bean.Img = "";
                    bean.QID = Tools.createID();
                    addList.Add(bean);
                }
                else
                {
                    bean.QID = vo.qid;
                    editList.Add(bean);
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

                int r = 0;
                if (addList.Count > 0)
                {
                    r = mapping.Add<ScenceSub>(addList, out errorMsg);
                    if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                    {
                        db.RollbackTrans(out errorMsg);//回滚
                        return false;
                    }
                }

                if (editList.Count > 0)
                {
                    r = mapping.Edit<ScenceSub>(editList, out errorMsg);
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
        /// 删除
        /// </summary>
        /// <param name="qid"></param>
        /// <returns></returns>
        public bool del(string qid)
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);

            List<IDataParameter> plist = new List<IDataParameter>();
            plist.Add(new SqlParameter("@QID", SqlDbType.NVarChar) { Value = qid });
            string sql = @"delete from Q_ScenceSub where QID_nvarchar=@QID";


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


    }
}
