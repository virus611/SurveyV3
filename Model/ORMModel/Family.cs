
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
[ORM(Name = "B_Family")]
public partial class Family : INotifyPropertyChanged
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

public Family()
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

private string m_FID;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "FID_nvarchar", IsKey = true)]
public string FID
{
set
{ 
m_FID = value;
PropertyChanged(this, new PropertyChangedEventArgs("FID"));
}
get { return m_FID; }
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

private string m_TCode;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "TCode_nvarchar")]
public string TCode
{
set
{ 
m_TCode = value;
PropertyChanged(this, new PropertyChangedEventArgs("TCode"));
}
get { return m_TCode; }
}

private string m_VCode;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "VCode_nvarchar")]
public string VCode
{
set
{ 
m_VCode = value;
PropertyChanged(this, new PropertyChangedEventArgs("VCode"));
}
get { return m_VCode; }
}

private string m_FCode;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "FCode_nvarchar")]
public string FCode
{
set
{ 
m_FCode = value;
PropertyChanged(this, new PropertyChangedEventArgs("FCode"));
}
get { return m_FCode; }
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

private string m_G01;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "G01_nvarchar")]
public string G01
{
set
{ 
m_G01 = value;
PropertyChanged(this, new PropertyChangedEventArgs("G01"));
}
get { return m_G01; }
}

private string m_LastCode;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "LastCode_nvarchar")]
public string LastCode
{
set
{ 
m_LastCode = value;
PropertyChanged(this, new PropertyChangedEventArgs("LastCode"));
}
get { return m_LastCode; }
}

private string m_LastText;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "LastText_nvarchar")]
public string LastText
{
set
{ 
m_LastText = value;
PropertyChanged(this, new PropertyChangedEventArgs("LastText"));
}
get { return m_LastText; }
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
