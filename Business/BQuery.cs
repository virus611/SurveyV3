using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using DBHelper;
using System.Data;
using System.Data.SqlClient;

namespace Business
{
    public class BQuery
    {
        /// <summary>
        /// 返回datatable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="plist"></param>
        /// <returns></returns>
        public DataTable getDataTable(string sql, List<IDataParameter> plist = null)
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;

            using (IDataBase db = new SqlDBHelper(dbstr, out errorMsg))
            {
                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                    return null;
                db.OpenConn(out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                    return null;
                if (plist != null && plist.Count > 0)
                {
                    db.AddParameters(plist, true);
                }
                DataTable dt = db.RunSqlDataTable(sql, out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                {
                    return null;
                }
                return dt;
            }
        }


        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="plist"></param>
        /// <returns></returns>
        public bool executeSQL(string sql, List<IDataParameter> plist = null)
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;

            using (IDataBase db = new SqlDBHelper(dbstr, out errorMsg))
            {
                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                    return false;
                db.OpenConn(out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                    return false;
                if (plist != null && plist.Count > 0)
                {
                    db.AddParameters(plist, true);
                }
                int r = db.RunSqlNonQuery(sql, out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                {
                    return false;
                }
                return true;
            }
        }

        public   bool executeSQLList(List<string> list, List<IDataParameter> plist = null)
        {
            if (list == null || list.Count == 0)
            {
                return true;
            }
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;

            using (IDataBase db = new SqlDBHelper(dbstr, out errorMsg))
            {
                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                    return false;
                db.OpenConn(out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                    return false;
                db.OpenTrans(out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                {
                    return false;
                }

                if (plist != null && plist.Count > 0)
                {
                    db.AddParameters(plist, true);
                }
                foreach (string sql in list)
                {
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

                return true;
            }
        }

        /// <summary>
        /// 统计用查询，返回int类型
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="plist"></param>
        /// <returns></returns>
        public int queryScalarInt(string sql, List<IDataParameter> plist = null)
        {
            string dbstr = Tools.GetECConnStr();
            string errorMsg = string.Empty;

            using (IDataBase db = new SqlDBHelper(dbstr, out errorMsg))
            {
                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                    return 0;
                db.OpenConn(out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                    return 0;
                if (plist != null && plist.Count > 0)
                {
                    db.AddParameters(plist, true);
                }

                object o = db.RunSqlScalar(sql, out errorMsg);
                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                {
                    return 0;
                }
                if (o == null)
                {
                    return 0;
                }
                return Convert.ToInt16(o);
            }
        }




    }
}
