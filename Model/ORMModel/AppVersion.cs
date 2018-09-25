
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
[ORM(Name = "S_AppVersion")]
public partial class AppVersion : INotifyPropertyChanged
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

public AppVersion()
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

private string m_ID;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "ID_nvarchar", IsKey = true)]
public string ID
{
set
{ 
m_ID = value;
PropertyChanged(this, new PropertyChangedEventArgs("ID"));
}
get { return m_ID; }
}

private int? m_Least;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.Int32, Size = 4, Name = "Least_int")]
public int? Least
{
set
{ 
m_Least = value;
PropertyChanged(this, new PropertyChangedEventArgs("Least"));
}
get { return m_Least; }
}

private int? m_Current;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.Int32, Size = 4, Name = "Current_int")]
public int? Current
{
set
{ 
m_Current = value;
PropertyChanged(this, new PropertyChangedEventArgs("Current"));
}
get { return m_Current; }
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

private string m_Url;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 500, Name = "Url_nvarchar")]
public string Url
{
set
{ 
m_Url = value;
PropertyChanged(this, new PropertyChangedEventArgs("Url"));
}
get { return m_Url; }
}

private DateTime? m_CreateTime;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.DateTime, Size = 8, Name = "CreateTime_datetime")]
public DateTime? CreateTime
{
set
{ 
m_CreateTime = value;
PropertyChanged(this, new PropertyChangedEventArgs("CreateTime"));
}
get { return m_CreateTime; }
}
}
}
