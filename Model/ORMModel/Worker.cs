
using ORM;
using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;//[IgnoreDataMember]
using System.Web.Script.Serialization;//[ScriptIgnore]

namespace Model.ORMModel
{
/// <summary>
/// 
/// </summary>
[ORM(Name = "U_Worker")]
public partial class Worker : INotifyPropertyChanged
{

//传递属性变化通知
public event PropertyChangedEventHandler PropertyChanged;
/// <summary>
/// 添加UpdateColumns记录
/// </summary>
private void AddUpdateColumns(object sender, PropertyChangedEventArgs e)
{
if (m_updatecolumns == null) m_updatecolumns = new List<string>();
if (m_updatecolumns.Contains(e.PropertyName) == false) m_updatecolumns.Add(e.PropertyName);
}

public Worker()
{
//注册事件
PropertyChanged += AddUpdateColumns;
}

private List<string> m_updatecolumns;
/// <summary>
/// 更新列集合
/// </summary>
[ScriptIgnore]//使用JavaScriptSerializer序列化时不序列化此字段
[IgnoreDataMember]//使用DataContractJsonSerializer序列化时不序列化此字段
//[JsonIgnore]//使用JsonConvert序列化时不序列化此字段
public List<string> UpdateColumns
{
set { m_updatecolumns = value; }
get { return m_updatecolumns; }
}

private string m_SID;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "SID_nvarchar", IsKey = true)]
public string SID
{
set
{ 
m_SID = value;
PropertyChanged(this, new PropertyChangedEventArgs("SID"));
}
get { return m_SID; }
}

private string m_Name;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "Name_nvarchar")]
public string Name
{
set
{ 
m_Name = value;
PropertyChanged(this, new PropertyChangedEventArgs("Name"));
}
get { return m_Name; }
}

private string m_Pwd;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "Pwd_nvarchar")]
public string Pwd
{
set
{ 
m_Pwd = value;
PropertyChanged(this, new PropertyChangedEventArgs("Pwd"));
}
get { return m_Pwd; }
}

private string m_ACode;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "ACode_nvarchar")]
public string ACode
{
set
{ 
m_ACode = value;
PropertyChanged(this, new PropertyChangedEventArgs("ACode"));
}
get { return m_ACode; }
}

private string m_VIDS;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 1000, Name = "VIDS_nvarchar")]
public string VIDS
{
set
{ 
m_VIDS = value;
PropertyChanged(this, new PropertyChangedEventArgs("VIDS"));
}
get { return m_VIDS; }
}

private string m_VNames;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 1000, Name = "VNames_nvarchar")]
public string VNames
{
set
{ 
m_VNames = value;
PropertyChanged(this, new PropertyChangedEventArgs("VNames"));
}
get { return m_VNames; }
}
}
}
