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
    public class BSceneQuestion
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<SceneVO> getList()
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);

            string sql = @"select * from Q_Scence order by QID_nvarchar";
            List<Scence> list = mapping.QueryList<Scence>(sql, out errorMsg);
            if (string.IsNullOrWhiteSpace(errorMsg) == false || list == null || list.Count == 0)
            {
                return null;
            }

            List<SceneVO> re = new List<SceneVO>();
            foreach (Scence item in list)
            {
                SceneVO sub = new SceneVO();
                sub.qid = item.QID;
                sub.question = item.Question;
                re.Add(sub);
            }

            return re;
        }

        /// <summary>
        /// 新增和修改
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool save(List<SceneVO> list)
        {
            if (list == null || list.Count == 0)
            {
                return true;
            }
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);

            List<Scence> addList = new List<Scence>();
            List<Scence> editList = new List<Scence>();
            foreach (SceneVO vo in list)
            {
                Scence bean = new Scence();
                bean.Question = vo.question;

                if (string.IsNullOrWhiteSpace(vo.qid))
                {

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
                    r = mapping.Add<Scence>(addList, out errorMsg);
                    if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                    {
                        db.RollbackTrans(out errorMsg);//回滚
                        return false;
                    }
                }

                if (editList.Count > 0)
                {
                    r = mapping.Edit<Scence>(editList, out errorMsg);
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
            string sql = @"delete from Q_Scence where QID_nvarchar=@QID";
            string sql2 = @"delete from Q_ScenceSub where PID_nvarchar=@QID";

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

                db.AddParameters(plist, true);
                int r = db.RunSqlNonQuery(sql, out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                {
                    db.RollbackTrans(out errorMsg);//回滚
                    return false;
                }

                db.RunSqlNonQuery(sql2, out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false)
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
