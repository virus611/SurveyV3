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
    public class BCheckQuestion
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<CheckVO> getList()
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);

            string sql = @"select * from Q_Check order by QNO_nvarchar";
            List<Check> list = mapping.QueryList<Check>(sql, out errorMsg);
            if (string.IsNullOrWhiteSpace(errorMsg) == false||list==null||list.Count==0)
            {
                return null;
            }

            List<CheckVO> re = new List<CheckVO>();
            foreach (Check item in list)
            {
                CheckVO sub=new CheckVO();
                sub.qid=item.QID;
                sub.qno=item.QNO;
                sub.img=item.Img;
                sub.question = item.Question;
                sub.a1=item.A1;
                sub.a2=item.A2;
                re.Add(sub); 
            }

            return re;
        }

        /// <summary>
        /// 新增和修改
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool save(List<CheckVO> list)
        {
            if (list == null || list.Count == 0)
            {
                return true;
            }
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;
            IMapping mapping = new SqlMapping(dbstr);

            List<Check> addList = new List<Check>();
            List<Check> editList = new List<Check>();
            foreach (CheckVO vo in list)
            {
                Check bean = new Check();
                bean.QNO = vo.qno;
                bean.Question = vo.question;
                bean.A1 = vo.a1;
                bean.A2 = vo.a2;
                 
                if (string.IsNullOrWhiteSpace(vo.qid))
                {
                    bean.Img = "";
                    bean.QID = Tools.createID();
                    addList.Add(bean);
                }else
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
                    r = mapping.Add<Check>(addList, out errorMsg);
                    if (string.IsNullOrWhiteSpace(errorMsg) == false || r==0)
                    {
                        db.RollbackTrans(out errorMsg);//回滚
                        return false;
                    }
                }

                if (editList.Count > 0)
                {
                    r = mapping.Edit<Check>(editList, out errorMsg);
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
            string sql = @"delete from Q_Check where QID_nvarchar=@QID";


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

            Check obj = new Check();
            obj.QID = qid;
            obj = mapping.Query<Check>(obj, out errorMsg);
            if (string.IsNullOrWhiteSpace(errorMsg) == false || obj == null  )
            {
                return false;
            }

            obj.Img = url;

            int r = mapping.Edit<Check>(obj, out errorMsg);
            if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
            {
                return false;
            } 
            return true;
        }
    }
}
