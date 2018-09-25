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
  public   class BArea
  {
      #region  地区 


      /// <summary>
      /// 返回地区列表
      /// </summary>
      /// <returns></returns>
      public List<AreaVO> getAreaList()
      {
          string dbstr = Tools.GetECConnStr();
          string errorMsg = string.Empty;
          IMapping mapping = new SqlMapping(dbstr);

          string sql = @"select * from S_Area order by Code_nvarchar";
          List<Area> list = mapping.QueryList<Area>(sql, out errorMsg);
          if (string.IsNullOrWhiteSpace(errorMsg) == false || list == null || list.Count == 0)
          {
              return null;
          }

          List<AreaVO> re = new List<AreaVO>();
          foreach (Area item in list)
          {
              AreaVO sub = new AreaVO();
              sub.aid = item.AID;
              sub.code = item.Code;
              sub.name = item.Name; 
              re.Add(sub);
          } 
          return re;
      }

      /// <summary>
      /// 保存地区
      /// </summary>
      /// <param name="list"></param>
      /// <returns></returns>
      public bool saveArea(List<AreaVO> list)
      {
          if (list == null || list.Count == 0)
          {
              return true;
          }
          string dbstr = Tools.GetECConnStr();
          string errorMsg = string.Empty;
          IMapping mapping = new SqlMapping(dbstr);

          List<Area> addList = new List<Area>();
          List<Area> editList = new List<Area>();
          foreach (AreaVO vo in list)
          {
              Area bean = new Area();
              bean.Code = vo.code;
              bean.Name = vo.name;
               
              if (string.IsNullOrWhiteSpace(vo.aid))
              { 
                  bean.AID = Tools.createID();
                  addList.Add(bean);
              }
              else
              {
                  bean.AID = vo.aid;
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
                  r = mapping.Add<Area>(addList, out errorMsg);
                  if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                  {
                      db.RollbackTrans(out errorMsg);//回滚
                      return false;
                  }
              }

              if (editList.Count > 0)
              {
                  r = mapping.Edit<Area>(editList, out errorMsg);
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
      /// 删除地区(地区、乡镇）
      /// </summary>
      /// <param name="aid"></param>
      /// <returns></returns>
      public bool delArea(string aid)
      {
          string dbstr = Tools.GetECConnStr();
          string errorMsg = string.Empty; 
          
          IMapping mapping = new SqlMapping(dbstr);
           
          List<IDataParameter> plist = new List<IDataParameter>();
          plist.Add(new SqlParameter("@AID", SqlDbType.NVarChar) { Value = aid });
           
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
              string sql = @"delete from S_Area where AID_nvarchar=@AID";
              db.AddParameters(plist,true);
              int r = db.RunSqlNonQuery(sql, out errorMsg);
              if (string.IsNullOrWhiteSpace(errorMsg) == false )
              {
                  db.RollbackTrans(out errorMsg);//回滚 
                  return false;
              }

              //乡镇
              sql = @"delete from S_Town where AID_nvarchar=@AID";
              r = db.RunSqlNonQuery(sql, out errorMsg);
              if (string.IsNullOrWhiteSpace(errorMsg) == false )
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

      #endregion


      #region  乡镇
      /// <summary>
      /// 返回乡镇列表
      /// </summary>
      /// <returns></returns>
      public List<TownVO> getTownList(string aid)
      {
          string dbstr = Tools.GetECConnStr();
          string errorMsg = string.Empty;
          IMapping mapping = new SqlMapping(dbstr);

          List<IDataParameter> plist = new List<IDataParameter>();
          plist.Add(new SqlParameter("@AID", SqlDbType.NVarChar) { Value = aid });

          string sql = @"select * from S_Town where AID_nvarchar=@AID order by Code_nvarchar";
          List<Town> list = mapping.QueryList<Town>(sql, out errorMsg,plist);
          if (string.IsNullOrWhiteSpace(errorMsg) == false || list == null || list.Count == 0)
          {
              return null;
          }

          List<TownVO> re = new List<TownVO>();
          foreach (Town item in list)
          {
              TownVO sub = new TownVO();
              sub.aid = item.AID;
              sub.tid = item.TID;
              sub.code = item.Code;
              sub.name = item.Name;
              re.Add(sub);
          }
          return re;
      }

      /// <summary>
      /// 保存地区
      /// </summary>
      /// <param name="list"></param>
      /// <returns></returns>
      public bool saveTown(string aid,List<TownVO> list)
      {
          if (list == null || list.Count == 0)
          {
              return true;
          }
          string dbstr = Tools.GetECConnStr();
          string errorMsg = string.Empty;
          IMapping mapping = new SqlMapping(dbstr);

          List<Town> addList = new List<Town>();
          List<Town> editList = new List<Town>();
          foreach (TownVO vo in list)
          {
              Town bean = new Town();
              bean.Code = vo.code;
              bean.Name = vo.name;
               
              if (string.IsNullOrWhiteSpace(vo.tid))
              {
                  bean.AID = aid;
                  bean.TID = Tools.createID();
                  addList.Add(bean);
              }
              else
              {
                  bean.TID = vo.tid;
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
                  r = mapping.Add<Town>(addList, out errorMsg);
                  if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                  {
                      db.RollbackTrans(out errorMsg);//回滚
                      return false;
                  }
              }

              if (editList.Count > 0)
              {
                  r = mapping.Edit<Town>(editList, out errorMsg);
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
      /// 删除乡镇(乡镇、村）
      /// </summary>
      /// <param name="aid"></param>
      /// <returns></returns>
      public bool delTown(string tid)
      {
          string dbstr = Tools.GetECConnStr();
          string errorMsg = string.Empty;

          IMapping mapping = new SqlMapping(dbstr);

          List<IDataParameter> plist = new List<IDataParameter>();
          plist.Add(new SqlParameter("@TID", SqlDbType.NVarChar) { Value = tid });

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
              string sql = @"delete from S_Town where TID_nvarchar=@TID";
              db.AddParameters(plist, true);
              int r = db.RunSqlNonQuery(sql, out errorMsg);
              if (string.IsNullOrWhiteSpace(errorMsg) == false)
              {
                  db.RollbackTrans(out errorMsg);//回滚 
                  return false;
              }

              //村
              sql = @"delete from S_Village where TID_nvarchar=@TID";
              r = db.RunSqlNonQuery(sql, out errorMsg);
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


      #endregion

      #region  村

      /// <summary>
      /// 返回村列表
      /// </summary>
      /// <returns></returns>
      public List<VillageVO> getVillageList(string tid)
      {
          string dbstr = Tools.GetECConnStr();
          string errorMsg = string.Empty;
          IMapping mapping = new SqlMapping(dbstr);

          List<IDataParameter> plist = new List<IDataParameter>();
          plist.Add(new SqlParameter("@TID", SqlDbType.NVarChar) { Value = tid });

          string sql = @"select * from S_Village where TID_nvarchar=@TID order by Code_nvarchar";
          List<Village> list = mapping.QueryList<Village>(sql, out errorMsg,plist);
          if (string.IsNullOrWhiteSpace(errorMsg) == false || list == null || list.Count == 0)
          {
              return null;
          }

          List<VillageVO> re = new List<VillageVO>();
          foreach (Village item in list)
          {
              VillageVO sub = new VillageVO();
              sub.vid = item.VID;
              sub.tid = item.TID;
              sub.code = item.Code;
              sub.name = item.Name;
              re.Add(sub);
          }
          return re;
      }

      /// <summary>
      /// 保存地区
      /// </summary>
      /// <param name="list"></param>
      /// <returns></returns>
      public bool saveVillage(string tid, List<VillageVO> list)
      {
          if (list == null || list.Count == 0)
          {
              return true;
          }
          string dbstr = Tools.GetECConnStr();
          string errorMsg = string.Empty;
          IMapping mapping = new SqlMapping(dbstr);

          List<Village> addList = new List<Village>();
          List<Village> editList = new List<Village>();
          foreach (VillageVO vo in list)
          {
              Village bean = new Village();
              bean.Code = vo.code;
              bean.Name = vo.name;

              if (string.IsNullOrWhiteSpace(vo.tid))
              {
                  bean.TID = tid;
                  bean.VID = Tools.createID();
                  addList.Add(bean);
              }
              else
              {
                  bean.VID = vo.vid;
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
                  r = mapping.Add<Village>(addList, out errorMsg);
                  if (string.IsNullOrWhiteSpace(errorMsg) == false || r == 0)
                  {
                      db.RollbackTrans(out errorMsg);//回滚
                      return false;
                  }
              }

              if (editList.Count > 0)
              {
                  r = mapping.Edit<Village>(editList, out errorMsg);
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
      /// 删除村
      /// </summary>
      /// <param name="aid"></param>
      /// <returns></returns>
      public bool delVillage(string vid)
      {
          string dbstr = Tools.GetECConnStr();
          string errorMsg = string.Empty;

          IMapping mapping = new SqlMapping(dbstr);

          List<IDataParameter> plist = new List<IDataParameter>();
          plist.Add(new SqlParameter("@VID", SqlDbType.NVarChar) { Value = vid });

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
              string sql = @"delete from S_Village where VID_nvarchar=@VID";
              db.AddParameters(plist, true);
              int r = db.RunSqlNonQuery(sql, out errorMsg);
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

      #endregion


      #region 下拉列表用

      /// <summary>
      /// 地区下拉
      /// </summary>
      /// <returns></returns>
      public List<AreaCombVO> getAreaCombList()
      {
          string dbstr = Tools.GetECConnStr();
          string errorMsg = string.Empty;
          IMapping mapping = new SqlMapping(dbstr);

          string sql = @"select * from S_Area order by Code_nvarchar";
          List<Area> list = mapping.QueryList<Area>(sql, out errorMsg);
          if (string.IsNullOrWhiteSpace(errorMsg) == false || list == null || list.Count == 0)
          {
              return null;
          }

          List<AreaCombVO> re = new List<AreaCombVO>();
          foreach (Area item in list)
          {
              AreaCombVO sub = new AreaCombVO();
              sub.id = item.AID;
              sub.code = item.Code;
              sub.name = item.Name;
              re.Add(sub);
          }
          return re;
      }

      public List<AreaCombVO> getTownCombList(string aid)
      {
          string dbstr = Tools.GetECConnStr();
          string errorMsg = string.Empty;
          IMapping mapping = new SqlMapping(dbstr);

          List<IDataParameter> plist = new List<IDataParameter>();
          plist.Add(new SqlParameter("@AID", SqlDbType.NVarChar) { Value = aid });
            
          string sql = @"select * from S_Town where AID_nvarchar=@AID order by Code_nvarchar";
          List<Town> list = mapping.QueryList<Town>(sql, out errorMsg,plist);
          if (string.IsNullOrWhiteSpace(errorMsg) == false || list == null || list.Count == 0)
          {
              return null;
          }

          List<AreaCombVO> re = new List<AreaCombVO>();
          foreach (Town item in list)
          {
              AreaCombVO sub = new AreaCombVO();
              sub.id = item.TID;
              sub.code = item.Code;
              sub.name = item.Name;
              re.Add(sub);
          }
          return re;
      }

      public List<AreaCombVO> getTownCombListByACode(string acode)
      {
          string dbstr = Tools.GetECConnStr();
          string errorMsg = string.Empty;
          IMapping mapping = new SqlMapping(dbstr);

          List<IDataParameter> plist = new List<IDataParameter>();
          plist.Add(new SqlParameter("@ACODE", SqlDbType.NVarChar) { Value = acode });

          string sql = @"select t.* from S_Town t,S_Area a where a.Code_nvarchar=@ACODE and a.AID_nvarchar=t.AID_nvarchar order by t.Code_nvarchar";
          List<Town> list = mapping.QueryList<Town>(sql, out errorMsg, plist);
          if (string.IsNullOrWhiteSpace(errorMsg) == false || list == null || list.Count == 0)
          {
              return null;
          }

          List<AreaCombVO> re = new List<AreaCombVO>();
          foreach (Town item in list)
          {
              AreaCombVO sub = new AreaCombVO();
              sub.id = item.TID;
              sub.code = item.Code;
              sub.name = item.Name;
              re.Add(sub);
          }
          return re;
      }


      public List<AreaCombVO> getVillageCombList(string tid)
      {
          string dbstr = Tools.GetECConnStr();
          string errorMsg = string.Empty;
          IMapping mapping = new SqlMapping(dbstr);

          List<IDataParameter> plist = new List<IDataParameter>();
          plist.Add(new SqlParameter("@TID", SqlDbType.NVarChar) { Value = tid });

          string sql = @"select * from S_Village where TID_nvarchar=@TID order by Code_nvarchar";
          List<Village> list = mapping.QueryList<Village>(sql, out errorMsg, plist);
          if (string.IsNullOrWhiteSpace(errorMsg) == false || list == null || list.Count == 0)
          {
              return null;
          }

          List<AreaCombVO> re = new List<AreaCombVO>();
          foreach (Village item in list)
          {
              AreaCombVO sub = new AreaCombVO();
              sub.id = item.VID;
              sub.code = item.Code;
              sub.name = item.Name;
              re.Add(sub);
          }
          return re;
      }


      #endregion
  }
}
