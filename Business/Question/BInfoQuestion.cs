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
using Business.Mobile;

namespace Business.Question
{
    public class BInfoQuestion
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<QInfoVO> getList()
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);

            string sql = @"select * from Q_Info order by QNO_nvarchar";
            List<Info> list = mapping.QueryList<Info>(sql, out errorMsg);
            if (string.IsNullOrWhiteSpace(errorMsg) == false || list == null || list.Count == 0)
            {
                return null;
            }

            List<QInfoVO> re = new List<QInfoVO>();
            foreach (Info item in list)
            {
                QInfoVO sub = new QInfoVO();
                sub.qid = item.QID;
                sub.qno = item.QNO;
                sub.answer = item.Answer;
                sub.question = item.Question;
                sub.iput = item.Put.Value;
                sub.muti = item.Muti.Value;
                sub.jump = item.Jump;  
                re.Add(sub);
            }

            return re;
        }

        public List<MInfoVO> getMobileList()
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);

            string sql = @"select * from Q_Info order by QNO_nvarchar";
            List<Info> list = mapping.QueryList<Info>(sql, out errorMsg);
            if (string.IsNullOrWhiteSpace(errorMsg) == false || list == null || list.Count == 0)
            {
                return null;
            }

            List<MInfoVO> re = new List<MInfoVO>();
            foreach (Info item in list)
            {
                MInfoVO sub = new MInfoVO();
                sub.qid = item.QID;
                sub.qno = item.QNO;
                sub.answer = item.Answer;
                sub.question = item.Question;
                sub.hand = item.Put.Value;
                sub.type = item.Muti.Value.ToString();
                sub.jump = item.Jump;
                re.Add(sub);
            }

            return re;
        }


        /// <summary>
        /// 新增和修改
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool save(List<QInfoVO> list)
        {
            if (list == null || list.Count == 0)
            {
                return true;
            }
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);

            List<Info> addList = new List<Info>();
            List<Info> editList = new List<Info>();
            foreach (QInfoVO vo in list)
            {
                Info bean = new Info();
                bean.QNO = vo.qno;
                bean.Question = vo.question;
                bean.Answer = vo.answer;
                bean.Put=vo.iput;
                bean.Muti=vo.muti;
                bean.Jump=vo.jump;

                 

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
                    r = mapping.Add<Info>(addList, out errorMsg);
                    if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                    {
                        db.RollbackTrans(out errorMsg);//回滚
                        return false;
                    }
                }

                if (editList.Count > 0)
                {
                    r = mapping.Edit<Info>(editList, out errorMsg);
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

            List<IDataParameter> plist = new List<IDataParameter>();
            plist.Add(new SqlParameter("@QID", SqlDbType.NVarChar) { Value = qid });
            string sql = @"delete from Q_Info where QID_nvarchar=@QID";


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
