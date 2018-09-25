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
   public class BMutiQuestion
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<MutiVO> getList()
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);

            string sql = @"select * from Q_Muti order by QNO_nvarchar";
            List<Muti> list = mapping.QueryList<Muti>(sql, out errorMsg);
            if (string.IsNullOrWhiteSpace(errorMsg) == false || list == null || list.Count == 0)
            {
                return null;
            }

            List<MutiVO> re = new List<MutiVO>();
            foreach (Muti item in list)
            {
                MutiVO sub = new MutiVO();
                sub.qid = item.QID;
                sub.qno = item.QNO;
                sub.img = item.Img;
                sub.question = item.Question;
                sub.a1 = item.A1;
                sub.a2 = item.A2;
                sub.a3 = item.A3;
                sub.a4 = item.A4;
                sub.a5 = item.A5;
                re.Add(sub);
            }

            return re;
        }

        /// <summary>
        /// 新增和修改
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool save(List<MutiVO> list)
        {
            if (list == null || list.Count == 0)
            {
                return true;
            }
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);

            List<Muti> addList = new List<Muti>();
            List<Muti> editList = new List<Muti>();
            foreach (MutiVO vo in list)
            {
                Muti bean = new Muti();
                bean.QNO = vo.qno;
                bean.Question = vo.question;
                bean.A1 = vo.a1;
                bean.A2 = vo.a2;
                bean.A3 = vo.a3;
                bean.A4 = vo.a4;
                bean.A5 = vo.a5; 

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
                    r = mapping.Add<Muti>(addList, out errorMsg);
                    if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                    {
                        db.RollbackTrans(out errorMsg);//回滚
                        return false;
                    }
                }

                if (editList.Count > 0)
                {
                    r = mapping.Edit<Muti>(editList, out errorMsg);
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
            string sql = @"delete from Q_Muti where QID_nvarchar=@QID";


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

        /// <summary>
        /// 更新图片
        /// </summary>
        /// <param name="qid">记录ID</param>
        /// <param name="url">图片地址</param>
        /// <returns></returns>
        public bool updateImg(string qid, string url)
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);

            Muti obj = new Muti();
            obj.QID = qid;
            obj = mapping.Query<Muti>(obj, out errorMsg);
            if (string.IsNullOrWhiteSpace(errorMsg) == false || obj == null)
            {
                return false;
            }

            obj.Img = url;

            int r = mapping.Edit<Muti>(obj, out errorMsg);
            if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
            {
                return false;
            }
            return true;
        }
    }
}

