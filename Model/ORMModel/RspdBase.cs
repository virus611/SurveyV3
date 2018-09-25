
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
[ORM(Name = "B_RspdBase")]
public partial class RspdBase : INotifyPropertyChanged
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

public RspdBase()
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

private string m_RID;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "RID_nvarchar", IsKey = true)]
public string RID
{
set
{ 
m_RID = value;
PropertyChanged(this, new PropertyChangedEventArgs("RID"));
}
get { return m_RID; }
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

private int? m_Age;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.Int32, Size = 4, Name = "Age_int")]
public int? Age
{
set
{ 
m_Age = value;
PropertyChanged(this, new PropertyChangedEventArgs("Age"));
}
get { return m_Age; }
}

private string m_Sex;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "Sex_nvarchar")]
public string Sex
{
set
{ 
m_Sex = value;
PropertyChanged(this, new PropertyChangedEventArgs("Sex"));
}
get { return m_Sex; }
}

private string m_Phone;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "Phone_nvarchar")]
public string Phone
{
set
{ 
m_Phone = value;
PropertyChanged(this, new PropertyChangedEventArgs("Phone"));
}
get { return m_Phone; }
}

private string m_KISH;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "KISH_nvarchar")]
public string KISH
{
set
{ 
m_KISH = value;
PropertyChanged(this, new PropertyChangedEventArgs("KISH"));
}
get { return m_KISH; }
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

private string m_SID;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "SID_nvarchar")]
public string SID
{
set
{ 
m_SID = value;
PropertyChanged(this, new PropertyChangedEventArgs("SID"));
}
get { return m_SID; }
}

private string m_SName;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "SName_nvarchar")]
public string SName
{
set
{ 
m_SName = value;
PropertyChanged(this, new PropertyChangedEventArgs("SName"));
}
get { return m_SName; }
}

private string m_FamilyCount;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "FamilyCount_nvarchar")]
public string FamilyCount
{
set
{ 
m_FamilyCount = value;
PropertyChanged(this, new PropertyChangedEventArgs("FamilyCount"));
}
get { return m_FamilyCount; }
}

private DateTime? m_StartTime;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.DateTime, Size = 8, Name = "StartTime_datetime")]
public DateTime? StartTime
{
set
{ 
m_StartTime = value;
PropertyChanged(this, new PropertyChangedEventArgs("StartTime"));
}
get { return m_StartTime; }
}

private DateTime? m_EndTime;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.DateTime, Size = 8, Name = "EndTime_datetime")]
public DateTime? EndTime
{
set
{ 
m_EndTime = value;
PropertyChanged(this, new PropertyChangedEventArgs("EndTime"));
}
get { return m_EndTime; }
}

private int? m_Second;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.Int32, Size = 4, Name = "Second_int")]
public int? Second
{
set
{ 
m_Second = value;
PropertyChanged(this, new PropertyChangedEventArgs("Second"));
}
get { return m_Second; }
}

private string m_Address;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 500, Name = "Address_nvarchar")]
public string Address
{
set
{ 
m_Address = value;
PropertyChanged(this, new PropertyChangedEventArgs("Address"));
}
get { return m_Address; }
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
}
}
