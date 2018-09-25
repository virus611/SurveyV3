
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
[ORM(Name = "B_RspdTime")]
public partial class RspdTime : INotifyPropertyChanged
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

public RspdTime()
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

private string m_RID;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "RID_nvarchar")]
public string RID
{
set
{ 
m_RID = value;
PropertyChanged(this, new PropertyChangedEventArgs("RID"));
}
get { return m_RID; }
}

private string m_Time;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "Time_nvarchar")]
public string Time
{
set
{ 
m_Time = value;
PropertyChanged(this, new PropertyChangedEventArgs("Time"));
}
get { return m_Time; }
}

private bool? m_Start;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.Boolean, Size = 1, Name = "Start_bit")]
public bool? Start
{
set
{ 
m_Start = value;
PropertyChanged(this, new PropertyChangedEventArgs("Start"));
}
get { return m_Start; }
}
}
}
