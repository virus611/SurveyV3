
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
[ORM(Name = "B_ResultPerson")]
public partial class ResultPerson : INotifyPropertyChanged
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

public ResultPerson()
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

private string m_FID;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "FID_nvarchar")]
public string FID
{
set
{ 
m_FID = value;
PropertyChanged(this, new PropertyChangedEventArgs("FID"));
}
get { return m_FID; }
}

private string m_Code;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "Code_nvarchar")]
public string Code
{
set
{ 
m_Code = value;
PropertyChanged(this, new PropertyChangedEventArgs("Code"));
}
get { return m_Code; }
}

private string m_Result;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "Result_nvarchar")]
public string Result
{
set
{ 
m_Result = value;
PropertyChanged(this, new PropertyChangedEventArgs("Result"));
}
get { return m_Result; }
}

private string m_Text;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "Text_nvarchar")]
public string Text
{
set
{ 
m_Text = value;
PropertyChanged(this, new PropertyChangedEventArgs("Text"));
}
get { return m_Text; }
}

private DateTime? m_Time;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.DateTime, Size = 8, Name = "Time_datetime")]
public DateTime? Time
{
set
{ 
m_Time = value;
PropertyChanged(this, new PropertyChangedEventArgs("Time"));
}
get { return m_Time; }
}

private bool? m_DelFlag;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.Boolean, Size = 1, Name = "DelFlag_bit")]
public bool? DelFlag
{
set
{ 
m_DelFlag = value;
PropertyChanged(this, new PropertyChangedEventArgs("DelFlag"));
}
get { return m_DelFlag; }
}

private DateTime? m_DelTime;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.DateTime, Size = 8, Name = "DelTime_datetime")]
public DateTime? DelTime
{
set
{ 
m_DelTime = value;
PropertyChanged(this, new PropertyChangedEventArgs("DelTime"));
}
get { return m_DelTime; }
}

private string m_GID;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "GID_nvarchar")]
public string GID
{
set
{ 
m_GID = value;
PropertyChanged(this, new PropertyChangedEventArgs("GID"));
}
get { return m_GID; }
}
}
}
